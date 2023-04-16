using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Documents.Client;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace updateFacultyList
{
    public static class updateFacultyList
    {
        private static readonly string endpointUrl = "https://faculty-availability-dashboard.documents.azure.com:443/";
        private static readonly string primaryKey = "KsbXgkqIoVF5K0fFm2hPgvLRQNtyE4Fno1z1xAzM1w890h0EoPoqicJ48JbftTIfujixCoBpwOGvACDbd6vZpg==";
        private static readonly string databaseId = "FacultyAvailabilityDB";
        private static readonly string containerId = "FacultyAvailabilityCollection";

        [FunctionName("updateFacultyList")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // Fetch data from Azure Cosmos DB
            CosmosClient cosmosClient = new CosmosClient(endpointUrl, primaryKey);
            Microsoft.Azure.Cosmos.Container container = cosmosClient.GetContainer(databaseId, containerId);
            FeedIterator<Faculty> feedIterator = container.GetItemQueryIterator<Faculty>("SELECT * FROM c");
            List<Faculty> facultyList = new List<Faculty>();
            while (feedIterator.HasMoreResults)
            {
                Microsoft.Azure.Cosmos.FeedResponse<Faculty> response = await feedIterator.ReadNextAsync();
                facultyList.AddRange(response.ToList());
            }

            // Convert facultyList to JSON
            string json = JsonConvert.SerializeObject(facultyList);

            return new OkObjectResult(json);

        }

        public class Faculty
        {
            public string name { get; set; }
            public string status { get; set; }
        }
    }
}
