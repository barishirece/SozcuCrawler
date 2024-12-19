using HtmlAgilityPack;
using Nest;
using SozcuCrawl.Pages;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Crawl
{
    public class NewsItem
    {
        public string? Title { get; set; }
        public string? Link { get; set; }
    }
    public static class CrawlService
    {
        public static async Task<List<NewsItem>> GetNewsDataAsync()
        {
            List<NewsItem> newsData = new List<NewsItem>();

            try
            {
                HttpClient client = new HttpClient();
                string htmlContent = await client.GetStringAsync("https://www.sozcu.com.tr/");

                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(htmlContent);

                var titleNodes = htmlDoc.DocumentNode.SelectNodes("//a[contains(@class, 'news-card-footer')]");

                    foreach (var node in titleNodes)
                    {
                        string title = System.Net.WebUtility.HtmlDecode((node.InnerText.Trim()));
                        string href = node.GetAttributeValue("href", string.Empty);

                        if (!string.IsNullOrEmpty(href) && !href.StartsWith("http"))
                        {
                            string fullUrl = $"https://www.sozcu.com.tr{href}";

                            newsData.Add(new NewsItem
                            {
                                Title = title,
                                Link = fullUrl
                            });
                        }
                    }

                    Console.WriteLine("Haberler ayıklandı.");
                    await ElasticsearchService.IndexNewsAsync(newsData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }

            return newsData;
        }

    }
}
