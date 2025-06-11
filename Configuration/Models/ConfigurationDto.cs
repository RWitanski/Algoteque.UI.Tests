namespace Algoteque.UI.Tests.Configuration.Models
{
    public class ConfigurationDto
    {
        public required string WebsiteUrl { get; set; }
        public required string User { get; set; }
        public required string Pass { get; set; }
        public required bool Headless { get; set; }
    }
}
