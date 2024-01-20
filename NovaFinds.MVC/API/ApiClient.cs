namespace NovaFinds.MVC.API
{
    using IFR.Logger;
    using System.Net.Http.Headers;
    using System.Text.Json;

    public class ApiClient(IConfiguration config)
    {
        private string? Url { get; set; } = config.GetSection("Config").GetSection("Apis").GetSection("NovaFinds").Value;

        private string? ApiKey { get; set; } = config.GetSection("Config").GetSection("ApiKeys").GetSection("NovaFinds").Value;

        private HttpClient GenerateHttpClient()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("X-Api-Key", this.ApiKey);
            return httpClient;
        }

        public async Task<T?> Get<T>(string action)
        {
            Logger.Debug($"Doing request to: {action}");
            var httpClient = GenerateHttpClient();
            var result = await httpClient.GetAsync(this.Url + action);
            result.EnsureSuccessStatusCode();
            var resultContent = result.Content.ReadAsStringAsync().Result;
            return JsonSerializer.Deserialize<T>(resultContent);
        }

    }
}