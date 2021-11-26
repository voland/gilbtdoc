#printf "usage"
#printf "./upload_page.sh <ip> <port> <filename>"
#printf "port is UID_of_screen+2"

ADDR_IP=$1
PORT_CMD=$(($2+0))
PORT_DATA=$(($2+1))

# Połączenia zostają zamknięte po zdefiniowanym czasie braku aktywności
CONN_TIMEOUT=4

while true
do
	# Wysłanie komendy "page"
	COMAND_RESPONSE=$(printf "page\n" | nc -w $CONN_TIMEOUT $ADDR_IP $PORT_CMD)
	echo "$COMAND_RESPONSE"
	sleep 0.20
	# sprawdzenie czy Tablica otworzyła port danych
	if [[ $COMAND_RESPONSE == *"Expecting"* ]]; then
		echo "Success: Ready to receive data"
		# podmiana napisu "date" na aktualną godzinę przy pomocy polecenia "sed"
		sed "s/date/$(date +%H:%M)/" $3 | nc -w $CONN_TIMEOUT $ADDR_IP $PORT_DATA
		sleep 3
	fi
	# Jeśli port danych zajęty wyświetl error
	if [[ $COMAND_RESPONSE == *"busy"* ]]; then
		echo "Error: Protocol not ready, check if data port opened and trigger closing potential hanged connection"
		nc -zv $ADDR_IP $PORT_DATA
		sleep 10
	fi
	sleep 1
done
