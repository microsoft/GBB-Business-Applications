using System.IO;
using System.Net;
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

namespace CaseAPI
{
    public static class CaseAPI
    {

       
        [FunctionName("CaseAPI")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "case_id", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Case ID** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            
            string caseId = req.Query["case_id"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            AutoLoan data = JsonConvert.DeserializeObject<AutoLoan>(requestBody);
            caseId = caseId ?? data?.pyID;

            string responseMessage = "";

            if (req.Method.ToUpper() == "GET")
            {

                // return the case from cosmos

                /*PLACEHOLDER CODE*/
                responseMessage = string.IsNullOrEmpty(caseId)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {caseId}. This HTTP triggered function executed successfully.";
            }
            else if (req.Method.ToUpper() == "POST")
            {
                //handle upsert

                if (data is AutoLoan) {

                    await data.CommitToCosmos(data);



                    responseMessage = "Case synchronized";

                }
                else
                {
                    responseMessage = "Unable to properly parse the request body";
                    return new BadRequestObjectResult(responseMessage);
                }

                                
            }
            else {
                responseMessage = "This method is not supported";
                return new BadRequestObjectResult(responseMessage);
            }

            return new OkObjectResult(responseMessage);
        }

        
        
    }
}

