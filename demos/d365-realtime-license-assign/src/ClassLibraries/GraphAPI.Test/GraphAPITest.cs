using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Graph;
using Microsoft.Graph.Core;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Generic;

namespace GraphAPI.Test
{
    [TestClass]
    public class GraphAPITest
    {
        private static string userId = "";
        private static string licenseSKU = "";

        [TestMethod]
        public async Task TestGetMe()
        {
            GraphAPI graphClient = setEnvironmentInfo();
            
            User myUser = await graphClient.getUserAsync(userId);

            Assert.IsNotNull(myUser);
            Assert.IsInstanceOfType(myUser,typeof(User));
            
        }

        [TestMethod]
        public async Task TestAssignUserLicense() {

            GraphAPI graphClient = setEnvironmentInfo();

            List<AssignedLicense> addLicenses = new List<AssignedLicense>();

            AssignedLicense licenseToAdd = new AssignedLicense();

            licenseToAdd.SkuId = new Guid(licenseSKU);

            addLicenses.Add(licenseToAdd);

            List<Guid> removeLicenses = new List<Guid>();

            User myUser = await graphClient.assignUserLicenseAsync(userId, addLicenses,removeLicenses);

            Assert.IsNotNull(myUser);
            Assert.IsInstanceOfType(myUser, typeof(User));

        }

        [TestMethod]
        public async Task TestReprocessLicense()
        {
            GraphAPI graphClient = setEnvironmentInfo();

            User myUser = await graphClient.reprocessLicenseAsync(userId);

            Assert.IsNotNull(myUser);
            Assert.IsInstanceOfType(myUser, typeof(User));

        }

        private GraphAPI setEnvironmentInfo() {

            GraphAPI graphAPI = new GraphAPI();

            graphAPI.clientId = "";
            graphAPI.clientSecret = "";
            graphAPI.redirectUri = "";
            //graphAPI.graphScopes = "/.default";
            graphAPI.tenantId = "";

            return graphAPI;
        
        }
    }
}
