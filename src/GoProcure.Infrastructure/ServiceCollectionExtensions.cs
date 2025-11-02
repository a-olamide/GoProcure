using GoProcure.Application.Abstraction.Policies;
using GoProcure.Application.Abstractions.Persistence;
using GoProcure.Application.Abstractions.ReadModels;
using GoProcure.Application.Abstractions.Repositories;
using GoProcure.Infrastructure.Persistence;
using GoProcure.Infrastructure.Policies;
using GoProcure.Infrastructure.ReadModels;
using GoProcure.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration cfg)
        {
            services.AddDbContext<Persistence.AppDbContext>(opt =>
                opt.UseSqlServer(
                    cfg.GetConnectionString("Default"),
                    sql => sql.MigrationsAssembly(typeof(Persistence.AppDbContext).Assembly.FullName)));

            /// UoW
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Repositories
            services.AddScoped<IPurchaseRequestRepository, PurchaseRequestRepository>();

            // Policies (stubs for now)
            services.AddScoped<IBudgetPolicy, BudgetPolicy>();
            services.AddScoped<IApprovalRoutingService, ApprovalRoutingService>();
            // services.AddScoped<IVendorPolicy, VendorPolicy>(); // when you need it

            services.AddScoped<IPurchaseRequestReadModel, PurchaseRequestReadModel>();

            return services;
        }
    }
}
