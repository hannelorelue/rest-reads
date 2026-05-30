using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace RestReads.Client.Services;

public class ApiClient
{
    private readonly HttpClient _http;

    public ApiClient(HttpClient http)
    {
        _http = http;
        _http.BaseAddress = new Uri("https://localhost:7000/api/");
    }

    public void SetToken(string token)
    {
        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }

    public Task<T?> GetAsync<T>(string url) =>
        _http.GetFromJsonAsync<T>(url);

    public Task<HttpResponseMessage> PostAsync<T>(string url, T body) =>
        _http.PostAsJsonAsync(url, body);

    public Task<HttpResponseMessage> PutAsync<T>(string url, T body) =>
        _http.PutAsJsonAsync(url, body);

    public Task<HttpResponseMessage> PatchAsync<T>(string url, T body) =>
        _http.PatchAsJsonAsync(url, body);

    public Task<HttpResponseMessage> DeleteAsync(string url) =>
        _http.DeleteAsync(url);
}
