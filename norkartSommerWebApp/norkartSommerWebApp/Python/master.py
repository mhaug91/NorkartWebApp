import os
import airQuality
import threading
import time
import sys
import iothub_client
from iothub_client import *
from iothub_client_args import *

timeout = 241000
minimum_polling_time = 9
message_timeout = 10000
receive_context = 0
runningProcess = 1
messageReceived = ""

# chose HTTP, AMQP or MQTT as transport protocol
protocol = IoTHubTransportProvider.AMQP

# String containing Hostname, Device Id & Device Key in the format:
# "HostName=<host_name>;DeviceId=<device_id>;SharedAccessKey=<device_key>"
connection_string = "HostName=norkartiothub.azure-devices.net;DeviceId=Pi1;SharedAccessKey=vQ6yFQMCW3mSx50SXmPX/PI9QHQOGCKqZip15QFo94E="

def set_certificates(iotHubClient):
    from iothub_client_cert import certificates
    try:
        iotHubClient.set_option("TrustedCerts", certificates)
        print("set_option TrustedCerts successful")
    except IoTHubClientError as e:
        print("set_option TrustedCerts failed (%s)" % e)



def receive_message_callback(message, counter):
    global runningProcess
    global messageReceived
    messageReceived = ""
    buffer = message.get_bytearray()
    size = len(buffer)
    messageString = (buffer[:size].decode('utf-8'))
    if messageString == "airquality":
        runningProcess = 1
    elif messageString == "Restart":
        print("Restart")
        runningProcess = 2
    else:
        runningProcess = 3
        messageReceived = messageString
    print("    Data: <<<%s>>> & Size=%d" % (buffer[:size].decode('utf-8'), size))
    map_properties = message.properties()
    key_value_pair = map_properties.get_internals()
    print("    Properties: %s" % key_value_pair)
    return IoTHubMessageDispositionResult.ACCEPTED



def send_confirmation_callback(message, result, user_context):
    print(
        "Confirmation[%d] received for message with result = %s" %
        (user_context, result))
    map_properties = message.properties()
    print("    message_id: %s" % message.message_id)
    print("    correlation_id: %s" % message.correlation_id)
    key_value_pair = map_properties.get_internals()
    print("    Properties: %s" % key_value_pair)



def iothub_client_init():
    # prepare iothub client
    iotHubClient = IoTHubClient(connection_string, protocol)

    # set the time until a message times out
    iotHubClient.set_option("messageTimeout", message_timeout)
    iotHubClient.set_message_callback(receive_message_callback, receive_context)
    return iotHubClient

def run():
    global runningProcess
    global messageReceived
    try:

        iotHubClient = iothub_client_init()
        while True:
            iotHubClient
            if runningProcess == 1:
                thr = threading.Thread(target=airQuality.run(), args=(), kwargs={})
                airQuality.run()
                runningProcess = 0
            if runningProcess == 2:
                print("shutting down")
                os.system('sudo reboot')
            if runningProcess == 3:
                try:
                    os.system(messageReceived)
                except:
                    continue
            runningProcess = 0
    except IoTHubError as e:
        print("Unexpected error %s from IoTHub" % e)
        return
    except KeyboardInterrupt:
        print("IoTHubClient sample stopped")

if __name__ == '__main__':
    print("\nPython %s" % sys.version)
    print("IoT Hub for Python SDK Version: %s" % iothub_client.__version__)

    try:
        (connection_string, protocol) = get_iothub_opt(sys.argv[1:], connection_string, protocol)
    except OptionError as o:
        print(o)
        usage()
        sys.exit(1)

    print("Starting the IoT Hub Python sample...")
    print("    Protocol %s" % protocol)
    print("    Connection string=%s" % connection_string)

    run()