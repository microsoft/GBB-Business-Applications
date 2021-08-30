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
    public class GraphAPI
    {

        public string clientId {get; set;}
        public string redirectUri { get; set; }
        public string clientSecret { get; set; }
        //public string graphScopes { get; set; }
        public string tenantId { get; set; }

        public async Task<User> getUserAsync(string userId) {

            GraphServiceClient graphClient = createGraphCilent();
                        
            try
            {
                var user = await graphClient.Users[userId].Request().GetAsync();

                return user;
            }
            catch (Exception ex)
            {
                logException(ex);
                throw new Exception("API request failed", ex);
                                                
            }


        }

        public async Task<User> assignUserLicenseAsync(string userId, IEnumerable<AssignedLicense> addLicenses, IEnumerable<Guid> removeLicenses)
        {
            GraphServiceClient graphClient = createGraphCilent();

            try
            {
                var user = await graphClient.Users[userId].AssignLicense(addLicenses, removeLicenses).Request().PostAsync();
                
                user = await reprocessLicenseAsync(user.Id);

                return user;
            }
            catch (Exception ex)
            {

                logException(ex);
                throw new Exception("API request failed", ex);
            }
        
        }

        public async Task<User> reprocessLicenseAsync(string userId) {

            GraphServiceClient graphClient = createGraphCilent();

            try
            {
                var user = await graphClient.Users[userId].ReprocessLicenseAssignment().Request().PostAsync();

                return user;
            }
            catch (Exception ex)
            {
                logException(ex);
                throw new Exception("API request failed", ex);

            }

        }

        protected GraphServiceClient createGraphCilent() {

            IConfidentialClientApplication client = ConfidentialClientApplicationBuilder.Create(clientId)
            .WithRedirectUri(redirectUri)
            .WithClientSecret(clientSecret)
            .WithTenantId(tenantId)
            .Build();

            ClientCredentialProvider authProvider = new ClientCredentialProvider(client);

            GraphServiceClient graphClient = new GraphServiceClient(authProvider);

            return graphClient;
         }

        protected void logException(Exception ex) { 
        
            //Add Exception Code Here
        }
    }
}
