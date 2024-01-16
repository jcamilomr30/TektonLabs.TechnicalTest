using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TektonLabs.TechnicalTest.Infraestructure
{
    public abstract class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;
        protected string ServiceName { get; set; }

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void SetAuthentication(string token)
        {
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
        }

        public void SetHeader(string key, string value)
        {
            _httpClient.DefaultRequestHeaders.Add(key, value);
        }

        public async Task<T> GetAsync<T>(string subPath)
        {
            return await GetResponseMessageAsync<T>(await _httpClient.GetAsync(subPath));
        }

        public async Task<T> PostAsync<T>(string subPath, object requestDto)
        {
            return await GetResponseMessageAsync<T>(await _httpClient.PostAsync(subPath, CreateByteArrayContent(requestDto)));
        }

        public async Task PostAsync(string subPath, object requestDto)
        {
            await GetResponseMessageAsync(await _httpClient.PostAsync(subPath, CreateByteArrayContent(requestDto)));
        }

        public async Task<T> PutAsync<T>(string subPath, object requestDto)
        {
            return await GetResponseMessageAsync<T>(await _httpClient.PutAsync(subPath, CreateByteArrayContent(requestDto)));
        }

        public async Task PutAsync(string subPath, object requestDto)
        {
            await GetResponseMessageAsync(await _httpClient.PutAsync(subPath, CreateByteArrayContent(requestDto)));
        }

        public async Task<T> DeleteAsync<T>(string subPath)
        {
            return await GetResponseMessageAsync<T>(await _httpClient.DeleteAsync(subPath));
        }

        public async Task DeleteAsync(string subPath)
        {
            await GetResponseMessageAsync(await _httpClient.DeleteAsync(subPath));
        }

        private async Task<T> GetResponseMessageAsync<T>(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode) await GetResponseMessageAsync(response);

            string responseMessage = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(response.StatusCode == HttpStatusCode.NotFound ? string.Empty : responseMessage);
        }

        private async Task GetResponseMessageAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.NotFound) return;

            string responseMessage = await response.Content.ReadAsStringAsync();

            var errorCatched = JsonConvert.DeserializeAnonymousType(responseMessage, new { Id = Guid.Empty, Error = string.Empty });

            string errorMessage = errorCatched?.Error ?? "Unknown error";
            var statusCode = response.StatusCode;

            if ((int)response.StatusCode >= (int)HttpStatusCode.InternalServerError)
            {
                errorMessage =
                    string.Concat($"Error on {ServiceName} calling { response.RequestMessage.RequestUri }. Status code: { (int)response.StatusCode }. ",
                        errorMessage);

                statusCode = HttpStatusCode.InternalServerError;
            }

            throw new HttpRequestException(errorMessage);
        }

        private ByteArrayContent CreateByteArrayContent(object requestDto)
        {
            var byteContent = new ByteArrayContent(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(requestDto)));

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return byteContent;
        }
    }
}