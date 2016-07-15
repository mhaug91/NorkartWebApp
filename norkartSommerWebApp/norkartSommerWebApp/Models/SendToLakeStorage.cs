using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.Management.DataLake.Analytics;
using Microsoft.Azure.Management.DataLake.Analytics.Models;
using Microsoft.Azure.Management.DataLake.Store;
using Microsoft.Azure.Management.DataLake.Store.Models;
using Microsoft.Azure.Management.DataLake.StoreUploader;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Reflection;

namespace norkartSommerWebApp.Models
{

    public class SendToLakeStorage
    {
        private static string _clientId;
        private static string _clientKey;
        private static string _tenantId;
        private static string _adlsAccountName;

        private static DataLakeStoreFileSystemManagementClient _adlsFileSystemClient;
        private static DataLakeAnalyticsCatalogManagementClient _adlaCatalogClient;
        private static DataLakeStoreAccountManagementClient _adlsClient;

        public void UploadFile(string srcFilePath, string destFilePath, bool force = true)
        {
            var parameters = new UploadParameters(srcFilePath, "/Telemetry/"+destFilePath+".json", "sommer16datalakeadls", isOverwrite: true);
            
            var frontend = new DataLakeStoreFrontEndAdapter("sommer16datalakeadls", _adlsFileSystemClient);
           

            var uploader = new DataLakeStoreUploader(parameters, frontend);
            
            uploader.Execute();
            
        }

        public async Task<TokenCredentials> Authenticate(string tenantId, string clientId, string clientKey)
        {
            var authContext = new AuthenticationContext("https://login.microsoftonline.com/"+tenantId);
            var creds = new ClientCredential(_clientId, _clientKey);
            var tokenAuthResult = await authContext.AcquireTokenAsync("https://management.core.windows.net/", creds);
            return new TokenCredentials(tokenAuthResult.AccessToken);
            
            
              
        }

        public void SetupClients(TokenCredentials tokenCreds, string subscriptionId)
        {
            _adlaCatalogClient = new DataLakeAnalyticsCatalogManagementClient(tokenCreds);

            _adlsClient = new DataLakeStoreAccountManagementClient(tokenCreds) {SubscriptionId = subscriptionId};

            _adlsFileSystemClient = new DataLakeStoreFileSystemManagementClient(tokenCreds);
        }

        public static void DownloadFile(string srcPath, string destPath)
        {

            try
            {
                var stream = _adlsFileSystemClient.FileSystem.Open(_adlsAccountName, srcPath);
                Console.WriteLine(stream);
                var fileStream = new FileStream(destPath, FileMode.Create);

                stream.CopyTo(fileStream);
                fileStream.Close();
                stream.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }


        }

        private static string CreateTmpFile()
        {
            string fileName = string.Empty;
            
            try
            {
                // Get the full name of the newly created Temporary file. 
                // Note that the GetTempFileName() method actually creates
                // a 0-byte file and returns the name of the created file.
                fileName = Path.GetTempFileName();

                // Craete a FileInfo object to set the file's attributes
                var fileInfo = new FileInfo(fileName) {Attributes = FileAttributes.Temporary};

                // Set the Attribute property of this file to Temporary. 
                // Although this is not completely necessary, the .NET Framework is able 
                // to optimize the use of Temporary files by keeping them cached in memory.

                Debug.WriteLine("TEMP file created at: " + fileName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to create TEMP file or set its attributes: " + ex.Message);
            }

            return fileName;
        }

        private static void UpdateTmpFile(string tmpFile, JObject obj)
        {
            try
            {
                // Write to the temp file.
                File.WriteAllText(tmpFile, string.Empty);
                var streamWriter = File.AppendText(tmpFile);
                
                //File.WriteAllText(tmpFile, string.Empty);
                streamWriter.Write(obj);
                streamWriter.Flush();
                streamWriter.Close();

                Console.WriteLine("TEMP file updated.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error writing to TEMP file: " + ex.Message);
            }
        }



        public static async Task Main(JObject obj)
        {

            _clientId = "adabc3e0-c180-4ccc-b33c-1bc1d3290b4a";
            _tenantId = "0591ba1f-7671-4e03-9b2f-b1800a28a4ff";
            _clientKey = "iPi6xyMix1T4nv8LFSO4HEtwy10mdwyq91z5mE/eA18=";
            _adlsAccountName = "sommer16datalakeadls";


            var s = new SendToLakeStorage();
            //string file = CreateTmpFile();
            var outPutDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            // Debug.WriteLine("HEI: " + outPutDirectory);
            var iconPath = Path.Combine(outPutDirectory, "temp.json");

            //string tempFile = "temp.json";

            var tempFile = new Uri(iconPath).LocalPath;

            Debug.WriteLine("tempfile: " + tempFile);

            UpdateTmpFile(tempFile, obj);

            var tokenCreds = await s.Authenticate(_tenantId, _clientId, _clientKey);

            _adlsFileSystemClient = new DataLakeStoreFileSystemManagementClient(tokenCreds);

            s.SetupClients(tokenCreds, "3f0352ad-ff15-4d3f-ad65-1af7929780f1");

            var objectId = obj.Property("id").Value;
            Debug.WriteLine("OBJECTID:  " + objectId);

            s.UploadFile(tempFile, objectId.ToString());
            
    
        }
    }
}