using Supermarket.Core.Common;

namespace Supermarket.Infrastructure.Database
{
    public class DatabaseOptions : IConfigurationSection
    {
        public static string SectionName => "Database";

        public required string ConnectionString { get; init; }
    }
}
