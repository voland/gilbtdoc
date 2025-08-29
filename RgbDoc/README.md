# Wstęp
Niniejsza dokumentacja jest na etapie ciągłego udoskonalania. Jeżeli cokolwiek wydaje się być niezrozumiałe lub niekompletne proszę o kontakt. Dane kontaktowe umieszczone są na końcu dokumentu.

# Sterowanie tablicą GilBT typ RGB poprzez połączenie szeregowe rs485 lub rs232 

Komendy wysyłamy przez połączenie szeregowe ustawione jest na BAUD=115200, każda komenda oprócz AT+PAGE musi być zakończona znakiem nowej linii **\n**.

### Opis Komend AT wysyłanych do urządzenia:

- "AT+RDEV=<nazwa_urządzenia>" Ustawienie urządzenia do odbierania komend (RecevingDevice). Ponieważ pod jedną magistralę szeregową może być podłączone kilka tablic, to która tablica ma odbierać komendy wybieramy za pomocą opisywanego polecenia, jeżeli chcemy aby wszystkie tablice odbierały i wykonywały polecenia jako nazwę podajemy znak \*.  
    przykład:  
	`AT+RDEV=*`  
	`AT+RDEV=screen1`  

- "AT+NAME=<nazwa_urządzenia>" Ustawia nazwę urządzenia.  
    przykład:  
	`AT+NAME=screen1`  

- "AT+STATUS" zwraca komunikat "OK".  
    przykład:  
	`AT+STATUS`  

- "AT+RST" Resetuje urządzenie, reset jest wymagany po ustawieniu niektórych parametrów opisanych poniżej.  
    przykład:  
	`AT+RST`  
    
- "AT+CONTR=" Ustawia jasność świecenia tablicy o ile ta nie jest regulowana automatycznie poprzez fotorezystor. Zakres parametru: 1-4.  
    przykład:  
	`AT+CONTR=4`  
    
- "AT+CONTRN=" Ustawia jasność świecenia w tablicy w nocy o ile ta nie jest regulowana automatycznie poprzez fotorezystor. Zakres parametru: 1-4. Kontrast nocny przełączany jest na podstawie wskazań zegara wewnętrznego.  
    przykład:  
	`AT+CONTRN=1`  
    
- "AT+RPOW=" Ustawia ograniczenie maksymalnej mocy urządzenia, parametr podawany jest w procentach. Zakres parametru: 1-100.  
    przykład:  
	`AT+RPOW=80`  

#### Poniżej opisane komendy dotyczą konfiguracji sieci, po wykonaniu dowolnej ilości z poniżej opisanych komend konieczne jest zresetowanie urządzenia gdyż sieć konfigurowania jest podczas restartu.

- "AT+NMOD=" Ustawia tryb ustawienia adresu ip urządzenia. Przyjmuje wartości: static,dhcp.  
    przykład:  
	`AT+NMOD=static`  
	`AT+NMOD=dhcp`  

- "AT+IP=" Ustawia adres ip urządzenia, aby ustawienie parametru miało skutek, koniecznie jest ustawienie trybu sieci na static.  
    przykład:  
	`AT+IP=192.168.1.12`  

- "AT+MA=" Ustawia maskę podsieci urządzenia, aby ustawienie parametru miało skutek, koniecznie jest ustawienie trybu sieci na static.  
    przykład:  
	`AT+MA=255.255.255.0`  

- "AT+GW=" Ustawia adres ip urządzenia, aby ustawienie parametru miało skutek koniecznie jest ustawienie trybu sieci na static.  
    przykład:  
	`AT+GW=192.168.1.1`  

- "AT+ETHMOD=" Ustawia tryb oraz przepustowość gniazda ethernet, dostępne możliwości to: AN, 100MFD, 100MHD, 10MFD, 10MHD. Domyślna wartość to AN (Auto Negotiation). Przydatne w sytuacji gdy kabel Lan jest bardzo długi i spowolnienie przepustowości może poprawić jakość transmisji.  
    przykład:  
	`AT+ETHMOD=AN`  
	`AT+ETHMOD=100MFD`  
	`AT+ETHMOD=100MHD`  
	`AT+ETHMOD=10MFD`  
	`AT+ETHMOD=10MHD`  

#### Poniżej opisane komendy dotyczą ustawiania wyświetlanej treści. Aby wysłać treść należy wysłać komendę rozpoczęcia transmisji, wysłać dane w postaci JSON opisanej poniżej w punkcie "Opis formatu strony json  w wersji 1", następnie wysłać komendę zakończenia transmisji.


- "AT+PAGE" Informuje urządzenie o rozpoczęciu nadawania danych do wyświetlania, ta komenda nie powinna być zakończona znakiem końca linii, cały łańcuch powinien wyglądać następująco: 'AT+PAGE<wysyłane dane>AT+EOD\n'.
    przykład:  
	`AT+PAGE`  

- "AT+EOD" Informuje urządzenie o zakończeniu nadawania danych do wyświetlania, (end of data). Po otrzymaniu komendy urządzenie powinno zdekodować dane oraz wyświetlić.
    przykład:  
	`AT+EOD`  

# Sterowanie tablicą GilBT typ RGB poprzez połączenie sieciowe

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
	`JSONPAGE:{"ver":1,"elements":[{"type":"rectangle","color":0x000020,"width":96,"height":16,"x":0,"y":0},{"type":"png", "filename":"image.png", "x":0,"y":0},{"type":"txt","content":"2019-10-18 14:18:46","color":0xff0000,"fonttype":1,"x":0,"y":0},{"type":"txt","content":"Tekst!","color":0x00ff00,"fonttype":2, "filename":"arialB16.fnt", "x":0,"y":0}]}`

- Zmiana kontrastu/kontrastu nocnego w skali od 1-4  
	`Kontrast4!`  
	`KontrastN1`  

- Ustawienie zegara wewnętrznego:  
	`TIME:2019-10-18 15:37:00!`  

- Dhcp Włącz/Włącz:  
	`NM0`  //Adres dynamiczny z dhcp.  
	`NM1`  //Adres statyczny ustawiany komend.

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

lub  

`echo "*command*" | nc -u _addressip_ _port_`  

przykład 2 ( w tym przypadku plik data.txt zawiera treść z przykładu 1. Port sterowania udp jest stały i zawsze wynosi 8888.)  

`nc -u 192.168.1.147 8888 < data.txt`  

## Sterowanie tablicą GilBT rgb poprzez protokół tcp/ip ( dane powyżej 1,5kb )

Ponieważ sercem tablicy led jest mikrokontroler a te posiadają małe zasoby, tablica nie przyjmuje pakietów udp wielkości powyżej 1,5kb jeżeli dane json page przekraczają tą wartość należy je wysłać poprzez połączenie tcp/ip.  

Wszystkie komendy, które można wysłać drogą udp/ip można również wysłać drogą tcp/ip aby to zrobić należy znać numer otwartego portu tcp danej tablicy, numer portu wyznacza się wedle wzoru ((UID) modulo 10000)+2. Przykładowo dla tablicy o znanym numerze uid 5308452 port wynosi: 8452+2 = 8454.  Podobnie jak w przypadku wysyłania drogą udp można korzystać bibliotek socketów dowolnego języka programowania lub wysłać dane przy pomocy programu netcat (komendy nc w terminalu).  

Przykład wysłania komendy "RESET" do tablicy drogą tcp/ip przy pomocy terminala z powłoką bash:  
`printf "RESET" | nc -w 2 -N 192.168.1.12 8454`  

#### Wysyłanie dowolnego pliku na kartę pamięci tablicy.  
 W celu wysłania pliku na tablicę led, podobnie jak ma to miejsce w przypadku komunikacji ftp, należy otworzyć dwa połączenia, połączenie do wysłania komendy oraz do wysłania danych. Po otwarciu portu komend *((UID) modulo 10000)+2*, i wydaniu polecenia `send` tablica otworzy port o numerze *((UID) modulo 10000)+3* na, który można wysłać dane, po czym należy zamknąć oba połączenia. 

 Wysłanie pliku na kartę pamięci uSD znajdującą się w sterowniku tablicy:  

`printf "send image.png\n" | nc -u _addressip_ _port_`  

 Usuwanie pliku z karty pamięci uSD znajdującej się w sterowniku tablicy:  

`printf "del image.png\n" | nc -u _addressip_ _port_`  

Wysyłając powyższą metodą jeżeli plik będzie posiadał nazwę "rgb_cm4.frm" zostanie on potraktowany jako nowy firmware tablicy i tablica po odebraniu firmware zresetuje się celem zaktualizowania oprogramowania.

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

>`{"ver":1,"elements":[{"type":"rectangle","color":0x7f,"width":120,"height":220,"x":0,"y":0},`  
>`{"type":"png","filename":"image.png","x":40,"y":8},`  
>`{"type":"txt","content":"linia1","color":0xff0000,"fonttype":1,"x":40,"y":8},`  
>`{"type":"txt","content":"linia2","color":0xff0000,"fonttype":1,"x":40,"y":16},`  
>`{"type":"txt","content":"linia8","color":0xff0000,"fonttype":1,"x":40,"y":24},`  
>`{"type":"txt","content":"linia9","color":0xff0000,"fonttype":1,"x":40,"y":32},`  
>`{"type":"txt","content":"linia10","color":0xff0000,"fonttype":1,"x":0,"y":8},`  
>`{"type":"txt","content":"Linia11","color":0x00ff00,"filename":"arialB16.fnt","fonttype":2,"x":0,"y":0}]}`  


Przykładowy skrypt bash sterujący tablicą:

>`PORT1=$(($2+0))`  
>`PORT2=$(($2+1))`  
>`printf "page\n" | nc -w 2 -N $1 $PORT1 & `  
>`printf "Sending page..."`  
>`nc -w 2 -N $1 $PORT2 < $3`  
>`sleep 4 `  

Wywołanie skryptu może wyglądać następująco:  
>`./upload_page.sh 192.168.1.12 8454 page.json`  

Po wysłaniu komendy *page* na port komend tablicy, zwraca ona informację `Re: Expecting file json_page on port <numer portu danych>` jeśli jest gotowa do odebrania nowego pakietu danych, lub `Re: DataPort is busy` jeśli poprzednie połączenie jest wciąż w toku, należy wówczas zamknąć port komend i ponowić próbę po 2 sekundach.  

W trakcie transmisji na porcie danych tablica zwraca informacje o odebranych danych, przykład poniżej:  
`DataPort: Ready to receive json_page`  
`DataPort: Total bytes received 1460`  
`DataPort: Total bytes received 2920`  
`DataPort: Total bytes received 5840`  
`DataPort: Total bytes received 7551`  

 **Uwaga:** Sterowanie tablicą musi odbywać się synchronicznie (jednowątkowo). Ostatnia linijka powyższego skryptu nakazuje odczekanie 4 sekund przed ponowną transmisją. Ponieważ tablica led działa jednowątkowo nie możliwe jest obsługiwanie wielu transmisji jednocześnie, przed próbą ponownego połączenia konieczne jest odczekanie do zakończenia i zamknięcia poprzednich połączeń. Alternatywnie zamiast trzymać się sztywnego czasu opóźnienia, można analizować dane zwracane przez porty komend i danych celem oceny czy wszystkie dane zostały dostarczone i kolejna transmisja jest możliwa. Sterowanie Udp nie posiada tego ograniczenia.  

## Opis formatu strony json  w wersji 1
1. Strona to innymi słowy zbiór elementów, które mają zostać wyświetlone na planszy led. Zapisana jest ona w formacie json tak aby poszczególne parametry i ich nazwy były samo wyjaśniające. Parametry, które wymagają dodatkowego wyjaśnienia opisane są poniżej. Przykład kodu:  
````{
	"ver": 1,
	"elements": [
        {
            "type":"rectangle",
            "color":0x000020,
            "width":96,
            "height":16,
            "x":0,
            "y":0
        }
		{
			"type": "png",
			"filename": "image.png",
			"x": 0,
			"y": 0
		},
		{
			"type": "txt",
			"content": "Napis1",
			"color": 0x00ff00,
			"fonttype": 1,
			"x": 24,
			"y": 1
		},
		{
			"type": "txt",
			"content": "Napis2",
			"color": 0x00ff00,
			"fonttype": 2,
            "filename":"arialB16.fnt",
			"x": 24,
			"y": 56
		},
		{
			"type": "txt",
			"content": "napis czcionką impact 16",
			"color": 0x00ff00,
			"fonttype": 7,
			"x": 24,
			"y": 56
		}
	]
}```

2. Dostępne elementy  
Aktualnie dostępne są 3 rodzaje elementów strony, rodzaj definiuje się w polu "type", elementy są nakładane warstwowo w kolejności podanej w kodzie.
	* rectangle - prostokąt o wybranym rozmiarze i kolorze, element może być użyty do ustawienia koloru tła.
	* png - bitmapa w formacie png, która musi być dostępna na karcie pamięci sd urządzenia.
	* txt - linia tekstu

3. Czcionka elementu tekst  
Jak widać w skrypcie json rodzaj czcionki określa się numerem, należy wprowadzić wartości od 0 do 7. Czcionki 0,1,4,5,6,7 to czcionki wkompilowane w firmware mają zawsze stałą wysokość i są dostępne niezależnie od zewnętrznej karty pamięci sd, pozostałe czcionki znajdują się na karcie pamięci w postaci plików, aby ich użyć należy podać "fonttype":2' oraz parametr "filename" czyli nazwę pliku czcionki, każdy plik czcionki to czcionka o stałym rozmiarze na przykład "arialB16.fnt" to arial bold 16px. 
	* 0 czcionka regular wkompilowana w firmware, jej wysokość wynosi zawsze 8px (nie wymaga karty pamięci).
	* 1 czcionka **bold** wkompilowana w firmware, jej wysokość wynosi zawsze 8px (nie wymaga karty pamięci).
	* 2 czcionka pobrana jest z karty pamięci, należy podać nazwę pliku jako parametr *filename*.
	* 4 czcionka Arial14 wkompilowana w firmware, jej wysokość wynosi zawsze 14px (nie wymaga karty pamięci).
	* 5 czcionka Arial16 wkompilowana w firmware, jej wysokość wynosi zawsze 16px (nie wymaga karty pamięci).
	* 6 czcionka Impact14 wkompilowana w firmware, jej wysokość wynosi zawsze 14px (nie wymaga karty pamięci).
	* 7 czcionka Impact16 wkompilowana w firmware, jej wysokość wynosi zawsze 16px (nie wymaga karty pamięci).
	* 8 czcionka Arial20 wkompilowana w firmware, jej wysokość wynosi zawsze 20px (nie wymaga karty pamięci).

	**Uwaga:** Jeśli na karcie SD brakuje wybranej czcionki zawsze zastępowana jest ona czcionką 0. 

	**Uwaga:** Wymagana karta pamięci SD nie powinna przekraczać rozmiaru 32gb, musi być sformatowana na FAT32.  

	**Uwaga:** Uwaga, drugi rodzaj czcionek został wygenerowany automatycznie i w rozmiarach poniżej 10px czytelność może być zakłócona.  

4. Kolor elementu tekst lub prostokąt  
	**Uwaga:** kolor podany jest w zmiennej integer 32bit ARGB, może być podany w formie decymalnej lub hexadecymalnej.   
    * Przykładowo 0xff0000 przekonwertowany do zmiennej int oznacza kolor czerwony.  
    * Przykładowo 0x00ff00 przekonwertowany do zmiennej int oznacza kolor zielony.  
    * Przykładowo 0x0000ff przekonwertowany do zmiennej int oznacza kolor niebieski.  
  
5. Współrzędne elementu x, y. Reprezentują położenie elementu na tablicy gdzie punkt x=0, y=0 znajduje się w lewym górnym rogu tablicy. Do x możemy przypisać wartości specjalne, które mogą oznaczać automatyczne zorientowanie tekstu w linii poziomej.  
    * x = 60000, tekst ustawiony na środku tablicy w osi poziomej.  
    * x = 60001, tekst ustawiony z prawej strony tablicy w osi poziomej.  
    * x = 60002, tekst ustawiony z lewej strony tablicy w osi poziomej.  
  
6. Bitmapa w formacie png  
    Aby wyświetlić bitmapę, musi ona być on dostępna na karcie pamięci w postaci pliku w formacie png, rozdzielczość bitmapy nie powinna przekraczać rozdzielczości samej tablicy. W składni json element obrazu definiuje się za pomocą 4 składowych: 
    * "type":"png" - określa że element jest typem bitmapy 
    * "filename":"nazwapliku.png" - określa nazwę pliku do wyświetlenia
    * "x":0 "y":0 - określa współrzędnie miejsca lewej górnej krawędzi bitmapy.



Wszystkie aktualnie dostępne czciąki załączone są w tym repozytorium, w przypadku potrzeby dostępu do innych rodzajów czcionek proszę kontaktować się z działem technicznym firmy GilBT, adres znajduje się w stopce.

# Kontakt

Arkadiusz Gil  
e-mail: voland83@gmail.com  
tel: 790597322  
