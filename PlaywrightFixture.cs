using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Algoteque.UI.Tests
{
    public class PlaywrightFixture : IAsyncLifetime
    {
        public required IPlaywright Playwright { get; set; }

        public async Task InitializeAsync()
        {
            Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        }

        public Task DisposeAsync()
        {
            Playwright?.Dispose();
            return Task.CompletedTask;
        }

        public async Task<IBrowser> LaunchBrowserAsync(string browserType, bool isHeadless)
        {
            return browserType switch
            {
                "chromium" => await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = isHeadless }),
                "firefox" => await Playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions { Headless = isHeadless }),
                "webkit" => await Playwright.Webkit.LaunchAsync(new BrowserTypeLaunchOptions { Headless = isHeadless }),
                _ => throw new System.ArgumentException($"Unknown browser type: {browserType}")
            };
        }
    }
}
