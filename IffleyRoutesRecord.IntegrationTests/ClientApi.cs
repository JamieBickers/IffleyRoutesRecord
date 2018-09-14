﻿using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IffleyRoutesRecord.IntegrationTests
{
    public class ClientApi
    {
        private readonly HttpClient httpClient;

        public ClientApi(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<T> GetAsync<T>(Uri uri)
        {
            var httpResult = await httpClient.GetAsync(uri);
            return await httpResult.Content.ReadAsAsync<T>();
        }

        public async Task<TResponse> PostAsync<TContent, TResponse>(Uri uri, TContent content)
        {
            var httpResult = await httpClient.PostAsJsonAsync(uri, content);
            return await httpResult.Content.ReadAsAsync<TResponse>();
        }

        public async Task ShutdownAsync(Uri baseUri)
        {
            await httpClient.GetAsync(new Uri(baseUri, "test/shutdown"));
        }

        public static string Serialize(object data)
        {
            return JsonConvert.SerializeObject(data).ToUpperInvariant();
        }

        private static async Task<string> ProcessHttpResultAsync(HttpResponseMessage httpResult)
        {
            if (!httpResult.IsSuccessStatusCode)
            {
                throw new HttpListenerException();
            }

            string resultContent = await httpResult.Content.ReadAsStringAsync();
            return resultContent.ToUpperInvariant();
        }
    }
}
