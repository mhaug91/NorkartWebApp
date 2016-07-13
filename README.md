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
	- Fikk det ikke til � fungere med json. De ble parset som tekst. 

### DocumentDB 
	- Lagrer som filer.
	- Enkelt � hente ut data.
	- Json blir parset riktig.
	- Dyrt

### Lake Storage
	- Enkelt � kj�re analyse med Lake Analytics
	- Lagres som filer.
	- Json blir parset riktig. 
	- Billig

# Iot Hub

	- Enkel kommunikasjon mellom clienter og server

# Windows Iot

	- Lite kompatibelt med b�de azure og grovepi
	- Kan deploye programmer fra Visual Studio (Tidvis up�litelig). Det er bedre � v�re p� samme nett som enheten og deploye over nett istedenfor � koble raspberrien i dataen. 

# Raspbian

	- Kompatibelt med grovepi og iothub
	- M� skrive python
	- Dette blir brukt for � gj�re azure iot kompatibelt med raspbian og python: https://github.com/Azure/azure-iot-sdks (Her m� man kanskje kj�re ./build.sh uten unittester (sudo ./build.sh --skip-unittests) og/eller �ke swap st�rrelse)

# API / Nettside

	- Skrevet i C# med Visual Studio MVC prosjekt
	- Alle informasjon g�r gjennom Apiet p� et tidspunkt
	- Informasjon blir sendt vekk fra klient til Apiet s� fort som mulig s� all logic skjer p� serversiden. 
	Dette gj�r programmene mindre tunge for klienten hvis programmet etterhvert blir stort. 
	- Apiet s�rger for � sende data til lagring i Azure. 
	- Nettsiden inneholder grafisk informasjon om dataene som ligger i azure, samt funksjonalitet for � sende kommandoer til enheten for � starte og stoppe programmer eller restarte enheten.


