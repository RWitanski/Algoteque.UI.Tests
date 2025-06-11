using System.Threading.Tasks;
using Microsoft.Playwright;
using Xunit;

namespace Algoteque.UI.Tests.Pages.Checkout
{
    public class CheckoutStepOnePage
    {
        private readonly IPage _page;

        public CheckoutStepOnePage(IPage page)
        {
            _page = page;
        }

        public ILocator FirstNameInput => _page.Locator("[data-test='firstName']");
        public ILocator LastNameInput => _page.Locator("[data-test='lastName']");
        public ILocator PostalCodeInput => _page.Locator("[data-test='postalCode']");
        public ILocator ContinueButton => _page.Locator("[data-test='continue']");

        public async Task ValidateLoaded()
        {
            await _page.WaitForURLAsync("**/checkout-step-one.html");
            Assert.Contains("/checkout-step-one.html", _page.Url);
            Assert.True(await FirstNameInput.IsVisibleAsync());
            Assert.True(await LastNameInput.IsVisibleAsync());
            Assert.True(await PostalCodeInput.IsVisibleAsync());
        }

        public async Task FillCheckoutInfo(string firstName, string lastName, string postalCode)
        {
            await FirstNameInput.FillAsync(firstName);
            await LastNameInput.FillAsync(lastName);
            await PostalCodeInput.FillAsync(postalCode);
        }

        public async Task ContinueAsync()
        {
            await ContinueButton.ClickAsync();
        }
    }
}
