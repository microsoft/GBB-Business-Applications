using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CaseAPI.PegaClasses
{
    public class Vehicle : Pega
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public bool LiveData { get; set; }
        public string Model { get; set; }
        public string Make { get; set; }
        public string Image { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

    }
}
