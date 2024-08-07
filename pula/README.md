﻿## Dokumentacja sterowania tablicą led firmy GilBT typ mono

Tablica posiada przyłącze do sieci lan z otwartym portem 21 ftp służącym do wymiany plików na dysku SD znajdującym się w tablicy. Sam protokół ftp został dodatkowo wzbogacony o kilka komend umożliwiających sterowanie treścią wyświetlaną na tablicy.  Przykładowy program klient (zawarty w tym repozytorium) program pula.exe ingeruje w to co ma wyświetlać ekran. Program pula uruchamia dwa wątki z czego pierwszy odlicza dwie zmienne dodając do nich 1 co określoną ilość milisekund, drugi wątek komunikuje się z ekranem przesyłając mu te dane.

# Przykład uruchomienia programu pula:

	`pula <adres ip tablicy led>`
	`pula 192.168.1.206`


# Przykład uruchomienia komunikacji przy pomocy programu netcat
Podobne sterowanie można uzyskać w każdym języku programowania posiadający możliwość wykorzystania socketów. Również przy pomocy polecenia nc (programu netcat) w terminalu linux.

Przykład wykorzystania programu "nc" dla komendy "gettime":

`printf "gettime" | nc -w 2 -N 192.168.1.206 21`

	

# Poprzez socket możemy wysyłać następujące komendy:

1. **"money"** - Inicjuje zmianę wyświetlanej treści. Wybór ilości wyświetlanych linijek tekstu uzależniony jest od ilości argumentów podanych w komendzie *money* i maksymalnie wynosi 4. A więc np: *money "pula1" "pula2" "pula3"* spowoduje wyświetlenie 3 linijek, współrzędnie linii w osi pionowej poszczególnych linii domyślnie wynoszą 0, 8, 16, 24 (licząc od góry w pikselach), aby wyświetlić linijkę pustą podajemy np zamiast drugiego argumentu pusty cudzysłów np. *money "pula1" "" "pula2"*. Argumenty muszą być podane w cudzysłowach, dopuszczalne jest wypisywanie znaków spoza zakresu znaków drukowanych ASCII (iso-8859-2) (np. wydruk znaku o kodzie 0xA8 za pomocą \xA8 (hex))

1. **"font"** - służy do zmiany czcionki używanej przy wyświetlaniu tekstu podanego przez komendę money, jako argument podajemy nazwę pliku czcionek istniejącego w pamięci Karty SD ekranu np. font arialbold30.fnt lub nazwę jednej z dwóch wbudowanych w tablicę czcionek o wysokości 8pix, nazwy te to "FONTNORMAL" "FONTFAT"

1. **"txtpos"** - służy do zmiany poziomej orientacji tekstu wprowadzonego przez komendę *money*, podajemy 4 argumenty dla każdej linii z osobna: 

   - "l" wyrównanie do lewej strony
   - "r" j.w. do prawej
   - "c" j.w. do środka
   - "1" - "5" scrollowanie (przesuwanie tekstu) w prędkości od 1 do 5

	Przykład:  
	`txtpos "l" "1" "3" "c"`  

	Powoduje ustawienie 1 linii do lewej, 2 linia scroll ( prędkość 1 ), 3 linia scroll ( prędkość 3 ), 4 linia wyrównanie do środka


1. **"setrowspos"** - zmienia pozycje wierszy (pionowo) na te podane przy wywołaniu komendy.

	Przykład:  
	`setrowspos "0" "8" "16" "24"`  

	Powoduje ustawienie po kolei linii w pozycjach 0, 8, 16, 24. patrząc od góry.

1. **"version"** - zwraca wersję firmware ekranu.

1. **"contrast"** - zmienia jasność ekranu, podajemy argument od 1 do 10

1. **"getcontrast"** - zwraca aktualną jasność ekranu

1. **"netconf"** - zmienia parametry IP urządzenia np.

	Przykład:  
	`netconf "192.168.1.2" "255.255.255.0" "192.168.1.1"`  

	Powoduje zmianę adresów na statyczne: ip=192.168.1.2 maska= 255.255.255.0 brama=192.168.1.1  

	Przykład:  
	`netconf "dhcp"`

	Powoduje włączenie przydzielania adresu poprzez system dhcp

1. **"gettime"** - zwraca czas wskazywany przez zegar w sterowniku. *Tablica pobiera aktualną godzinę z dostępnych w internecie serwerów NTP.*

Alternatywnie zamiast komendy *money* możemy wysłać bitmapę w specjalnym formacie "lim" do wyświetlenia, dokonujemy tego za pomocą standardowej komendy protokołu ftp "STOR", jako parametr podajemy nazwę pliku docelowego "image.lim".  

Przykład: 

`STOR image.lim`  

# Wysyłanie bitmapy jako treści obrazu:

Program sterownika automatycznie taki plik zamiast zapisać na dysku wyświetli go na ekranie. Przykład użycia dostępny jest w programie pula (metoda SendGraphic), należy go jedynie odkomentować i podać ścieżkę do grafiki jedno bitowej o rozmiarze 128x32.

**Uwaga: Komendy przyjmujące po więcej niż jeden argument przyjmują je w cudzysłowach.**

# Kontakt:

Arkadiusz Gil  
e-mail: voland83@gmail.com  
tel.: 790597322  
