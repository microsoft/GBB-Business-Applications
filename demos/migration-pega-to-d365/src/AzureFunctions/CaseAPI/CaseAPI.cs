using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using CaseAPI.PegaClasses;
using Microsoft.Azure.Cosmos;
using CaseAPI.Model;
using Newtonsoft.Json;

namespace CaseAPI
{
    public static class CaseAPI
    {

       
        [FunctionName("CaseAPI")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "case_id", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Case ID** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            //log.LogInformation("C# HTTP trigger function processed a request for " );
            
            string caseId = req.Query["case_id"];

            log.LogInformation("C# HTTP trigger function processed a request for {0}.", caseId);

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            AutoLoan data = JsonConvert.DeserializeObject<AutoLoan>(requestBody);
            caseId = caseId ?? data?.pyID;
            string responseMessage = "";
            bool err = false;

            
            if (req.Method.ToUpper() == "GET")
            {

                AutoLoan autoLoan = new AutoLoan();

                autoLoan = await autoLoan.GetFromCosmos(caseId);

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(autoLoan, Formatting.Indented),System.Text.Encoding.UTF8,"application/json")
                };

                //responseMessage = autoLoan.ToString();              
                                
            }
            else if (req.Method.ToUpper() == "POST")
            {
                
                if (data is AutoLoan) {

                    await data.CommitToCosmos(data);

                    ServiceBus serviceBus = new ServiceBus();

                    await serviceBus.SendAsync(data.pyID, "pegaautoloans");
                    
                    responseMessage = "Case synchronized";

                }
                else
                {
                    err = true;
                    responseMessage = "Unable to properly parse the request body";
                    
                }

                                
            }
            else {
                err = true;
                responseMessage = "This method is not supported";
                
            }

            return err
                ? new HttpResponseMessage(HttpStatusCode.BadRequest) {

                    Content = new StringContent(JsonConvert.SerializeObject(responseMessage))
                }
                : new HttpResponseMessage(HttpStatusCode.OK) {

                    Content = new StringContent(JsonConvert.SerializeObject(responseMessage))

                };
            
            
        }


        [FunctionName("CaseAttachmentsAPI")]
        [OpenApiOperation(operationId: "SyncAttachment", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "case_id", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Case ID** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
        public static async Task<HttpResponseMessage> SyncAttachment([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            string caseId = req.Query["case_id"];

            log.LogInformation("C# HTTP trigger function processed a request for {0}.", caseId);

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Attachment data = JsonConvert.DeserializeObject<Attachment>(requestBody);
            caseId = caseId ?? data?.ReferenceCaseID;
            string responseMessage = "";
            bool err = false;

            if (req.Method.ToUpper() == "GET")
            {
                //Not implemented
                responseMessage = "GET Method not implemented yet";

            }
            else if (req.Method.ToUpper() == "POST") {

                ServiceBus serviceBus = new ServiceBus();

                string dataString = data.ToString();

                await serviceBus.SendAsync(dataString, "pegaattachments");

                responseMessage = "Attachment Queued";
            }

            return err
                ? new HttpResponseMessage(HttpStatusCode.BadRequest)
                {

                    Content = new StringContent(JsonConvert.SerializeObject(responseMessage))
                }
                : new HttpResponseMessage(HttpStatusCode.OK)
                {

                    Content = new StringContent(JsonConvert.SerializeObject(responseMessage))

                };

        }

    }
}

