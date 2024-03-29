# Instrukcja wgrywania firmware sterowników rgb firmy GilBT

## Metoda przez kartę pamięci (bardziej niezawodna):
Do wgrania firmware do tablicy wymagana jest karta pamięci SD lub SDHC sformatowana w systemie FAT32 tylko i wyłącznie. Pomimo wielkich rozmiarów tablicy samo gniazdo jest małe i delikatne, w przypadku utrudnionego dostępu można użyć pęsety. Gniazdo karty jest typu push/push (pchnij/pchnij) czyli aby wprowadzić kartę popychamy ją, aby wyjąć wykonujemy tą samą czynność, karta powinna się sama wysunąć. Poniżej znajduje się obraz sterownika wraz z wskazaniem gdzie należy spodziewać się gniazda SD.
![rgb_controler.jpg](rgb_controler.jpg)

Procedura wgrania firmware jest następująca:  
- Proszę wyłączyć ekran.
- Jeśli slot nie jest pusty wyjąć kartę/karty pamięci i zapamiętać kolejność kart w modułach (jeśli jest ich wiele) oraz ich orientację w tablicy.
- Nagrać na karty plik firmware *.frm.
- **Koniecznie użyć funkcji bezpiecznego usuwania sprzętu przed wyjęciem karty pamięci z komputera.**
- Wsunąć kartę/karty pamięci do ekranu w zapamiętanej kolejności.
- Włączyć ekran.
- Odczekać kilkanaście sekund.

W przypadku braku sukcesu, można sprawdzić następujące rzeczy:  
- Upewnić się, że karta sformatowana jest na system plików FAT32 oraz, że użyta była funkcja bezpiecznego usuwania sprzętu.
- Psiknąć na styki karty preparatem konserwującym styki, na przykład Kontakt PR, alternatywnie WD 40.
- Można kilkukrotnie włożyć/wyjąć kartę tak aby sprowokować przetarcie styków gniazda.
- Sprawdzić sprawność karty sd, czy po skopiowaniu pliku z powrotem na dysk jest on identyczny z oryginałem.

## Metoda przez sieć Lan lub Wifi:

- Tablica musi być podłączona do wspólnej sieci z komputerem z zainstalowanym programem LPD GilBT
- W programie lpd przyciskamy lewy shift+ctrl i klikamy "Ekrany w sieci", powinno pojawić się "okno serwisowe".
- W oknie serwisowym widzimy, wszystkie ekrany i ich pod ekrany, aby mieć pewność, że obecne w sieci dane o ekranach są aktualne, klikamy "wyczyść listę" i czekamy aż urządzenia ponownie się pojawią na liście. 
- Możemy kliknąć przyciski "debuguj" dla każdego pod ekranu ( mastera i slave'ów ) tablicy, którą chcemy przeprogramować aby widzieć wysyłane przez nie komunikaty i mieć pewność, że wszystko przebiega zgodnie z procedurą.
- Klikamy wyślij firmware (tylko mastera) i wybieramy plik *.frm do wysłania. Pojawi się pasek postępu, który powinien dojść do końca i wysłać do wszystkich pod modułów zadany plik. Obserwując tablicę powinniśmy widzieć, że tablica zrobiła się czarna (wyświetla napis "transfer" albo zielone kropki/kreski) co oznacza, że wgrywanie pliku idzie pomyślnie, jeżeli tylko część tablicy wygasła to znaczy, że nie wszystkie moduły przyjmują plik ( może to być spowodowane uszkodzeniem listy slaveów lub karty pamięci w konkretnych modułach) i pasek postępu nie dojdzie do końca, można wysłać plik do każdego slavea z osobna lub wgrać do niedziałających modułów firmware przez kartę pamięci, opis znajduje się dalej. 
- Urządzenie powinno się samo zresetować i wgrać program do własnej pamięci.
W razie czego przy nie udanej próbie, można wyłączyć i włączyć ekran ponownie i całą operację powtórzyć.
Proszę nie eksperymentować za wiele z "Oknem serwisowym" może to spowodować niepoprawne działanie urządzenia.

## Kontakt

Arkadiusz Gil  
e-mail: voland83@gmail.com  
tel: 790597322  
