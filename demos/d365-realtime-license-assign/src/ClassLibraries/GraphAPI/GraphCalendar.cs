using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Graph;
using Microsoft.Graph.Core;
using Microsoft.Graph.Extensions;
using Microsoft.Identity.Client;
using Microsoft.Graph.Auth;
using Microsoft.Extensions.Logging;

namespace GraphAPI
{
    class GraphCalendar : GraphAPI
    {
        public static Calendar graphCalendar { get; set; }

        public async Task<Calendar> getCalendarAsync() {

            GraphServiceClient graphClient = createGraphCilent();

            try
            {
                //
            }
            catch (Exception ex) {

                logException(ex);
            }

            return graphCalendar;
        }
    }
}
