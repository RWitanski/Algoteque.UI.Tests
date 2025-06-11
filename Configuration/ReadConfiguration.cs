using Algoteque.UI.Tests.Configuration.Models;
using Microsoft.Extensions.Configuration;

namespace Algoteque.UI.Tests.Configuration
{
    public sealed class ReadConfiguration
    {
        private static readonly Lazy<ReadConfiguration> _instance = new(() => new ReadConfiguration());
        public static ReadConfiguration Instance => _instance.Value;

        public ConfigurationDto Config { get; }

        private ReadConfiguration()
        {
            var cfgBuilder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: false)
                .Build();

            var dto = new ConfigurationDto
            {
                WebsiteUrl = string.Empty, // Default value to satisfy required property
                User = string.Empty,       // Default value to satisfy required property
                Pass = string.Empty,       // Default value to satisfy required property
                Headless = false           // Default value to satisfy required property
            };
            cfgBuilder.GetSection("TestsConfiguration").Bind(dto); // Bind data from section

            Config = dto;
        }
    }
}
