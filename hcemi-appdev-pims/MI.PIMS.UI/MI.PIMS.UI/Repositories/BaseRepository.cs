
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;
using MI.PIMS.UI.Services;
using MI.PIMS.UI.Common;

namespace MI.PIMS.UI.Repositories
{
    public abstract class BaseRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _clientName;

        public BaseRepository(IHttpClientFactory clientFactory, string clientName = "HttpClient")
        {
            _clientFactory = clientFactory;
            _clientName = clientName;
        }

        #region GET
        protected async Task<T> GetAsync<T>(string apiUrl)
        {
            var result = default(T);
            var client = _clientFactory.CreateClient(_clientName);
            var response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                result = await System.Text.Json.JsonSerializer.DeserializeAsync<T>(responseStream);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                response.Content?.Dispose();
                throw new HttpRequestException($"{response.StatusCode}:{content}");
            }

            return result;
        }

        protected async Task<IEnumerable<T>> GetAsyncList<T>(string apiUrl, int timeOut = 600)
        {
            IEnumerable<T> result = null;
            var client = _clientFactory.CreateClient(_clientName);
            client.Timeout = TimeSpan.FromSeconds(timeOut);
            var response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();

                result = await System.Text.Json.JsonSerializer.DeserializeAsync<IEnumerable<T>>(responseStream);
             
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                response.Content?.Dispose();
                throw new HttpRequestException($"{response.StatusCode}:{content}");
            }
            return result;
        }

        #endregion
        
        #region POST
        protected async Task<T> PostAsync<T, PL>(string apiUrl, PL payload, CancellationToken cancellationToken = default(CancellationToken))
        {
            T result = default(T);

            // Serialize our concrete class into a JSON String
            //var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(payload));
            var stringPayload = JsonConvert.SerializeObject(payload);

            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            var client = _clientFactory.CreateClient(_clientName);
            var response = await client.PostAsync(apiUrl, httpContent, cancellationToken).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    result = JsonConvert.DeserializeObject<T>(x.Result);
                }, cancellationToken);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                response.Content?.Dispose();
                throw new HttpRequestException($"{response.StatusCode}:{content}");
            }
            return result;
        }
        protected async Task<T> PostAsync<T>(string apiUrl, object obj, int timeOut = 600)
        {
            var result = default(T);
            var client = _clientFactory.CreateClient(_clientName);
            client.Timeout = TimeSpan.FromSeconds(timeOut);
            var jsonObj = new StringContent(System.Text.Json.JsonSerializer.Serialize(obj),Encoding.UTF8,"application/json");

            using var response = await client.PostAsync(apiUrl, jsonObj);
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                result = await System.Text.Json.JsonSerializer.DeserializeAsync<T>(responseStream);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                response.Content?.Dispose();
                throw new HttpRequestException($"{response.StatusCode}:{content}");
            }

            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Return Object Class</typeparam>
        /// <param name="apiUrl">Url of the end point</param>
        /// <param name="obj">Payload</param>
        /// <param name="timeOut">Time out period in seconds for the request</param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        protected async Task<IEnumerable<T>> PostAsyncList<T>(string apiUrl, object obj, int timeOut = 600)
        {
            IEnumerable<T> result = null;
            var client = _clientFactory.CreateClient(_clientName);
            client.Timeout = TimeSpan.FromSeconds(600);
            var jsonObj = new StringContent(System.Text.Json.JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");

            using var response = await client.PostAsync(apiUrl, jsonObj);
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                result = await System.Text.Json.JsonSerializer.DeserializeAsync<IEnumerable<T>>(responseStream);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                response.Content?.Dispose();
                throw new HttpRequestException($"{response.StatusCode}:{content}");
            }

            return (IEnumerable<T>)result;
        }


        #endregion

        #region PUT
        protected async Task<T> PutAsync<T>(string apiUrl, object obj)
        {
            var result = default(T);
            var client = _clientFactory.CreateClient(_clientName);
            var jsonObj = new StringContent(System.Text.Json.JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");

            using var response = await client.PutAsync(apiUrl, jsonObj);
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                result = await System.Text.Json.JsonSerializer.DeserializeAsync<T>(responseStream);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                response.Content?.Dispose();
                throw new HttpRequestException($"{response.StatusCode}:{content}");
            }

            return result;
        }

        protected async Task<IEnumerable<T>> PutAsyncList<T>(string apiUrl, object obj)
        {
            var result = default(T);
            var client = _clientFactory.CreateClient(_clientName);
            var jsonObj = new StringContent(System.Text.Json.JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");

            using var response = await client.PostAsync(apiUrl, jsonObj);
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                result = (T)await System.Text.Json.JsonSerializer.DeserializeAsync<IEnumerable<T>>(responseStream);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                response.Content?.Dispose();
                throw new HttpRequestException($"{response.StatusCode}:{content}");
            }

            return (IEnumerable<T>)result;
        }


        #endregion

        #region DELETE
        protected async Task<T> DeleteAsync<T>(string apiUrl)
        {
            var result = default(T);
            var client = _clientFactory.CreateClient(_clientName);

            using var response = await client.DeleteAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                result = await System.Text.Json.JsonSerializer.DeserializeAsync<T>(responseStream);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                response.Content?.Dispose();
                throw new HttpRequestException($"{response.StatusCode}:{content}");
            }

            return result;
        }


        #endregion

    }
}
