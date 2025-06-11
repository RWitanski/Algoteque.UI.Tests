using Microsoft.Playwright;

namespace Algoteque.UI.Tests.Pages.Checkout
{
    public class CheckoutCompletePage
    {
        private readonly IPage _page;

        public CheckoutCompletePage(IPage page)
        {
            _page = page;
        }

        public ILocator CompleteHeader => _page.Locator(".complete-header");
        public ILocator CompleteText => _page.Locator(".complete-text");
        public ILocator BackHomeButton => _page.Locator("[data-test='back-to-products']");

        public async Task ValidateLoaded()
        {
            await _page.WaitForURLAsync("**/checkout-complete.html");
            Assert.Contains("/checkout-complete.html", _page.Url);
        }

        public async Task ValidateOrderComplete()
        {
            Assert.True(await CompleteHeader.IsVisibleAsync());
            Assert.Equal("Thank you for your order!", await CompleteHeader.InnerTextAsync());
            Assert.True(await CompleteText.IsVisibleAsync());
            Assert.Contains("Your order has been dispatched", await CompleteText.InnerTextAsync());
        }

        public async Task BackToProductsAsync()
        {
            await BackHomeButton.ClickAsync();
        }
    }
}
