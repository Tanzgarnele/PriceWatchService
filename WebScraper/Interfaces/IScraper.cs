using System;
using System.Threading;
using System.Threading.Tasks;
using WebScraper.Models;

namespace WebScraper.Interfaces
{
    public interface IScraper
    {
        Task<ScapedProduct> ScrapeProductAsync(String url, CancellationToken cancellationToken);
    }
}