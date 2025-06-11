using System.Threading.Tasks;
using Microsoft.Playwright;
using Xunit;

namespace Algoteque.UI.Tests.Pages.Checkout
{
    public class CheckoutStepTwoPage
    {
        private readonly IPage _page;

        public CheckoutStepTwoPage(IPage page)
        {
            _page = page;
        }

        public ILocator FinishButton => _page.Locator("[data-test='finish']");
        public ILocator CartItems => _page.Locator(".cart_item");
        public ILocator ProductNameLocator => _page.Locator(".inventory_item_name");
        public ILocator SummarySubtotal => _page.Locator(".summary_subtotal_label");
        public ILocator SummaryTax => _page.Locator(".summary_tax_label");
        public ILocator SummaryTotal => _page.Locator(".summary_total_label");

        public async Task ValidateLoaded()
        {
            await _page.WaitForURLAsync("**/checkout-step-two.html");
            Assert.Contains("/checkout-step-two.html", _page.Url);
        }
        public async Task ValidateOrderSummaryAsync(string? expectedProductName = null, decimal? expectedItemPrice = null)
        {
            Assert.True(await CartItems.CountAsync() > 0, "There should be at least one product in the summary.");

            if (!string.IsNullOrEmpty(expectedProductName))
            {
                var productNames = await ProductNameLocator.AllInnerTextsAsync();
                Assert.Contains(expectedProductName, productNames);
            }

            // Read all product prices from the cart
            var priceLocators = _page.Locator(".inventory_item_price");
            int priceCount = await priceLocators.CountAsync();
            decimal sumOfProductPrices = 0m;
            for (int i = 0; i < priceCount; i++)
            {
                var priceText = await priceLocators.Nth(i).InnerTextAsync();
                decimal price = decimal.Parse(priceText.Replace("$", ""));
                sumOfProductPrices += price;
            }

            Assert.True(await SummarySubtotal.IsVisibleAsync(), "Subtotal should be visible.");
            Assert.True(await SummaryTax.IsVisibleAsync(), "Tax should be visible.");
            Assert.True(await SummaryTotal.IsVisibleAsync(), "Total should be visible.");

            var subtotalText = await SummarySubtotal.InnerTextAsync();
            var taxText = await SummaryTax.InnerTextAsync();
            var totalText = await SummaryTotal.InnerTextAsync();

            decimal subtotal = decimal.Parse(subtotalText.Split('$')[1]);
            decimal tax = decimal.Parse(taxText.Split('$')[1]);
            decimal total = decimal.Parse(totalText.Split('$')[1]);

            Assert.Equal(sumOfProductPrices, subtotal);

            if (expectedItemPrice.HasValue)
            {
                Assert.Equal(expectedItemPrice.Value, subtotal);
            }

            Assert.Equal(subtotal + tax, total);
            Assert.True(tax > 0, "Tax should be greater than zero.");
        }

        public async Task FinishAsync()
        {
            await FinishButton.ClickAsync();
        }
    }
}
