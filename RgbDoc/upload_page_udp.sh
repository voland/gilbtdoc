#printf "usage"
#printf "./upload_page.sh <ip>  <filename>"
while true
do

sed "s/<time>/$(date +%H:%M)/" $2 | sed "s/<date>/$(date +%d.%m.%y)/" | sed "1s/^/JSONPAGE: /" | nc -w 2 -u $1 8888

done
