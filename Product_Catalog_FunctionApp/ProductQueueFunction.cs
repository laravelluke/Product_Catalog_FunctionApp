using System;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Product_Catalog_FunctionApp
{
    public class Product : TableEntity
    {
        public string Name { get; set; }
        public double Price { get; set; }

    } 


    public class ProductQueueFunction
    {
        [FunctionName("ProductQueueFunction")]
        [return: Table("productTable")]
        public static Product Run([QueueTrigger("productqueue", Connection = "StorageConnectionString")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
            string[] messageSplitted = myQueueItem.Split(' ');
            string name = messageSplitted[1];
            double price = 0.0;

            Double.TryParse(messageSplitted[3], out price);


            if(price > 100.0 || name.Contains("a"))
            {
                return new Product { PartitionKey = "Product", RowKey = Guid.NewGuid().ToString(), Name = name, Price = price }; 
            }

            return null;
            
            
            
        }
    }
}
