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
            autoLoan.partitionId = autoLoan.Applicant.LastName;
            autoLoan.LastName = partitionId;

            CosmosDB cosmosDB = new CosmosDB();

            await cosmosDB.CreateOrUpdateItemAsync(autoLoan);

            ServiceBus serviceBus = new ServiceBus();
            await serviceBus.SendAsync(autoLoan.pyID);
        }
    }
}
