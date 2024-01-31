namespace NovaFinds.MVC.API
{
    using Errors;
    using IFR.Logger;
    using System.Net;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;

    public class ApiClient(IConfiguration config)
    {
        private const string ApplicationJson = "application/json";

        private const string XApiKeyHeader = "X-Api-Key";

        private string? Url { get; set; } = config.GetSection("Config").GetSection("Apis").GetSection("NovaFinds").Value;

        private string? ApiKey { get; set; } = config.GetSection("Config").GetSection("ApiKeys").GetSection("NovaFinds").Value;

        private HttpClient GenerateHttpClient()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ApplicationJson));
            httpClient.DefaultRequestHeaders.Add(XApiKeyHeader, this.ApiKey);
            return httpClient;
        }

        public async Task<T?> Get<T>(string action)
        {
            Logger.Debug($"Doing {WebRequestMethods.Http.Get} request to: {action}");
            var httpClient = GenerateHttpClient();
            var result = await httpClient.GetAsync(this.Url + action);
            result.EnsureSuccessStatusCode();
            var resultContent = result.Content.ReadAsStringAsync().Result;
            return JsonSerializer.Deserialize<T>(resultContent);
        }

        public async Task<(T? Data, IEnumerable<ApiError>? Errors)> Post<T>(string action, object value)
        {
            Logger.Debug($"Doing {WebRequestMethods.Http.Post} request to: {action}");
            var httpClient = GenerateHttpClient();
            var content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, ApplicationJson);

            var result = await httpClient.PostAsync(this.Url + action, content);
            if (result.IsSuccessStatusCode){
                var resultContent = await result.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<T>(resultContent);
                return (data, null);
            }
            var errorContent = await result.Content.ReadAsStringAsync();
            var errors = JsonSerializer.Deserialize<IEnumerable<ApiError>>(errorContent);
            return (default, errors);
        }

        public async Task<(T? Data, IEnumerable<ApiError>? Errors)> Put<T>(string action, object value)
        {
            Logger.Debug($"Doing {WebRequestMethods.Http.Put} request to: {action}");
            var httpClient = GenerateHttpClient();
            var content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, ApplicationJson);

            var result = await httpClient.PutAsync(this.Url + action, content);
            if (result.IsSuccessStatusCode){
                var resultContent = await result.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<T>(resultContent);
                return (data, null);
            }
            var errorContent = await result.Content.ReadAsStringAsync();
            var errors = JsonSerializer.Deserialize<IEnumerable<ApiError>>(errorContent);
            return (default, errors);
        }

        public void Delete(string action)
        {
            Logger.Debug($"Doing Delete request to: {action}");
            var httpClient = GenerateHttpClient();
            _ = httpClient.DeleteAsync(this.Url + action).Result;
        }
    }
}