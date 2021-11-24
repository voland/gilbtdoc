## Wstęp
Niniejsza dokumentacja  jest na etapie ciągłego udoskonalania. Jeżeli cokolwiek wydaje się być niejasne lub niekompletne proszę o kontakt. Dane kotaktowe umieszczone są na dole dokumentu.

## Pobieranie informacji o konfiguracji tablicy led.
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

Nasłuchiwania można dokonywać za pomocą soketów w dowolnym języku programowania bądź tak za pomocą programu netcat, istotne jest aby żadna zapora sieciowa nie blokowała pakietów przychodzących na port 6001, należy tymczasowo wyłączyć firewall, lub lepiej wprowadzić odpowiednią regułę do zapory sieciowej.  

Przykład pobierania konfiguracji tablic z otoczenia sieciowego za pomocą programu netcat.
> nc -kul 6001

## Log Tablicy

W pewnych sytuacjach pomocne może być odczytywanie loga sterowanej tablicy, która wysyła pewne informacje na adres rozgłoszeniowy na numer portu uzależniony od swojego numeru seryjnego UID. Numer portu stanowią 4 ostatnie numeru UID. ( Numer UID to stały numer seryjny sterownika osobny dla każdej tablicy).  

Przykład odczytania loga dla tablicy o UID=5308452
> nc -kul 8452

## Sterowanie tablicą GilBT typ RGB poprzez połączenie udp/ip 

Sterowanie dokonujemy wysyłając komendy do tablicy, używamy do tego połączenia lan oraz protokołu udp/ip. W dowolnym języku programowania możemy utworzyć takie połączenie używając tak zwanych socketów. Możemy też użyć komendy "nc" (programu netcat) w terminalu linux oraz windows do wysyłania komend bez tworzenia żadnego dodatkowego oprogramowania.

### Opis Komend wysyłanych do urządzenia:

- Sterowanie treścią tablicy pakietami udp/ip ( dane do 1,5kb )  
	JSONPAGE: <_content_>  
	przykład1:  
	`JSONPAGE:{"ver":1,"elements":[{"color":32,"width":96,"height":16,"type":"rectangle","x":0,"y":0},{"content":"2019-10-18 14:18:46","color":-65536,"fontsize":8,"fonttype":1,"type":"line","x":0,"y":0},{"content":"Tekst!","color":65280,"fontsize":16,"fonttype":2,"type":"line","x":0,"y":0}]}`

- Zmiana kontrastu/kontrastu nocnego w skali od 1-4  
	`Kontrast4!`  
	`KontrastN1`  

- Ustawienie zegara wewnętrznego:  
	`TIME:2019-10-18 15:37:00!`  

- Dhcp Wyłącz/Włącz   ( Net Mode )  
	`NM0`  
	`NM1`  

- Adres IP  
	`MIP192.168.1.205`  

- Maska  
	`MA255.255.255.0`  

- Brama Domyślna  
	`GW192.168.1.1`  

- Reset, po zmianie ustawień sieci trzeba zresetować tablicę komendą:  
	`RESET`  

- Zmiana nazwy ekranu
	`NAZWA:ekran`  

- ustawienie godziny wyłączenia/włączenia automatycznego ( gdy same zera to funkcja nieaktywna)  
	`CW1:0-6:0!`  

Sterowanie komendą netcat może wyglądać następująco:  
`nc -u _addressip_ _port_ < _file_with_command_`  

lub  

`printf "*command*" | nc -u _addressip_ _port_`  

przykład2 ( w tym przypadku plik data.txt zawiera treść z przykładu 1. Port sterowania udp jest stały i zawsze wynosi 8888.)  

`nc -u 192.168.1.147 8888 < data.txt`  

## Sterowanie tablicą GilBT rgb poprzez protokół tcp/ip ( dane powyżej 1,5kb )

Ponieważ sercem tablicy led jest mikrokontroler a te posiadają małe zasoby, tablica nie przyjmuje pakietów udp wielkości powyżej 1,5kb jeżeli dane json page przekraczają tą wartość należy je wysłać poprzez połączenie tcp/ip.  

Wszystkie komendy, które można wysłać drogą udp/ip można również wysłać drogą tcp/ip aby to zrobić należy znać numer otwartego portu tcp danej tablicy, numer portu wyznacza się wedle wzoru ((UID) modulo 10000)+2. Przykładowo dla tablicy o znanym numerze uid 5308452 port wynosi: 8452+2 = 8454.  Podobnie jak w przypadku wysyłania drogą udp można korzystać bibliotek socketów dowolnego języka programowania lub wysłać dane przy pomocy programu netcat (komendy nc w terminalu).  

Przykład wysłania komendy "RESET" do tablicy drogą tcp/ip przy pomocy terminala z powłoką bash:  
`printf "RESET" | nc -w 2 -N 192.168.1.12 8454`  

#### Wysyłanie dowolnego pliku na kartę pamięci tablicy.  
 W celu wysłania pliku na tablicę led, podobnie jak ma to miejsce w przypadku komunikacji ftp, należy otworzyć dwa połączenia, połączenie do wysłania komendy oraz do wysłania danych. Po otwarciu portu komend *((UID) modulo 10000)+2*, i wydaniu polecenia `send` tablica otworzy port o numerze *((UID) modulo 10000)+3* na, który można wysłać dane, po czym należy zamknąć oba połączenia. 

Jeżeli wysłany powyższą metodą plik będzie posiadał nazwę "rgb_cm4.frm" zostanie on potraktowany jako nowy firmware tablicy i tablica po odebraniu firmware zresetuje się celem zaktualizowania oprogramowania.

Przykładowy skrypt shell "flash.sh" do wysłania pliku firmware.

>`PORT1=$(($2+0)`  
>`PORT2=$(($2+1)`  
>`printf "send rgb_cm4.frm\n" | nc -w 5 -N $1 $PORT1 &`  
>`sleep `  
>`printf "Sending firmware file`  
>`nc -w 5 -N $1 $PORT2 < $`  

Wywołanie skryptu może wyglądać następująco:  
>`./flash.sh 192.168.1.12 8454 rgb_cm4.frm`  

#### Wysyłanie danych do wyświetlenia drogą tcp/ip
Aby wysłać dane json przekraczające 1,5kb należy skorzystać z metody podobnej jak w przypadku wysyłania pliku tyle, że stosujemy komendę *page* zamiast *send*. 

Przykład pliku page.json:

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


Przykładowy skrypt bash sterujący tablicą:

>`PORT1=$(($2+0))`  
>`PORT2=$(($2+1))`  
>`printf "page\n" | nc -w 2 -N $1 $PORT1 & `  
>`printf "Sending page..."`  
>`nc -w 2 -N $1 $PORT2 < $3`  
>`sleep 4 `  

Wywołanie skryptu może wyglądać następująco:  
>`./upload_page.sh 192.168.1.12 8454 page.json`  

Po wysłaniu komendy *page* na port komend tablicy, zwraca ona informację `Re: Expecting file json_page on port <numer portu danych>` jeśli jest gotowa odebrania nowego pakietu danych, lub `Re: DataPort is busy` jeśli poprzednie połączenie jest wciąż w toku, należy wówczas zamknąć port komend i ponowić próbę po 2 sekundach.  

W trakcie transmisji na porcie danych tablica zwraca informacje o odebranych danych, przykład poniżej:  
`DataPort: Ready to receive json_page`  
`DataPort: Total bytes received 1460`  
`DataPort: Total bytes received 2920`  
`DataPort: Total bytes received 5840`  
`DataPort: Total bytes received 7551`  

 **Uwaga:** Sterowanie tablicą musi odbywać się synchronicznie (jednowątkowo). Ostatnia linijka powyższego skryptu nakazuje odczekanie 4 sekund przed ponowną transmisją. Ponieważ tablica led działa jednowątkowo nie możliwe jest obsługiwanie wielu transmisji jednocześnie, przed próbą ponownego połączenia konieczne jest odczekanie do zakończenia i zamknięcia poprzednich połączeń. Alternatywnie zamiast trzymać się sztywnego czasu opóźnienia, można analizować dane zwracane przez porty komend i danych celem oceny czy wszystkie dane zostały dostarczone i kolejna transmisja jest możliwa.**   

## Opis formatu strony json  w wersji 1
1. Elementy  
W tym momencie dostępne są tylko 2 rodzaje elementów strony:
	* line - linia tekstu
	* rectangle - prostokąt o wybranym rozmiarze i kolorze.

1. Czcionka  
	Jak widać w skrypcie json rodzaj czcionki określa się numerem, aktualnie można podać wartości od 0 do 3. Z czego dwie pierwsze czcionki wkompilowane są w firmware mają zawsze stałą wysokość 8px, pozostałe czcionki znajdują się na karcie pamięci w postaci plików, ten drugi rodzaj czcionek był generowany automatycznie przez program i w rozmiarach poniżej 10px może być nieczytelny.
	* 0 czcionka regular wkompilowana w firmware jej wysokość wynosi zawsze 8px niezależnie od ustawienia parametru *fontsize*
	* 1 czcionka **bold** wkompilowana w firmware jej wysokość wynosi zawsze 8px niezależnie od ustawienia parametru *fontsize*
	* 2 czcionka arial regular, czcionka pobrana jest z karty pamięci z pliku arialXX.rgb.fnt, gdzie XX oznacza wielkość w px
	* 3 czcionka arial bold, czcionka pobrana jest z karty pamięci z pliku arialBXX.rgb.fnt, gdzie XX oznacza wielkość w px

	**Uwaga:** Jeśli na karcie SD brakuje wybranej czcionki zawsze zastępowana jest ona czcionką 0.

2. Kolor  
	**Uwaga:** kolor podany jest w zmiennej integer 32bit ARGB.   
    * Przykładowo 0x00ff0000 przekonwertowany do zmiennej int oznacza kolor czerwony  
    * Przykładowo 0x0000ff00 przekonwertowany do zmiennej int oznacza kolor zielony  
    * Przykładowo 0x000000ff przekonwertowany do zmiennej int oznacza kolor niebieski  


W przypadku potrzeby dostępu do innych rodzajów czcionek proszę kontaktować się z autorem tekstu, adres znajduje się w stopce.
Możliwe jest też wkompilowanie najczęściej używanej czcionki w firmware urządzenia celem optymalizacji i pominięcia karty pamięci sd, w takiej sytuacji również proszę o kontakt.

# Kontakt

Arkadiusz Gil  
e-mail: voland83@gmail.com  
tel: 790597322  
