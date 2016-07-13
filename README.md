# API
Path: /Api/Controllers

#### IotController
Receives string messages which is sent to devices through Azure iot hub. 
Calls /Models/IotHub.cs which handles the logic. 

#### ValuesController
Receives a JSON object from devices. The object is stored to Azure Lake Storage and DocumentDB
Calls /Models/SendToDocDB.cs & /Models/SendToLakeStorage.cs which handles the logic.

# Client
The system only supports one client atm.

#### Python
ssh pi@dex.local
cd azure-iot-sdks/python/device/samples/
Right now only master.py and AirQuality.py is used.

#### master.py
- Listens for commands from Azure iot hub.
- Imports airQuality.py and runs it if "startAirQuality" is received. 
- Restarts the device if "Restart" is received.

To add commands edit run() & receive_message_callback().

# Admin
Contains buttons which fire events that call IotHub.cs with predefined string values which is sent to the device. This means that by pressing a button we cant start AirQuality.py or reboot the raspberry pi.


==============================================================
#Temporary Explanations
==============================================================


# Storage

### Blob Storage
	- Lagrer alt som filer.
	- Relativt uoversiktlig. 
	- Fikk det ikke til å fungere med json. De ble parset som tekst. 

### DocumentDB 
	- Lagrer som filer.
	- Enkelt å hente ut data.
	- Json blir parset riktig.
	- Dyrt

### Lake Storage
	- Enkelt å kjøre analyse med Lake Analytics
	- Lagres som filer.
	- Json blir parset riktig. 
	- Billig

# Iot Hub

	- Enkel kommunikasjon mellom clienter og server

# Windows Iot

	- Lite kompatibelt med både azure og grovepi
	- Kan deploye programmer fra Visual Studio (Tidvis upålitelig). Det er bedre å være på samme nett som enheten og deploye over nett istedenfor å koble raspberrien i dataen. 

# Raspbian

	- Kompatibelt med grovepi og iothub
	- Må skrive python
	- Dette blir brukt for å gjøre azure iot kompatibelt med raspbian og python: https://github.com/Azure/azure-iot-sdks (Her må man kanskje kjøre ./build.sh uten unittester (sudo ./build.sh --skip-unittests) og/eller øke swap størrelse)

# API / Nettside

	- Skrevet i C# med Visual Studio MVC prosjekt
	- Alle informasjon går gjennom Apiet på et tidspunkt
	- Informasjon blir sendt vekk fra klient til Apiet så fort som mulig så all logic skjer på serversiden. 
	Dette gjør programmene mindre tunge for klienten hvis programmet etterhvert blir stort. 
	- Apiet sørger for å sende data til lagring i Azure. 
	- Nettsiden inneholder grafisk informasjon om dataene som ligger i azure, samt funksjonalitet for å sende kommandoer til enheten for å starte og stoppe programmer eller restarte enheten.


