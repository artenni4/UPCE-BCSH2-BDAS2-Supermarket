using Supermarket.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermarket.Infrastructure.Database
{
    public class DatabaseOptions : IConfigurationSection
    {
        public static string SectionName => "Database";

        public required string ConnectionString { get; init; }
    }
}
