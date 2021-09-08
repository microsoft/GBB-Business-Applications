using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CaseAPI.PegaClasses
{
    public partial class Pega
    {
        public string pyLabel { get; set; }
        public string pyNote { get; set; }
        public string pxCreateDateTime { get; set; }
        public string pzInsKey { get; set; }
        public string pyTemplateDisplayText { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
