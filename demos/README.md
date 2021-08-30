# Real-Time License Assignment with Graph API

This sample contains a class library that demonstrates how to assign a license to a user in real-time using the Graph API. This capability could be leveraged in a scenairo where there are large numbers of users migrating over to a new application, or as a standard automation within a companies Joiner-Mover-Leaver (JML) process.

> [!NOTE]
> To test this class library in action, simply leverage the unit test class included with the solution, simply copy or clone this repository and update the following code snippets in the GraphAPITest.cs file.

```csharp
    private static string userId = "";
    private static string licenseSKU = "";
```

and

```csharp
     private GraphAPI setEnvironmentInfo() {

            GraphAPI graphAPI = new GraphAPI();

            graphAPI.clientId = "";
            graphAPI.clientSecret = "";
            graphAPI.redirectUri = "";
            //graphAPI.graphScopes = "/.default";
            graphAPI.tenantId = "";

            return graphAPI;
        
        }
```

## Solution Explanation

The Graph API provides a robust set of tools for managing your Azure AD environment. In this scenario there are two key steps for handling the license assignment, the assignment itself and the reprocessing of the license. The second step is what enables this scenario to be leveraged in real time, as it removes the background delay caused by waiting for the system jobs to update the license assignments. 

> [!IMPORTANT]
> This scenario does not address the administrative refresh delay between Azure AD and Dataverse. If you need ensure that new users being created are synchronized in near-real time with Dataverse, then consider leveraging the [Force Sync Azure AD Group Members to Dataverse](https://preview.flow.microsoft.com/en-us/galleries/public/templates/6e4162ca7afc48479e3ad1caadc6c1e6/force-sync-azure-active-directory-group-members-to-specified-cds-instance/) Power Automate template