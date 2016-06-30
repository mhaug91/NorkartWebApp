using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApiController;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System.Configuration;
using System.Linq.Expressions;
using System.Threading.Tasks;
using norkartSommerWebApp.Models;
using System.Net;
using Newtonsoft.Json;

namespace norkartSommerWebApp.Models
{
    public class SendToDocDB
    {
        private const string EndpointUri = "https://norkartsommer16db.documents.azure.com:443/";
        private const string PrimaryKey = "nbhmi7c5pD1Pr3PrLfIcSu15evszCnE71zUQkURU7rt9fgV5tdSX7Ss4NEEH4v9Y0kIdCxXeoG6aesh43WR82Q==";
        private DocumentClient client;
        JsonValues value;
        static Microsoft.ApplicationInsights.TelemetryClient telemetry = new Microsoft.ApplicationInsights.TelemetryClient();

        public static void Main(JsonValues value, string dbName, string docName)
        {
            
            try
            {
                SendToDocDB p = new SendToDocDB();
                p.init(value, dbName, docName).Wait();
                
            }
            catch (DocumentClientException de)
            {
                Exception baseException = de.GetBaseException();
                telemetry.TrackTrace("Exception: " + de);

            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                telemetry.TrackTrace("Exception: " + e);

            }
            
        }

        // ADD THIS PART TO YOUR CODE
        private async Task init(JsonValues value, string dbName, string docName)
        {
            
            this.client = new DocumentClient(new Uri(EndpointUri), PrimaryKey);
            
            this.value = value;
            
            this.CreateDatabaseIfNotExists(dbName).ConfigureAwait(false);
            
            
            this.CreateDocumentCollectionIfNotExists(dbName, docName).ConfigureAwait(false);
            
            
            
            this.CreateValuesDocumentIfNotExists(dbName, docName, value).ConfigureAwait(false);
            
        }

        private async Task CreateDatabaseIfNotExists(string databaseName)
        {
            // Check to verify a database with the id=FamilyDB does not exist
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
                    DocumentCollection collectionInfo = new DocumentCollection();
                    collectionInfo.Id = collectionName;

                    //Configure collections for maximum query flexibility including string range queries.
                    collectionInfo.IndexingPolicy = new IndexingPolicy(new RangeIndex(DataType.String) { Precision = -1 });

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
        private async Task CreateValuesDocumentIfNotExists(string databaseName, string collectionName, JsonValues values)
        {
            try
            {
                await this.client.ReadDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, values.Id)).ConfigureAwait(false);
                //await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), values).ConfigureAwait(false);

            }
            catch (DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), values).ConfigureAwait(false);
                    telemetry.TrackTrace("Created New Document: " + values.Id);
                }
                else
                {
                    throw;
                }
            }
        }
        public class JsonValues
        {
            [JsonProperty(PropertyName = "id")]
            public string Id { get; set; }
            public string name { get; set; }
            public double humidity { get; set; }
            public double temperature { get; set; }
            public string date { get; set; }
            public double longitude { get; set; }
            public double latitude { get; set; }
            public override string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }
        }
    }
}