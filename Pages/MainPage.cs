using Microsoft.Playwright;

namespace Algoteque.UI.Tests.Pages
{
    public class MainPage
    {
        private readonly IPage _page;

        public MainPage(IPage page)
        {
            _page = page;
        }

        public ILocator ProductsContainer => _page.Locator(".inventory_list");
        public ILocator MenuButton => _page.Locator("#react-burger-menu-btn");
        public ILocator CartIcon => _page.Locator(".shopping_cart_link");
        public ILocator AddToCartButtons => _page.Locator(".inventory_item button");

        public async Task ValidateLoaded(string websiteUrl)
        {
            await _page.WaitForURLAsync("**/inventory.html");
            Assert.Equal($"{websiteUrl}/inventory.html", _page.Url);
            Assert.True(await ProductsContainer.IsVisibleAsync());
            Assert.True(await MenuButton.IsVisibleAsync());
        }

        public async Task AddProductToCartAsync(int productIndex = 0)
        {
            await AddToCartButtons.Nth(productIndex).ClickAsync();
        }

        public async Task GoToCartAsync()
        {
            await CartIcon.ClickAsync();
        }
    }
}