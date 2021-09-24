using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CaseAPI.PegaClasses
{
    public class Income : Pega
    {
        public string Company { get; set; }
        public int Years { get; set; }
        public int IncomeAmount { get; set; }
        public string Title { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
