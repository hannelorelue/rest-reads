using System.Text.Json;
using System.Text.Json.Serialization;
using RestReads.Core.DTOs.Books;
using RestReads.Core.Interfaces;

namespace RestReads.Api.Services;

public class OpenLibraryService(HttpClient http) : IOpenLibraryService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    public async Task<IEnumerable<OpenLibraryBookDto>> SearchAsync(string query)
    {
        var url = $"search.json?q={Uri.EscapeDataString(query)}&fields=key,title,author_name,isbn,number_of_pages_median,cover_i&limit=20";
        var response = await http.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<SearchResponse>(content, JsonOptions);

        return result?.Docs.Select(ToDto) ?? [];
    }

    private static OpenLibraryBookDto ToDto(SearchDoc doc) => new()
    {
        OpenLibraryKey = doc.Key,
        Title = doc.Title,
        Author = doc.AuthorName?.FirstOrDefault() ?? "Unknown",
        Isbn = doc.Isbn?.FirstOrDefault(),
        TotalPages = doc.NumberOfPagesMedian,
        CoverUrl = doc.CoverId is not null
            ? $"https://covers.openlibrary.org/b/id/{doc.CoverId}-M.jpg"
            : null
    };

    // Internal types for deserializing the Open Library response
    private class SearchResponse
    {
        public List<SearchDoc> Docs { get; set; } = [];
    }

    private class SearchDoc
    {
        public string Key { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public List<string>? AuthorName { get; set; }
        public List<string>? Isbn { get; set; }
        public int? NumberOfPagesMedian { get; set; }

        // Field is "cover_i" not "cover_id", so explicit name needed
        [JsonPropertyName("cover_i")]
        public int? CoverId { get; set; }
    }
}
