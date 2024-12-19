using Crawl;
using Nest;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SozcuCrawl.Pages
{
    public class ElasticsearchService
    {
        private static ElasticClient _client;

        static ElasticsearchService()
        {
            var settings = new ConnectionSettings(new Uri("http://localhost:9200")).DefaultIndex("news");
            _client = new ElasticClient(settings);
        }

        // Veritabanına yazma
        public static async Task IndexNewsAsync(List<NewsItem> newsData)
        {

            var response = await _client.Indices.DeleteAsync("news"); // "news" index'ini sil

            if (response.IsValid)
                Console.WriteLine("Veritabanı başarıyla temizlendi.");
            else
                Console.WriteLine("Veritabanı temizlenirken hata oluştu: " + response.OriginalException?.Message);            

            foreach (var news in newsData)
                await _client.IndexDocumentAsync(news);
            
            Console.WriteLine("Haberler veritabanına eklendi.");
        }

        // Elasticsearch'ten sorgulama
        public static async Task<List<NewsItem>> SearchNewsAsync(string query)
        {
            List<NewsItem> newsData = new List<NewsItem>();

            var response = await _client.SearchAsync<NewsItem>(s => s
                .Size(64)
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.Title)
                        .Query(query)
                    )
                )
            );

            foreach (var hit in response.Hits)
            {
                newsData.Add(hit.Source);
            }
            return newsData;
        }
    }
}
