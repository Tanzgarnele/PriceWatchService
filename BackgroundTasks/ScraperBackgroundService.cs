using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebScraper.Interfaces;

namespace BackgroundTasks
{
    public class ScraperBackgroundService : BackgroundService
    {
        private readonly IScraper scraper;

        public ScraperBackgroundService(IScraper scraper)
        {
            this.scraper = scraper ?? throw new ArgumentNullException(nameof(scraper));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromMinutes(20), stoppingToken);

                await this.scraper.ScrapeProductAsync("", stoppingToken);
            }
        }
    }
}