using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Payroll_Manager.Persistence.Repository;
using Payroll_Manager.Persistence.Repository.Abstraction;
using Payroll_Manager.StartupTasks;
using Payroll_Manager.Storage;

namespace Payroll_Manager.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStartupTask<T>(this IServiceCollection services)
            where T : class, IStartupTask
            => services.AddTransient<IStartupTask, T>();

        public static void AddDatabaseAccess(this IServiceCollection services)
        {
            services.AddScoped<IDataAccess, DataAccess>();
        }

        public static void AddFileStorage(this IServiceCollection services, string storage)
        {
            services.AddScoped(provider =>
            {
                IWebHostEnvironment env = provider.GetRequiredService<IWebHostEnvironment>();
                return new FileStorage(Path.Combine(env.ContentRootPath, storage));
            });
        }
    }
}
