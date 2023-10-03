using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Keyavi.Logs.LogAnalyticsWorkspace.Function
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddAppsettingsFile(
            this IConfigurationBuilder configurationBuilder,
            FunctionsHostBuilderContext context,
            bool useEnvironment = false
        )
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var environmentSection = string.Empty;

            if (useEnvironment)
            {
                environmentSection = $".{context.EnvironmentName}";
            }

            configurationBuilder.AddJsonFile(
                path: Path.Combine(context.ApplicationRootPath, $"appsettings{environmentSection}.json"),
                optional: true,
                reloadOnChange: false);

            return configurationBuilder;
        }
    }

}
