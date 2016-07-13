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



