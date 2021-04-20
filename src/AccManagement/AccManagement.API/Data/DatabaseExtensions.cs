using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;

namespace AccManagement.API.Data {
    public static class DatabaseExtensions {
        public static IServiceCollection AddSqliteDatabase(this IServiceCollection services, string connectionString) {
            services.AddSingleton(new DatabaseConfig(connectionString, SqliteFactory.Instance));
            services.AddSingleton<ISqlDatabase, SqlDatabase>();
            return services;
        }
    }
}