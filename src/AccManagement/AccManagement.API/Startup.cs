using System.Reflection;
using AccManagement.API.Common.DapperTypeHandlers;
using AccManagement.API.Data;
using AccManagement.API.Transactions;
using Dapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AccManagement.API {
    public class Startup {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration) {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services) {
            SqlMapper.AddTypeHandler(new GuidTypeHandler());
            services.AddSqliteDatabase(_configuration.GetConnectionString("TransactionsDb"));
            
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            
            services.AddSingleton(Log.Logger);
            
            services.AddHealthChecks();
            
            services.AddMemoryCache();
            services.AddResponseCaching();

            services.AddControllers()
                .AddFluentValidation(config =>
                    config.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            app.ApplicationServices
                .GetRequiredService<ISqlDatabase>()
                .EnsureDbIsUpgraded();

            app.UseResponseCaching();

            app.UseSerilogRequestLogging();
            
            app.UseRouting();

            app.UseEndpoints(endpoints => {
                endpoints.MapHealthChecks("/ping");
                endpoints.MapControllers();
            });
        }
    }
}