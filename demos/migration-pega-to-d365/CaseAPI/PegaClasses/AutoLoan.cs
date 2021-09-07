using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CaseAPI.PegaClasses
{
    public class AutoLoan : Case
    {
        public int LoanAmount { get; set; }
        public Address Address { get; set; }
        public Applicant Applicant { get; set; }
        public Income Income { get; set; }
        public Vehicle Vehicle { get; set; }
        public Contact Contact { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
