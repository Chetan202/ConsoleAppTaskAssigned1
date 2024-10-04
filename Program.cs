using Microsoft.Xrm.Tooling.Connector;
using System;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;

namespace ConsoleAppTaskAssigned1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Replace with your connection string
            string connectionString = @"AuthType=OAuth;Url=https://orgd7e5c4b1.crm8.dynamics.com/;Username=MahithReddy@Pascalcase333.onmicrosoft.com;Password=Powerapps@2024;AppId=51f81489-12ee-4a9e-aaae-a2591f45987d;RedirectUri=app://58145B91-0C36-4500-8554-080854F2AC97";

            CrmServiceClient service = new CrmServiceClient(connectionString);

            if (!service.IsReady)
            {
                Console.WriteLine("Error connecting to CRM: " + service.LastCrmError);
                return;
            }

            // Variables for pagination
            int pageSize = 5000; // Number of records per page
            int pageNumber = 1; // Start at page 1
            string pagingCookie = null; // Initialize paging cookie

            while (true)
            {
                // Create a query to retrieve records from contoso_testingtable
                QueryExpression query = new QueryExpression("contoso_testingtable")
                {
                    ColumnSet = new ColumnSet("contoso_firstname", "contoso_lastname"),
                    Criteria = new FilterExpression
                    {
                        Conditions =
                        {
                            new ConditionExpression("statecode", ConditionOperator.Equal, 0) // Ensure only active records are retrieved
                        }
                    },
                    PageInfo = new PagingInfo
                    {
                        Count = pageSize,
                        PageNumber = pageNumber,
                        PagingCookie = pagingCookie // Use the current paging cookie for subsequent requests
                    }
                };

                // Retrieve the results
                EntityCollection results = service.RetrieveMultiple(query);

                // Display the total count of records and actual records for the current page
                Console.WriteLine($"\nPage {pageNumber}:");
                Console.WriteLine($"Number of records: {results.Entities.Count}");

                //// Loop through and display each record's first name and last name
                //foreach (var entity in results.Entities)
                //{
                //    string firstName = entity.Contains("contoso_firstname") ? entity["contoso_firstname"].ToString() : "N/A";
                //    string lastName = entity.Contains("contoso_lastname") ? entity["contoso_lastname"].ToString() : "N/A";
                //    Console.WriteLine($"First Name: {firstName}, Last Name: {lastName}");
                //}

                // Update paging cookie for the next page
                pagingCookie = results.PagingCookie;

                // Check if there are more pages
                if (results.MoreRecords)
                {
                    Console.WriteLine("Press 'N' for next page or 'E' to exit.");
                    var key = Console.ReadKey(true).KeyChar;
                    if (key == 'n' || key == 'N')
                    {
                        pageNumber++;
                    }
                    else if (key == 'e' || key == 'E')
                    {
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("No more records available.");
                    break;
                }
            }

            // Prompt to keep the console window open
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
