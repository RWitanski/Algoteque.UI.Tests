using Microsoft.Playwright;

namespace Algoteque.UI.Tests.Pages
{
    public class LoginPage
    {
        private readonly IPage _page;

        public LoginPage(IPage page)
        {
            _page = page;
        }

        public ILocator UserNameInput => _page.Locator("#user-name");

        public ILocator UserNamePass => _page.Locator("#password");

        public ILocator LoginButton => _page.Locator("#login-button");

        public async Task Login(string username)
        {
            await UserNameInput.FillAsync(username);
            await UserNamePass.FillAsync("secret_sauce");
            await LoginButton.ClickAsync();
        }

        public async Task ValidateLoginError(string expectedMessage)
        {
            var errorContainer = _page.Locator(".error-message-container");
            Assert.True(await errorContainer.IsVisibleAsync(), "Error message container should be visible.");
            var actualMessage = await errorContainer.InnerTextAsync();
            Assert.Contains(expectedMessage, actualMessage);
        }
    }
}
