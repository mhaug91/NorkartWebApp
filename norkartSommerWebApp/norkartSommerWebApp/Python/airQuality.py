import requests                                                                                    
import time                                                                                        
import grovepi                                                                                     
import atexit                                                                                      
import datetime                                                                                    
import json                                                                                        
                                                                                                   
                                                                                                   
def run():                                                                                         
    atexit.register(grovepi.dust_sensor_dis)                                                       
    grovepi.dust_sensor_en()                                                                       
    while True:                                                                                    
                                                                                                   
        try:                                                                                       
            [new_val, lowpulsoccupancy] = grovepi.dustSensorRead()                                 
            print ("New Val: " +  str(new_val))                                                    
            print ("Occupancy: " + str(lowpulsoccupancy))                                          
            headers = {'content-type' : 'application/json'}                                        
            if new_val:                                                                            
                data = {}                                                                          
                data['id'] = time.strftime("%Y-%m-%dT%H:%M:%SZ")                                   
                #data['id'] = time.strftime("%Y-%m-%d %H:%M:%S")                                   
                #data['id'] = time.strftime("%Y%m%d%H%M%S")                                        
                print(data['id'])                                                                  
                data['telemetry'] = lowpulsoccupancy                                               
                data['name'] = 'Arduberry3'                                                        
                json_data = json.dumps(data)                                                       
                url = "http://norkartsommerwebapp.azurewebsites.net/api/Values/PostAirQuality"     
                res = requests.post(url, data=json_data, headers=headers)                          
                print (res.status_code)                                                            
                #print (res.text)                                                                  
            time.sleep(5)                                                                          
        except IOError, e:                                                                         
            print (str(e))                                                                         
                                                                                                   
if __name__ == '__main__':                                                                         
    run()                                                                                          