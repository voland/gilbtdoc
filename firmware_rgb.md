# Instrukcja wgrywania firmware sterowników rgb firmy GilBT

## Metoda przez sieć Lan lub Wifi:

- Tablica musi być podłączona do wspólnej sieci z komputerem z zainstalowanym programem LPD GilBT
- W programie lpd przyciskamy lewy shift+ctrl i klikamy "Ekrany w sieci", powinno pojawić się "okno serwisowe".
- W oknie serwisowym widzimy, wszystkie ekrany i ich pod ekrany, aby mieć pewność, że są obecne w sieci dane są aktualne, klikamy "wyczyść listę" i czekamy aż ponownie się pojawią na liście. 
- Możemy kliknąć przyciski "debuguj" dla każdego pod ekranu ( mastera i slave'ów ) tablicy, którą chcemy przeprogramować aby widzieć wysyłane przez nie komunikaty i mieć pewność, że wszystko przebiega zgodnie z procedurą.
- Klikamy wyślij firmware (tylko mastera) i wybieramy plik *.frm do wysłania. Pojawi się pasek postępu, który powinien dojść do końca i wysłać do wszystkich pod modułów zadany plik. Obserwując tablicę powinniśmy widzieć, że tablica zrobiła się czarna (wyświetla napis "transfer" albo zielone kropki/kreski) co oznacza, że wgrywanie pliku idzie pomyślnie, jeżeli tylko część tablicy wygasła to znaczy, że nie wszystkie moduły przyjmują plik ( może to być spowodowane uszkodzeniem listy slaveów lub karty pamięci w konkretnych modułach) i pasek postępu nie dojdzie do końca, można wysłać plik do każdego slavea z osobna lub wgrać do niedziałających modułów firmware przez kartę pamięci, opis znajduje się dalej. 
- Urządzenie powinno się same zresetować i wgrać program do własnej pamięci.
W razie czego przy nie udanej próbie, można wyłączyć i włączyć ekran ponownie i całą operację powtórzyć.
Proszę nie eksperymentować za wiele z "Oknem serwisowym" może to spowodować niepoprawne działanie urządzenia.

## Metoda przez kartę pamięci (bardziej niezawodna):
Jeżeli ekran posiada uszkodzoną kartę pamięci lub niesprawną sieć albo wystąpią inne przeszkody wówczas pierwsza metoda nie zadziała może się okazać że trzeba będzie wgrać firmware na nową kartę albo ją sformatować.

- Proszę wyłączyć ekran.
- Wyjąć kartę/karty pamięci ( zapamiętać kolejność) .
- Nagrać na nie plik firmware *.frm.
- Koniecznie użyć funkcji bezpiecznego usuwania sprzętu w celu wyjęcia karty pamięci z komputera.
- Wsunąć kartę/karty pamięci do ekranu w zapamiętanej kolejności.
- Włączyć ekran.
- Odczekać kilkanaście sekund.
