using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Common
{
    public class CloudMailClient
    {
        private readonly Helper _helper;

        public CloudMailClient(Helper helper)
        {
            _helper = helper;
        }

        public async Task<HttpResponseMessage> GetAuthTokenAsync()
        {
            var api_url = _helper.CloudMailServiceAuthTokenUrl;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(api_url);
                client.DefaultRequestHeaders.Accept.Clear();

                var nameValueCollection = new NameValueCollection();
                nameValueCollection.Add("client_id", _helper.CloudMailServiceClientId);
                nameValueCollection.Add("client_secret", _helper.CloudMailServiceClientSecret);
                nameValueCollection.Add("grant_type", _helper.CloudMailServiceGrantType);
                var sc = string.Join("&", nameValueCollection.AllKeys.SelectMany(k => nameValueCollection.GetValues(k).Select(v => $"{Uri.EscapeDataString(k)}={Uri.EscapeDataString(v)}")));

                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(api_url),
                    Headers = {
                        { HttpRequestHeader.Accept.ToString(), "*/*" }
                    },
                    Content = new StringContent(sc, Encoding.UTF8, "application/x-www-form-urlencoded")
                };

                var response = await client.SendAsync(httpRequestMessage);
                return response;
            }
        }

        public async Task<HttpResponseMessage> SendMailAsyn(CloudToken token, CloudMailBody body)
        {
            var bearerToken = $"{token.token_type} {token.access_token}";

            var test = JsonConvert.SerializeObject(body);
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(_helper.CloudMailServiceSendMailUrl),
                    Headers = {
                        { HttpRequestHeader.Authorization.ToString(), bearerToken },
                        { HttpRequestHeader.Accept.ToString(), "*/*" },
                    },
                    Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(body))
                };
                httpRequestMessage.Headers.Add("x-app-nm", "origin");
                var response = await client.SendAsync(httpRequestMessage); 
               
                return response;
            };
        }
    }

    public class CloudToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
    }

    public class CloudMailBody
    {
        public List<string> to { get; set; }
        public string from { get; set; }
        public string subject { get; set; }
        public string html { get; set; }
        public context context { get; set; }
        public options options { get; set; }
    }

    public class context
    {
        public string communicationConfig { get; set; }
    }

    public class options
    {
    }
}
