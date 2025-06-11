using System.Threading.Tasks;
using Microsoft.Playwright;
using Xunit;

namespace Algoteque.UI.Tests.Pages
{
    public class CartPage
    {
        private readonly IPage _page;

        public CartPage(IPage page)
        {
            _page = page;
        }

        public ILocator CheckoutButton => _page.Locator("[data-test='checkout']");

        public async Task ValidateLoaded()
        {
            await _page.WaitForURLAsync("**/cart.html");
            Assert.Contains("/cart.html", _page.Url);
        }

        public async Task StartCheckoutAsync()
        {
            await CheckoutButton.ClickAsync();
            await _page.WaitForURLAsync("**/checkout-step-one.html");
            Assert.Contains("/checkout-step-one.html", _page.Url);
        }
    }
}
