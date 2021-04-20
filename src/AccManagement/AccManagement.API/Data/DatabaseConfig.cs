using System;
using System.Data.Common;

namespace AccManagement.API.Data {
    public class DatabaseConfig {
        public string ConnectionString { get; }
        public DbProviderFactory ProviderFactory { get; }
        
        public DatabaseConfig(string connectionString, DbProviderFactory providerFactory) {
            if (string.IsNullOrWhiteSpace(connectionString)) {
                throw new ArgumentException($"{nameof(connectionString)} can not be null or whitespace");
            }

            if (providerFactory is null) {
                throw new ArgumentNullException(nameof(providerFactory));
            }
            
            ConnectionString = connectionString;
            ProviderFactory = providerFactory;
        }
    }
}