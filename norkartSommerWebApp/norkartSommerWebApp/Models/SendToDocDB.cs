using System;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json.Linq;

namespace norkartSommerWebApp.Models
{
    public class SendToDocDb
    {
        private const string EndpointUri = "https://norkartsommer16db.documents.azure.com:443/";
        private const string PrimaryKey = "nbhmi7c5pD1Pr3PrLfIcSu15evszCnE71zUQkURU7rt9fgV5tdSX7Ss4NEEH4v9Y0kIdCxXeoG6aesh43WR82Q==";
        private DocumentClient client;
        Microsoft.ApplicationInsights.TelemetryClient telemetry;
        
        public static async Task Main(JObject value, string dbName)
        {
            
            try
            {
                var p = new SendToDocDb();
                p.init(value, dbName).Wait();
                
            }
            catch (DocumentClientException de)
            {
                var baseException = de.GetBaseException();
                //telemetry.TrackTrace("Exception: " + de);

            }
            catch (Exception e)
            {
                var baseException = e.GetBaseException();
                //telemetry.TrackTrace("Exception: " + e);

            }
            
        }

        //Initiates the connections to the DB
        private async Task init(JObject value, string dbName)
        {
            telemetry = new Microsoft.ApplicationInsights.TelemetryClient();
            this.client = new DocumentClient(new Uri(EndpointUri), PrimaryKey);

            var docName = value.Property("name").Value.ToString();
            //await deadlocks the program
            this.CreateDatabaseIfNotExists(dbName).ConfigureAwait(false);
            
            this.CreateDocumentCollectionIfNotExists(dbName, docName).ConfigureAwait(false);
            
            this.CreateValuesDocumentIfNotExists(dbName, docName, value).ConfigureAwait(false);
            
        }

        private async Task CreateDatabaseIfNotExists(string databaseName)
        {
            // Check to verify a database with the name=databaseName does not exist
            try
            {
                await this.client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(databaseName)).ConfigureAwait(false);
                
            }
            catch (DocumentClientException de)
            {
                // If the database does not exist, create a new database
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    await this.client.CreateDatabaseAsync(new Database { Id = databaseName }).ConfigureAwait(false);
                    telemetry.TrackTrace("Created New Database: " + databaseName);
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task CreateDocumentCollectionIfNotExists(string databaseName, string collectionName)
        {
            try
            {
                await this.client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName)).ConfigureAwait(false);
                
            }
            catch (DocumentClientException de)
            {
                // If the document collection does not exist, create a new collection
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    var collectionInfo = new DocumentCollection
                    {
                        Id = collectionName,
                        IndexingPolicy = new IndexingPolicy(new RangeIndex(DataType.String) {Precision = -1})
                    };

                    //Configure collections for maximum query flexibility including string range queries.

                    // Here we create a collection with 400 RU/s.
                    await this.client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(databaseName),
                        collectionInfo,
                        new RequestOptions { OfferThroughput = 400 }).ConfigureAwait(false);
                    telemetry.TrackTrace("Created New Document Collection: " + collectionName);
                }
                else
                {
                    throw;
                    
                }
            }
        }
        private async Task CreateValuesDocumentIfNotExists(string databaseName, string collectionName, JObject values)
        {
            var jsonId = values.Property("id");
            var items = values.Property("items");
            
            
            try
            {
                await this.client.ReadDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, jsonId.Value.ToString() )).ConfigureAwait(false);
            }
            //If document does not already exist: create new document
            catch (DocumentClientException de)
            {
                System.Diagnostics.Debug.WriteLine("ERROR: " + de);
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), values).ConfigureAwait(false);
                    telemetry.TrackTrace("Created New Document: " + jsonId.Value.ToString());
                }
                else
                {
                    throw;
                }
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("ERROR: " + e);
            }
        }
    }
}