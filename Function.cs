using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace updateFacultyList
{
    public static class Function
    {
        [FunctionName("Function")]
        public static void Run([CosmosDBTrigger(
            databaseName: "databaseName",
            collectionName: "collectionName",
            ConnectionStringSetting = "ConnectionStrings:AccountEndpoint=https://faculty-availability-dashboard.documents.azure.com:443/;AccountKey=KsbXgkqIoVF5K0fFm2hPgvLRQNtyE4Fno1z1xAzM1w890h0EoPoqicJ48JbftTIfujixCoBpwOGvACDbd6vZpg==;",
            LeaseCollectionName = "leases")]IReadOnlyList<Document> input,
            ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                log.LogInformation("Documents modified " + input.Count);
                log.LogInformation("First document Id " + input[0].Id);
            }
        }
    }
}
