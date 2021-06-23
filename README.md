# Sterwowanuie tablicą GilBT rgb poprzez protokuł udp/ip 

Sterowanie dokonujemy wysyłając komendy do tablicy, używamy do tego połączenia lan oraz protokołu udp/ip. W dowolnym języku programowania możemy utworzyć takie połącznie używając tak zwanych socketów. Możemy też użyć komendy "nc" (programu netcat) w terminalu linux oraz windows do wysyłania komend bez tworzenia żadnego dodatkowego oprogramowania.

- Sterowanie treścią tablicy  
	JSONPAGE: <_content_>  
	przykład1:  
	>JSONPAGE:{"ver":1,"elements":[{"color":32,"width":96,"height":16,"type":"rectangle","x":0,"y":0},{"content":"2019-10-18 14:18:46","color":-65536,"fontsize":8,"fonttype":1,"type":"line","x":0,"y":0},{"content":"Tekst!","color":65280,"fontsize":16,"fonttype":2,"type":"line","x":0,"y":0}]}

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

# Kontakt

Arkadiusz Gil  
e-mail: volan83@gmail.com  
tel: 790597322  
