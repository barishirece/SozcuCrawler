using Crawl;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SozcuCrawl.Pages;
using System.Collections.Generic;
using System.Threading.Tasks;

    public class IndexModel : PageModel
    {
    private readonly ElasticsearchService _elasticsearchService;
    public List<NewsItem>? NewsItems { get; set; }
    public string? SearchTerm { get; set; }

    public IndexModel(ElasticsearchService elasticsearchService)
    {
        _elasticsearchService = elasticsearchService;
    }

    public async Task OnGetAsync(string searchTerm)
    {
        NewsItems = await ElasticsearchService.SearchNewsAsync(searchTerm);
    }
}

