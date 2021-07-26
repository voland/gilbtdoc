## Sterowanie tablicą GilBT rgb poprzez protokół udp/ip 

Sterowanie dokonujemy wysyłając komendy do tablicy, używamy do tego połączenia lan oraz protokołu udp/ip. W dowolnym języku programowania możemy utworzyć takie połączenie używając tak zwanych socketów. Możemy też użyć komendy "nc" (programu netcat) w terminalu linux oraz windows do wysyłania komend bez tworzenia żadnego dodatkowego oprogramowania.

### Komendy wysyłane do urządzenia:

- Sterowanie treścią tablicy pakietami udp/ip ( dane do 1,5kb )  
	JSONPAGE: <_content_>  
	przykład1:  
	>`JSONPAGE:{"ver":1,"elements":[{"color":32,"width":96,"height":16,"type":"rectangle","x":0,"y":0},{"content":"2019-10-18 14:18:46","color":-65536,"fontsize":8,"fonttype":1,"type":"line","x":0,"y":0},{"content":"Tekst!","color":65280,"fontsize":16,"fonttype":2,"type":"line","x":0,"y":0}]}`

	**Uwaga**, kolor podany w zmiennej integer 32bit ARGB, podstawowe czcionki to 0 i 1 fontnormal i fontfat, zestaw czcionek może być zmieniony po kontakcie z działem firmy gilbt.  
    Przykładowo 0x00ff0000 przekonwertowany do zmiennej int oznacza kolor czerwony  
    Przykładowo 0x0000ff00 przekonwertowany do zmiennej int oznacza kolor zielony  
    Przykładowo 0x000000ff przekonwertowany do zmiennej int oznacza kolor niebieski  

- Zmiana kontrastu/kontrastu nocnego w skali od 1-4  
	Kontrast4!  
	KontrastN1!  

- Ustawienie zegara wewnętrznego:  
	TIME:2019-10-18 15:37:00!

- Dhcp Wyłącz/Włącz   ( Net Mode )  
	NM0  
	NM1  

- Adres IP  
	MIP192.168.1.205

- Maska  
	MA255.255.255.0

- Brama Domyślna  
	GW192.168.1.1

- Reset, po zmianie ustawień sieci trzeba zresetować tablicę komendą:  
	RESET

- Zmiana nazwy ekranu widocznej na przykład w programie LPD  
	NAZWA:ekran

- ustawienie godziny wyłączenia/włączenia automatycznego ( gdy same zera to funkcja nieaktywna)  
	CW1:0-6:0!

Sterowanie komendą netcat może wyglądać następująco:  
> nc -u _addressip_ _port_ < _file_with_command_  

przykład2 ( w tym przypadku plik data.txt zawiera treść z przykładu 1. Port sterowania to 8888.)  

>nc -u 192.168.1.147 8888 < data.txt

### Sterowanie tablicą poprzez połączenie tcp/ip ( dane json powyżej 1,5kb )

- Ponieważ sercem tablicy led jest płytka z mikrokontrolerem a te posiadają swoje ograniczenia, tablica nie przyjmuje pakietów udp powyżej 1,5kb aby wysłać takie dane należy zrobić poprzez połączenie tcp/ip.  

- Wszystkie komendy, które można wysłać drogą udp/ip można również wysłać drogą tcp/ip aby to zrobić należy znać numer otwartego portu tcp danej tablicy, numer portu wyznacza się wedle wzoru ((UID) modulo 1000)+2, dla tablicy o znanym numerze uid 5308452 port wynosi: 8452+2 = 8454. ( Numer UID to stały numer seryjny sterownika osobny dla każdej tablicy). Podobnie jak w przypadku wysyłania drogą udp można korzystać bibliotek socketów dowolnego języka programowania lub wysłać dane przy pomocy programu netcat (komendy nc w konsoli linux).  
Przykład wysłania komendy "RESET" do tablicy drogą tcp/ip przy pomocy konsoli bash/linux:  
 >`printf "RESET" | nc -w 2 192.168.1.12 8452`  

 - Wysyłanie dowolnego pliku na kartę pamięci tablicy.  
 W celu wysłania pliku na tablicę led, należy otworzyć dwa połączenia, połączenie do wysłania komendy oraz do wysłania danych, podobnie jak ma to miejsce w przypadku komunikacji FTP. Po otwarciu portu komend *((UID) modulo 1000)+2*, i wydaniu polecenia `send` tablica otworzy port o numerze *((UID) modulo 1000)+3* na, który można wysłać dane, po czym zamknąć oba połączenia. 

- Jeżeli wysłany powyższą metodą plik będzie posiadał nazwę "rgb_cm4.frm" zostanie on potraktowany jako nowy firmware tablicy i tablica zresetuje się celem zaktualizowania oprogramowania.

Przykładowy skrypt shell "flash.sh" do wysłania pliku firmware.

>`PORT1=$(($2+0)`  
>`PORT2=$(($2+1)`  
>`printf "send rgb_cm4.frm\n" | nc -w 5 $1 $PORT1 &`  
>`sleep `  
>`printf "Sending firmware file`  
>`nc -w 5 $1 $PORT2 < $`  

    Parametr -w określa wartość czasu jaki netcat ma czekać do przedawnienia połączenia w przypadku braku odpowiedzi tablicy.  

Wywołanie skryptu może wyglądać następująco:  
`./flash.sh 192.168.1.12 8454 rgb_cm4.frm`  

#### Wysyłanie danych do wyświetlenia drogą tcp/ip
Aby wysłać dane json przekraczające 1,5kb należy skorzystać z metody podobnej jak w przypadku wysyłania pliku, tyle że stosujemy komendę *page* zamiast *send*. Aby wysłać plik page.json o przykładowej zawartości:  

>`{"ver":1,"elements":[{"color":127,"width":120,"height":220,"type":"rectangle","x":0,"y":0},`  
>`{"content":"linia1","color":-65536,"fontsize":8,"fonttype":1,"type":"line","x":40,"y":8},`  
>`{"content":"linia2","color":-65536,"fontsize":8,"fonttype":1,"type":"line","x":40,"y":16},`  
>`{"content":"linia3","color":-65536,"fontsize":8,"fonttype":1,"type":"line","x":40,"y":24},`  
>`{"content":"linia4","color":-65536,"fontsize":8,"fonttype":1,"type":"line","x":40,"y":32},`  
>`{"content":"linia6","color":-65536,"fontsize":8,"fonttype":1,"type":"line","x":40,"y":8},`  
>`{"content":"linia7","color":-65536,"fontsize":8,"fonttype":1,"type":"line","x":40,"y":16},`  
>`{"content":"linia8","color":-65536,"fontsize":8,"fonttype":1,"type":"line","x":40,"y":24},`  
>`{"content":"linia9","color":-65536,"fontsize":8,"fonttype":1,"type":"line","x":40,"y":32},`  
>`{"content":"linia10","color":-65536,"fontsize":8,"fonttype":1,"type":"line","x":0,"y":8},`  
>`{"content":"Linia11","color":65280,"fontsize":16,"fonttype":2,"type":"line","x":0,"y":0}]}`  


Należy uruchomić skrypt "upload_page.sh" o treści:

>`PORT1=$(($2+0))`  
>`PORT2=$(($2+1))`  
>`printf "page\n" | nc -w 2 $1 $PORT1 & `  
>`sleep 4 `  
>`printf "Sending page..."`  
>`nc -w 2 $1 $PORT2 < $3`  

Wywołanie skryptu może wyglądać następująco:  
`./upload_page.sh 192.168.1.12 8454 page.json`  

## Pobieranie konfiguracji sterownika
Pobieranie danych konfiguracyjnych ze sterownika dokonujemy poprzez nasłuchiwanie na porcie 6001 udp/ip. Każda tablica z sterownikiem rgb dokonuje rozgłaszania informacji o własnej konfiguracji. Informacje te zawarte są w uproszczonym formacie XML, przykład takiego pakietu danych znajduje się poniżej.

> \<AnnVer2\>\<N\>Dom\</N\>\<V\>master_2020-10-06_35959e2\</V\>\<D\>p7s8RGB200Mhz8bit\</D\>\<S\>128x32\</S\>\<U\>3670045\</U\>\<SIP\>192.168.1.239\</SIP\>\<K\>4\</K\>\<KN\>1\</KN\>\<PS\>4\</PS\>\<CW\>\<HOff\>0\</HOff\>\<MOff\>0\</MOff\>\<HOn\>0\</HOn\>\<MOn\>0\</MOn\>\</CW\>\<Sy\>false\</Sy\>\<OP\>true\</OP\>\<MPV\>6\</MPV\>\<StIP\>true\</StIP\>\<NM\>255.255.255.0\</NM\>\<GW\>192.168.1.1\</GW\>\<IP\>192.168.1.239\</IP\>\<Pin\>\<On\>false\</On\>\<Val\>AAAA\</Val\>\</Pin\>\<FP\>4\</FP\>\<PP\>szkolna.GBT\</PP\>\<Sch\>false\</Sch\>\<Press\>\<SP\>\<N\>szkolna.GBT\</N\>\<StaD\>0\</StaD\>\<StaM\>0\</StaM\>\<StaY\>0\</StaY\>\<StoD\>0\</StoD\>\<StoM\>0\</StoM\>\<StoY\>0\</StoY\>\</SP\>\<SP\>\<N\>Projekt.GBT\</N\>\<StaD\>2\</StaD\>\<StaM\>1\</StaM\>\<StaY\>2018\</StaY\>\<StoD\>30\</StoD\>\<StoM\>12\</StoM\>\<StoY\>2018\</StoY\>\</SP\>\<SP\>\<N\>gradient.GBT\</N\>\<StaD\>7\</StaD\>\<StaM\>12\</StaM\>\<StaY\>2018\</StaY\>\<StoD\>7\</StoD\>\<StoM\>12\</StoM\>\<StoY\>2018\</StoY\>\</SP\>\<SP\>\<N\>Kopia Projekt1.GBT\</N\>\<StaD\>14\</StaD\>\<StaM\>11\</StaM\>\<StaY\>2019\</StaY\>\<StoD\>14\</StoD\>\<StoM\>11\</StoM\>\<StoY\>2019\</StoY\>\</SP\>\</Press\>\<Slvs\>\<Sl\>\<U\>3670045\</U\>\<SX\>0\</SX\>\<SY\>0\</SY\>\<EX\>128\</EX\>\<EY\>32\</EY\>\</Sl\>\</Slvs\>\</AnnVer2\>

Gzie poszczególne znaczniki oznaczają:
- \<AnnVer2\> - Pakiet danych zgłoszonych wraz informacją ich wersji.
- \<N\> - Nazwa tablicy
- \<V\> - Wersja oprogramowania
- \<D\> - Użyty sterownik grafiki paneli diodowych ( rodzaj paneli diodowych)
- \<S\> - Size, rozdzielczość tablicy
- \<U\> - **Uid numer seryjny sterownika**
- \<SIP\> - Bieżący adres IP urządzenia otrzymany
- \<K\> - Kontrast
- \<KN\> - Kontrast Nocny
- \<CW\> - Czasowe wyłączanie (Bypass)
- \<HOff\> - Godzina wyłączenia
- \<MOff\> - Minuta wyłączenia
- \<HOn\> - Godzina włączenia
- \<MOn\> - Minuta włączenia
- \<Sy\> - Synchronizacja czasu włączona/wyłączona
- \<StIP\> - Dane w przypadku statycznego adresu ip
- \<NM\> - Net mask , maska sieciowa
- \<GW\> - GateWay, brama domyślna
- \<IP\> - Adres IP
- \<PIN\> - Dane dotyczące pin przy dostępie z pilota
- \<Val\> - Wartość pin

Na ten moment pozostałe parametry są nieistotne w kontekście realizacji projektu.

Nasłuchiwania można dokonywać za pomocą soketów w dowolnym języku programowania bądź tak jak poprzednio za pomocą programu netcat, istotne jest aby żadna zapora sieciowa nie blokowała pakietów przychodzących na port 6001, należy tymczasowo wyłączyć firewall, lub lepiej wprowadzić odpowiednią regułę do zapory sieciowej.
> nc -ul 6001

# Kontakt

Arkadiusz Gil  
e-mail: voland83@gmail.com  
tel: 790597322  
