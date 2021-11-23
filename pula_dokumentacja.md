## Dokumentacja sterowania tablicą led firmy GilBT typ mono firmware kolejkowe

#Klient w tym wypadku program pula.exe ingeruje w to co ma wyświetlać ekran. Pula uruchamia dwa wątki z czego pierwszy odlicza dwie zmienne dodając do nich 1 co określoną ilość milisekund, drugi wątek komunikuje się z ekranem przesyłając mu te dane.

#Przykład uruchomienia programu pula:
	pula <adres ip>
	pula 192.168.1.206

#Poprzez socket możemy wysyłać następujące komendy

	"gettime" - zwraca czas wskazywany przez zegar w sterowniku

	"money" - wyświetla tekst w trakcie slajdu "aktualne imieniny" wybór ilości wyświetlanych linijek tekstu uzależniony jest od ilości argumentów podanych w komendzie money a więc np: money "pula1" "pula2" "pula3" spowoduje wyświetlenie 3 linijek, aby wyświetlić linijkę pustą podajemy np zamiast drugiego argumentu pusty cudzysłów np. money "pula1" "" "pula2", w ekranie o wysokości 32 pixeli, możliwe jest wyświetlenie maksymalnie dwóch linijek. Argumenty muszą być podane w cudzysłowach, dopuszczalne jest wypisywanie znaków spoza zakresu znaków drukowanych ASCII (np. wydruk znaku o kodzie 0xA8 za pomocą \xA8 (hex))

	"font" - służy do zmiany czcionki używanej przy wyświetlaniu tekstu podanego przez komendę money, jako argument podajemy nazwę pliku czcionek istniejącego w pamięci Karty SD ekranu np. font arialbold30.fnt lub nazwę jednej z dwóch wbudowanych w tablicę czcionek o wysokości 8pix, nazwy te to "FONTNORMAL" "FONTFAT"

	"txtpos" - służy do zmiany orientacji tekstu wprowadzonego przez komendę money, podajemy 4 argumenty dla każdej linii z osobna:
		"l" wyrównanie do lewej strony
		"r" j.w. do prawej
		"c" j.w. do środka
		"1" - "5" scrollowanie (przesuwanie tekstu) w prędkości od 1 do 5
		przykład użycia komendy:
			txtpos "l" "1" "3" "c"
			skutek:
				ustawienie 1 linii do lewej, 2 linia scroll ( prędkość 1 ), 3 linia scroll ( prędkość 3 ), 4 linia wyrównanie do środka


	"setrowspos" - zmienia pozycje wierszy (pionowo) na te podane przy wywołaniu komendy, normalnie są one uzależnione od wysokości czcionki.
		przykład użycia komendy:
			setrowspos "0" "8" "16" "24"
			skutek:
				ustawienie po kolei linii w pozycjach 0, 8, 16, 24. patrząc od góry.

	"version" - zwraca wersję firmware ekranu.

	"contrast" - zmienia jasność ekranu, podajemy argument od 1 do 10

	"getcontrast" - zwraca aktualną jasność ekranu

	"netconf" - zmienia parametry IP urządzenia np.
		netconf "192.168.1.2" "255.255.255.0" "192.168.1.1" //powoduje zmianę na static ip=192.168.1.2 maska= 255.255.255.0 brama=192.168.1.1
		netconf "dhcp" //powoduje włączenie przydzielania adresu poprzez system dhcp

#Alternatywnie zamiast komendy money możemy wysłać bitmapę do wyświetlenia, dokonujemy tego za pomocą standardowej komendy protokołu ftp "STOR" tyle, że podajemy jako parametr nazwę pliku docelowego "image.lim" 
		czyli: 
			STOR image.lim, program sterownika automatycznie taki plik zamiast zapisać na dysku wyświetli go na ekranie. 
			Przykład użycia dostępny jest w programie pula (metoda SendGraphic), trzeba go tylko odkomentować i podać ścieżkę do jakiejś grafiki jedno bitowej o rozmiarze 128x32.

#Warty zauważenia jest fakt, że komendy przyjmujące po więcej niż jeden argument przyjmują je w cudzysłowach.

#w razie pytań proszę się kontaktować 
			tel: 790597322 
			e-mail: arkadiusz.gil@gitbt.com

  ____ _ _ ____ _____ 
 / ___(_) | __ )_   _|
| |  _| | |  _ \ | |  
| |_| | | | |_) || |  
 \____|_|_|____/ |_|  
                      
