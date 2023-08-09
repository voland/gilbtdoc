# Introduction
This documentation is currently being continuously improved. If you find any parts that are unclear or incomplete, please feel free to contact us. Contact information can be found at the bottom of this document.

# Controlling RGB GilBT Display via Serial Connection (RS485 or RS232)

Commands are sent through a serial connection configured with a BAUD rate of 115200. Each command must be terminated with a newline character `<CR>`.

### Description of AT Commands Sent to the Device:

- `AT+RDEV=<device_name>`: Sets the receiving device for commands (ReceivingDevice). Since multiple displays can be connected to a single serial bus, you can select which display receives commands using this command. To have all displays receive and execute commands, use the `*` character as the device name.  
   Example:
   - `AT+RDEV=*`
   - `AT+RDEV=screen1`

- `AT+NAME=<device_name>`: Sets the device name.  
   Example:
   - `AT+NAME=screen1`

- `AT+STATUS`: Returns the message "OK".  
   Example:
   - `AT+STATUS`

- `AT+RST`: Resets the device. A reset is required after changing certain parameters described below.  
   Example:
   - `AT+RST`

- `AT+CONTR=`: Sets the display's brightness if not adjusted automatically by a light sensor. Parameter range: 1-4.  
   Example:
   - `AT+CONTR=4`

- `AT+CONTRN=`: Sets the display's brightness at night if not regulated by a light sensor. Parameter range: 1-4. Night contrast changes based on the internal clock.  
   Example:
   - `AT+CONTRN=1`

- `AT+RPOW=`: Sets the device's maximum power limit, with the parameter given in percentages. Parameter range: 1-100.  
   Example:
   - `AT+RPOW=80`

#### The following commands relate to network configuration. After executing any of the described commands, a device reset is necessary as network configuration occurs during restart.

- `AT+NMOD=`: Sets the mode for configuring the device's IP address. Values: static, dhcp.  
   Example:
   - `AT+NMOD=static`
   - `AT+NMOD=dhcp`

- `AT+IP=`: Sets the device's IP address. For the parameter to take effect, the network mode must be set to static.  
   Example:
   - `AT+IP=192.168.1.12`

- `AT+MA=`: Sets the device's subnet mask. For the parameter to take effect, the network mode must be set to static.  
   Example:
   - `AT+MA=255.255.255.0`

- `AT+GW=`: Sets the device's gateway address. For the parameter to take effect, the network mode must be set to static.  
   Example:
   - `AT+GW=192.168.1.1`

- `AT+ETHMOD=`: Sets the Ethernet socket's mode and bandwidth. Available options: AN, 100MFD, 100MHD, 10MFD, 10MHD. Default is AN (Auto Negotiation). Useful for long LAN cables to improve transmission quality.  
   Example:
   - `AT+ETHMOD=AN`
   - `AT+ETHMOD=100MFD`
   - `AT+ETHMOD=100MHD`
   - `AT+ETHMOD=10MFD`
   - `AT+ETHMOD=10MHD`

#### The following commands are for setting displayed content. To send content, initiate data transmission with a start command, send data in JSON format as described below in the "Description of JSON Page Format in Version 1" section, then conclude transmission with an end command.

- `AT+PAGE`: Notifies the device of the start of data transmission for display.
   Example:
   - `AT+PAGE`

- `AT+EOD`: Notifies the device of the end of data transmission for display (end of data). Upon receiving this command, the device should decode and display the data.
   Example:
   - `AT+EOD`

# Controlling RGB GilBT Display via Network Connection

## Retrieving LED Display Configuration Information
To retrieve configuration data from the controller, listen on UDP/IP port 6001. Each display with an RGB controller broadcasts its own configuration information. This information is in a simplified XML format. An example of such a data packet is provided below.

> `<AnnVer2><N>Home</N><V>master_2020-10-06_35959e2</V><D>p7s8RGB200Mhz8bit</D><S>128x32</S><U>3670045</U><SIP>192.168.1.239</SIP><K>4</K><KN>1</KN><PS>4</PS><CW><HOff>0</HOff><MOff>0</MOff><HOn>0</HOn><MOn>0</MOn></CW><Sy>false</Sy><OP>true</OP><MPV>6</MPV><StIP>true</StIP><NM>255.255.255.0</NM><GW>192.168.1.1</GW><IP>192.168.1.239</IP><Pin><On>false</On><Val>AAAA</Val></Pin><FP>4</FP><PP>school.GBT</PP><Sch>false</Sch><Press><SP><N>school.GBT</N><StaD>0</StaD><StaM>0</StaM><StaY>0</StaY><StoD>0</StoD><StoM>0</StoM><StoY>0</StoY></SP><SP><N>Project.GBT</N><StaD>2</StaD><StaM>1</StaM><StaY>2018</StaY><StoD>30</StoD><StoM>12</StoM><StoY>2018</StoY></SP>...</Press><Slvs><Sl><U>3670045</U><SX>0</SX><SY>0</SY><EX>128</EX><EY>32</EY></Sl></Slvs></AnnVer2>`

Where individual tags represent:
- `<AnnVer2>` - Data packet submitted with version information.
- `<N>` - Display name
- `<V>` - Software version
- `<D>` - Used LED panel graphics driver (type of LED panels)
- `<S>` - Size, display resolution
- `<U>` - **UID serial number of the controller**
- `<SIP>` - Current received IP address
- `<K>` - Contrast
- `<KN>` - Night Contrast
- `<CW>` - Conditional off (Bypass)
- `<HOff>` - Turn-off hour
- `<MOff>` - Turn-off minute
- `<HOn>` - Turn-on hour
- `<MOn>` - Turn-on minute
- `<Sy>` - Time synchronization enabled/disabled
- `<StIP>` - Data in case of static IP address
- `<NM>` - Net mask, subnet mask
- `<GW>` - GateWay, default gateway
- `<IP>` - IP address
- `<PIN>` - Data about the PIN when accessed with a remote control
- `<Val>` - PIN value

Listening can be done using sockets in any programming language, or by using the "nc" (netcat) command in Linux and Windows terminal to send commands without creating additional software.

Example of retrieving display configurations from the network environment using the netcat program:
> nc -kul 6001

## Display Log

In certain situations, reading the log of a controlled display can be helpful. The display sends specific information to the broadcast address on a port number dependent on its UID serial number. The port number consists of the last 4 digits of the UID. (UID number is a fixed serial number for each display.)

Example of reading the log for a display with UID=5308452
> nc -kul 8452

## Controlling RGB GilBT Display via UDP/IP Connection

Control is achieved by sending commands to the display using LAN and UDP/IP protocol. In any programming language, we can create such a connection using sockets. The "nc" (netcat) command can also be used in the Linux and Windows terminal to send commands without creating any additional software.

### Description of Commands Sent to the Device:

- Control of display content using UDP/IP packets (data up to 1.5kb)  
	JSONPAGE: <_content_>  
	Example 1:  
	`JSONPAGE:{"ver":1,"elements":[{"color":32,"width":96,"height":16,"type":"rectangle","x":0,"y":0},{"content":"2019-10-18 14:18:46","color":-65536,"fontsize":8,"fonttype":1,"type":"line","x":0,"y":0},{"content":"Text!","color":65280,"fontsize":16,"fonttype":2,"type":"line","x":0,"y":0}]}`

- Changing contrast/night contrast on a scale of 1-4  
	`Kontrast4!`  
	`KontrastN1`  

- Setting the internal clock:  
	`TIME:2019-10-18 15:37:00!`

- Turning DHCP on/off (Net Mode):  
	`NM0`  
	`NM1`

- IP Address:  
	`IP192.168.1.205`

- Subnet Mask:  
	`MA255.255.255.0`

- Default Gateway:  
	`GW192.168.1.1`

- Reset, after changing network settings, the display needs to be reset using the command:  
	`RESET`

- Changing the screen name:  
	`NAZWA:screen`  

- Setting automatic turn-off/turn-on time (if all zeros, the function is inactive):  
	`CW1:0-6:0!`

Controlling with the netcat command can look like the following:  
`nc -u _addressip_ _port_ < _file_with_command_`  

or  

`printf "*command*" | nc -u _addressip_ _port_`

Example 2 (in this case, the file data.txt contains the content from Example 1. The control UDP port is fixed and always set to 8888.)  

`nc -u 192.168.1.147 8888 < data.txt`

## Control of GilBT RGB Display via TCP/IP Protocol (data above 1.5kb)

Since the heart of the LED display is a microcontroller with limited resources, the display does not accept UDP packets larger than 1.5kb for JSON page data. If JSON page data exceeds this size, it should be sent via a TCP/IP connection.

All commands that can be sent via UDP/IP can also be sent via TCP/IP. To do so, you need to know the open TCP port number of the display. The port number is determined by the formula ((UID) modulo 10000) + 2. For example, for a display with a known UID of 5308452, the port number would be 8452 + 2 = 8454. Similar to sending via UDP, you can use socket libraries of any programming language or use the `nc` command in the terminal.

Example of sending the "RESET" command to the display via TCP/IP using a bash shell:
`printf "RESET" | nc -w 2 -N 192.168.1.12 8454`

#### Sending Any File to the Display's Memory Card
To send a file to the LED display, similar to FTP communication, you need to open two connections: one for sending commands and another for sending data. After opening the command port *((UID) modulo 10000) + 2* and issuing the `send` command, the display will open a port with the number *((UID) modulo 10000) + 3*, where you can send the data. Afterward, both connections should be closed.

If a file sent using this method has the name "rgb_cm4.frm," it will be treated as a new firmware for the display. The display will reset after receiving the firmware to update the software.

Example shell script "flash.sh" to send firmware files:
>`PORT1=$(($2+0))`  
>`PORT2=$(($2+1))`  
>`printf "send rgb_cm4.frm\n" | nc -w 5 -N $1 $PORT1 &`  
>`sleep `  
>`printf "Sending firmware file`  
>`nc -w 5 -N $1 $PORT2 < $`  

Calling the script can look like this:  
>`./flash.sh 192.168.1.12 8454 rgb_cm4.frm`

#### Sending Data for Display via TCP/IP
To send JSON data exceeding 1.5kb, a method similar to sending files can be used, but with the `page` command instead of `send`.

Example of the page.json file:
>`{"ver":1,"elements":[{"color":127,"width":120,"height":220,"type":"rectangle","x":0,"y":0},`  
>`{"content":"line1","color":-65536,"fontsize":8,"fonttype":1,"type":"line","x":40,"y":8},`  
>`{"content":"line2","color":-65536,"fontsize":8,"fonttype":1,"type":"line","x":40,"y":16},`  
>`{"content":"line3","color":-65536,"fontsize":8,"fonttype":1,"type":"line","x":40,"y":24},`  
>`{"content":"line4","color":-65536,"fontsize":8,"fonttype":1,"type":"line","x":40,"y":32},`  
>`{"content":"line6","color":-65536,"fontsize":8,"fonttype":1,"type":"line","x":40,"y":8},`  
>`{"content":"line7","color":-65536,"fontsize":8,"fonttype":1,"type":"line","x":40,"y":16},`  
>`{"content":"line8","color":-65536,"fontsize":8,"fonttype":1,"type":"line","x":40,"y":24},`  
>`{"content":"line9","color":-65536,"fontsize":8,"fonttype":1,"type":"line","x":40,"y":32},`  
>`{"content":"line10","color":-65536,"fontsize":8,"fonttype":1,"type":"line","x":0,"y":8},`  
>`{"content":"Line11","color":65280,"fontsize":16,"fonttype":2,"type":"line","x":0,"y":0}]}`

Example bash script to control the display:
>`PORT1=$(($2+0))`  
>`PORT2=$(($2+1))`  
>`printf "page\n" | nc -w 2 -N $1 $PORT1 & `  
>`printf "Sending page..."`  
>`nc -w 2 -N $1 $PORT2 < $3`  
>`sleep 4 `  

Calling the script can look like this:  
>`./upload_page.sh 192.168.1.12 8454 page.json`

After sending the `page` command to the command port of the display, it will return the message `Re: Expecting file json_page on port <data port number>` when ready to receive new data packets or `Re: DataPort is busy` if the previous connection is still ongoing. In the latter case, you should close the command port and retry after 2 seconds.

During transmission on the data port, the display will return information about received data, as shown below:  
`DataPort: Ready to receive json_page`  
`DataPort: Total bytes received 1460`  
`DataPort: Total bytes received 2920`  
`DataPort: Total bytes received 5840`  
`DataPort: Total bytes received 7551`

**Note:** Control of the display must be synchronous (single-threaded). The last line of the script instructs a 4-second delay before the next transmission. Since the LED display operates single-threadedly, it's not possible to handle multiple transmissions simultaneously. Before attempting a new connection, you need to wait for previous connections to finish and close. Unlike UDP control, this limitation exists.

## Description of JSON Page Format, Version 1
1. Example JSON page:  
   `{"ver":1,"elements":[{"color":32,"width":96,"height":16,"type":"rectangle","x":0,"y":0},{"content":"2019-10-18 14:18:46","color":-65536,"fontsize":8,"fonttype":1,"type":"line","x":0,"y":0},{"content":"Text!","color":65280,"fontsize":16,"fonttype":2,"type":"line","x":0,"y":0}]}`

2. Elements  
there are only two types of elements available in the page:
   * line - text line
   * rectangle - a rectangle with a chosen size and color.

3. Font
As seen in the JSON script, font type is identified by a number. Currently, you can use values from 0 to 3. The first two fonts are compiled into the firmware and always have a fixed height of 8px. Other fonts are stored as files on the memory card. The second type of fonts was automatically generated by the program and may be unreadable at sizes below 10px.
   * 0 - regular font, compiled into the firmware, always has a height of 8px regardless of the *fontsize* parameter
   * 1 - **bold** font, compiled into the firmware, always has a height of 8px regardless of the *fontsize* parameter
   * 2 - Arial regular font, loaded from the memory card as the file arialXX.rgb.fnt, where XX represents the size in pixels
   * 3 - Arial bold font, loaded from the memory card as the file arialBXX.rgb.fnt, where XX represents the size in pixels

   **Note:** If the selected font is missing on the SD card, it's replaced with font 0.

4. Color  
   The color is specified in a 32-bit ARGB integer variable.
   * For example, 0x00ff0000 converted to an int variable represents the color red.
   * For example, 0x0000ff00 converted to an int variable represents the color green.
   * For example, 0x000000ff converted to an int variable represents the color blue.

If you need access to other font types, please contact the text author. The contact information is provided in the footer. It's also possible to compile the most commonly used font into the device firmware for optimization and to omit the SD memory card. In such a case, please contact the author as well.

# Contact

Arkadiusz Gil
Email: voland83@gmail.com
Phone: 790597322
