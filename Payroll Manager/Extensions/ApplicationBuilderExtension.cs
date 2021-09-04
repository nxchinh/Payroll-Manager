using Microsoft.AspNetCore.Builder;
using Payroll_Manager.Infrastructure;

namespace Payroll_Manager.Extensions
{
    public static class ApplicationBuilderExtension
    {
        public static void UseExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
