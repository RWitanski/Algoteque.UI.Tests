using Algoteque.UI.Tests.Pages;
using Algoteque.UI.Tests.Pages.Checkout;
using Algoteque.UI.Tests.Configuration;
using Algoteque.UI.Tests.Configuration.Models;

namespace Algoteque.UI.Tests
{
    public class Tests : IClassFixture<PlaywrightFixture>
    {
        private readonly PlaywrightFixture _fixture;
        private readonly ConfigurationDto _config;
        private readonly string _browserType;

        public Tests(PlaywrightFixture fixture)
        {
            _fixture = fixture;
            _config = ReadConfiguration.Instance.Config;
            _browserType = Environment.GetEnvironmentVariable("BROWSER") ?? "chromium";
        }

        [Theory]
        [InlineData("standard_user", true, "")]
        [InlineData("locked_out_user", false, "Epic sadface: Sorry, this user has been locked out.")]
        public async Task LoginTest(string username, bool shouldSucceed, string expectedError)
        {
            await using var browser = await _fixture.LaunchBrowserAsync(_browserType, _config.Headless);
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

        [Fact]
        public async Task AddProductToCartFinishCheckoutAndBackHomeTest()
        {
            await using var browser = await _fixture.LaunchBrowserAsync(_browserType, _config.Headless);
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