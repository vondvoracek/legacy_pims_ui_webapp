////using Microsoft.Graph;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Azure.Identity;
////using Microsoft.Graph.Models;
//using MI.PIMS.BL.Services.Interfaces;

//namespace MI.PIMS.BL.Services
//{
//    public class GraphHandlerService: IGraphHandlerService
//    {
//        string tenantId = "****";
//        string clientId = "****";
//        string clientSecret = "****";

//        public GraphServiceClient GraphClient { get; private set; }
//        public GraphHandlerService()
//        {

//            GraphClient = CreateGraphClient(tenantId, clientId, clientSecret);
//        }





//        public GraphServiceClient CreateGraphClient(string tenantId, string clientId, string clientSecret)
//        {
//            var options = new TokenCredentialOptions
//            {
//                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
//            };

//            var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret, options);
//            var scopes = new[] { "https://graph.microsoft.com/.default" };

//            return new GraphServiceClient(clientSecretCredential, scopes);
//        }


//        public async Task<User> GetAzureADUser(string userPrincipalName)
//        {
//            return await GraphClient.Users[userPrincipalName].GetAsync();
//        }
//    }
//}
