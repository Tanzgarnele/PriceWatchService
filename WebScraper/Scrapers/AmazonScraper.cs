using HtmlAgilityPack;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebScraper.Interfaces;
using WebScraper.Models;

namespace WebScraper.Scrapers
{
    public class AmazonScraper : IScraper
    {
        private readonly HttpClient httpClient = new HttpClient();

        public async Task<ScapedProduct> ScrapeProductAsync(String url, CancellationToken cancellationToken)
        {
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(url, cancellationToken);
                response.EnsureSuccessStatusCode();
                String content = await response.Content.ReadAsStringAsync();

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(content);
                HtmlNode priceNode = doc.DocumentNode.SelectSingleNode("//span[@class='a-price']//span[@class='a-offscreen']");
                HtmlNode imgNode = doc.DocumentNode.SelectSingleNode("//img[@id='landingImage']");
                HtmlNode productNameNode = doc.DocumentNode.SelectSingleNode("//span[@id='productTitle']");

                String priceText = priceNode?.InnerText?.Replace("€", "").Trim() ?? "N/A";
                String imageUrl = imgNode?.GetAttributeValue("src", "N/A") ?? "N/A";
                String productName = productNameNode?.InnerText?.Trim() ?? "N/A";

                return new ScapedProduct
                {
                    PriceText = priceText,
                    ImageUrl = imageUrl,

                    Name = productName
                };
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request error: {ex.Message}");
                //TODO: Add Logging
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                //TODO: Add Logging
                return null;
            }
        }
    }
}