using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CaseAPI.PegaClasses
{
    public class Attachment : Pega
    {
        public string pxAttachName { get; set; }
        public string pyStoredInRepository { get; set; }
        public string  pyContentLocation { get; set; }
        public string pyRepositoryFileName { get; set; }
        public string ReferenceCaseID { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
