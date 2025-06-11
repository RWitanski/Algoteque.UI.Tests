using System.Threading.Tasks;
using Algoteque.UI.Tests.Pages;
using Algoteque.UI.Tests.Pages.Checkout;
using Algoteque.UI.Tests.Configuration;
using Microsoft.Playwright;
using Xunit;
using Algoteque.UI.Tests.Configuration.Models;

namespace Algoteque.UI.Tests
{
    public class Tests : IClassFixture<PlaywrightFixture>
    {
        private readonly PlaywrightFixture _fixture;
        private readonly ConfigurationDto _config;

        public Tests(PlaywrightFixture fixture)
        {
            _fixture = fixture;
            _config = ReadConfiguration.Instance.Config;
        }

        [Theory]
        [InlineData("chromium", "standard_user", true, "")]
        [InlineData("firefox", "standard_user", true, "")]
        [InlineData("chromium", "locked_out_user", false, "Epic sadface: Sorry, this user has been locked out.")]
        [InlineData("firefox", "locked_out_user", false, "Epic sadface: Sorry, this user has been locked out.")]
        public async Task LoginTest(string browserType, string username, bool shouldSucceed, string expectedError)
        {
            await using var browser = await _fixture.LaunchBrowserAsync(browserType, _config.Headless);
            await using var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();
            await page.GotoAsync(_config.WebsiteUrl);

            var loginPage = new LoginPage(page);
            await loginPage.Login(username);

            if (shouldSucceed)
            {
                var mainPage = new MainPage(page);
                await mainPage.ValidateLoaded(_config.WebsiteUrl);
            }
            else
            {
                await loginPage.ValidateLoginError(expectedError);
            }
        }

        [Theory]
        [InlineData("chromium")]
        [InlineData("firefox")]
        public async Task AddProductToCartFinishCheckoutAndBackHomeTest(string browserType)
        {
            await using var browser = await _fixture.LaunchBrowserAsync(browserType, _config.Headless);
            await using var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();
            await page.GotoAsync(_config.WebsiteUrl);

            var loginPage = new LoginPage(page);
            await loginPage.Login(_config.User);

            var mainPage = new MainPage(page);
            await mainPage.ValidateLoaded(_config.WebsiteUrl);

            await mainPage.AddProductToCartAsync();
            await mainPage.GoToCartAsync();

            var cartPage = new CartPage(page);
            await cartPage.ValidateLoaded();
            await cartPage.StartCheckoutAsync();

            var stepOne = new CheckoutStepOnePage(page);
            await stepOne.ValidateLoaded();
            await stepOne.FillCheckoutInfo("John", "Doe", "12345");
            await stepOne.ContinueAsync();

            var stepTwo = new CheckoutStepTwoPage(page);
            await stepTwo.ValidateLoaded();
            await stepTwo.ValidateOrderSummaryAsync("Sauce Labs Backpack");
            await stepTwo.FinishAsync();

            var completePage = new CheckoutCompletePage(page);
            await completePage.ValidateLoaded();
            await completePage.ValidateOrderComplete();
            await completePage.BackToProductsAsync();

            await mainPage.ValidateLoaded(_config.WebsiteUrl);
        }
    }
}
