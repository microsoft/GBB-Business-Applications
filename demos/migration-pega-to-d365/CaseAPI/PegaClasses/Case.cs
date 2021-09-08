using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CaseAPI.PegaClasses
{
    public partial class Case : Pega
    {
        public string pyID { get; set; }
        public string pyStatusWork { get; set; }
        public string pxCurrentStageLabel { get; set; }
        public string id { get; set; }
        public string partitionId { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
