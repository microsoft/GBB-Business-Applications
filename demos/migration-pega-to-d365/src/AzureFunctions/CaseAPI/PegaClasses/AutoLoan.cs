using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Azure.Cosmos;
using CaseAPI.Model;

namespace CaseAPI.PegaClasses
{
    public class AutoLoan : Case
    {
        public int LoanAmount { get; set; }
        public int PurchasePrice { get; set; }
        public string PdfImage { get; set; }
        public Address Address { get; set; }
        public Applicant Applicant { get; set; }
        public Income Income { get; set; }
        public Vehicle Vehicle { get; set; }
        public Contact Contact { get; set; }
        public string LastName { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

        public async Task CommitToCosmos(AutoLoan autoLoan) {

            autoLoan.id = autoLoan.pyID;
            autoLoan.caseType = "AutoLoan";
            autoLoan.partitionId = autoLoan.caseType;
            

            CosmosDB cosmosDB = new CosmosDB();

            await cosmosDB.CreateOrUpdateItemAsync(autoLoan);

            
        }

        public async Task<AutoLoan> GetFromCosmos(string caseId) {

            PartitionKey partitionKey = new PartitionKey("AutoLoan");

            CosmosDB cosmosDB = new CosmosDB();

            AutoLoan autoLoan = await cosmosDB.GetExistingAutoLoan(caseId, partitionKey);

            return autoLoan;
        
        }
    }
}
