using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Savalan.Web.Mvc.Builder
{
    public static class Extensions
    {
        public static IApplicationBuilder UseSavalan(this IApplicationBuilder builder){

            builder.UseRoutes();
            return builder;
        }

        public static IApplicationBuilder UseRoutes(this IApplicationBuilder builder)
        {
            builder.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            return builder;
        }
    }
}