using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configuration
{
    public record DatabaseSettings
    {
        public string ConnectionString { get; init; } = string.Empty;
    }


}
