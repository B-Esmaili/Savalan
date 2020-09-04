using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using McMaster.NETCore.Plugins;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

namespace Savalan.Web.Mvc
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Savalan web services using default configurations.
        /// </summary>
        /// <param name="services">The  <see ref="IServiceCollection">  to add services to</param>
        /// <returns>An <see cref="IServiceCollection"/> that can be used to further configure the MVC services</returns>
        public static IServiceCollection AddSavalan(this IServiceCollection services)
        {
            return AddSavalan(services, _mvcSetupAction: null);
        }

        /// <summary>
        /// Adds Savalan web services using custom <see ref="MvcOptions"/>.
        /// </summary>
        /// <param name="services">The  <see ref="IServiceCollection">  to add services to</param>
        /// <param name="mvcSetupAction">An <see cref="Action{MvcOptions}"/> to configure the provided <see cref="MvcOptions"/>.</param> 
        /// <returns>An <see cref="IServiceCollection"/> that can be used to further configure the MVC services</returns>
        public static IServiceCollection AddSavalan(this IServiceCollection services, Action<MvcOptions> mvcSetupAction)
        {
            return AddSavalan(services, _mvcSetupAction: mvcSetupAction);
        }

        /// <summary>
        /// Adds Savalan web services using custom <see ref="MvcOptions"/> and <see ref="MvcOptions"/>.
        /// </summary>
        /// <param name="services">The  <see ref="IServiceCollection">  to add services to</param>
        /// <param name="mvcSetupAction">An <see cref="Action{MvcOptions}"/> to configure the provided <see cref="MvcOptions"/>.</param> 
        /// <param name="savalanWebSetupAction">An <see cref="Action{SavalanWebOptions}"/> to configure the provided <see cref="SavalanWebOptions"/>.</param>
        /// <returns>An <see cref="IServiceCollection"/> that can be used to further configure the MVC services</returns>
        public static IServiceCollection AddSavalan(
            this IServiceCollection services,
            Action<MvcOptions> mvcSetupAction,
            Action<SavalanWebOptions> savalnWebSetup)
        {
            return AddSavalan(services, _mvcSetupAction: mvcSetupAction, savalanSetupOptions: savalnWebSetup);
        }

        /// <summary>
        /// Adds Savalan web services with predefined set of plugin assemblies using default configurations.
        /// </summary>
        /// <param name="services">The  <see ref="IServiceCollection">  to add services to</param>
        /// <param name="pluginAssemblyPaths">List of fully qualified plugin assembly paths</param>
        /// <returns>An <see cref="IServiceCollection"/> that can be used to further configure the MVC services</returns>
        public static IServiceCollection AddSavalanWithPlugins(
            this IServiceCollection services,
            IEnumerable<string> pluginAssemblyPaths)
        {
            return AddSavalanWithPlugins(services, pluginAssemblyPaths, null);
        }

        /// <summary>
        /// Adds Savalan web services with predefined set of plugin assemblies using custom <see ref="MvcOptions" />.
        /// </summary>
        /// <param name="services">The  <see ref="IServiceCollection">  to add services to</param>
        /// <param name="pluginAssemblyPaths">List of fully qualified plugin assembly paths</param>
        /// <param name="mvcSetupAction">An <see cref="Action{MvcOptions}"/> to configure the provided <see cref="MvcOptions"/>.</param>
        /// <returns>An <see cref="IServiceCollection"/> that can be used to further configure the MVC services</returns>
        public static IServiceCollection AddSavalanWithPlugins(
            this IServiceCollection services,
            IEnumerable<string> pluginAssemblyPaths,
            Action<MvcOptions> mvcSetupAction)
        {
            return AddSavalan(services, pluginAssemblyPaths: pluginAssemblyPaths, _mvcSetupAction: mvcSetupAction);
        }

        /// <summary>
        /// Adds Savalan web services with predefined set of plugin assemblies using custom <see ref="MvcOptions" />.
        /// </summary>
        /// <param name="services">The  <see ref="IServiceCollection">  to add services to</param>
        /// <param name="pluginAssemblyPaths">List of fully qualified plugin assembly paths</param>
        /// <param name="mvcSetupAction">An <see cref="Action{MvcOptions}"/> to configure the provided <see cref="MvcOptions"/>.</param>
        /// <param name="savalanWebSetupAction">An <see cref="Action{SavalanWebOptions}"/> to configure the provided <see cref="SavalanWebOptions"/>.</param>
        /// <returns>An <see cref="IServiceCollection"/> that can be used to further configure the MVC services</returns>
        public static IServiceCollection AddSavalanWithPlugins(
            this IServiceCollection services,
            IEnumerable<string> pluginAssemblyPaths,
            Action<MvcOptions> mvcSetupAction,
            Action<SavalanWebOptions> savalanWebSetupAction)
        {
            return AddSavalan(
                services,
                pluginAssemblyPaths: pluginAssemblyPaths,
                _mvcSetupAction: mvcSetupAction,
                savalanSetupOptions: savalanWebSetupAction);
        }

        private static IServiceCollection AddSavalan(
            this IServiceCollection services,
            Action<MvcOptions> _mvcSetupAction = null,
            Action<SavalanWebOptions> savalanSetupOptions = null,
            IEnumerable<string> pluginAssemblyPaths = null)
        {

            var mvcBuilder = services.AddMvcCore();

            ConfigureApplicationPartManager(mvcBuilder, pluginAssemblyPaths.Select(LoadPluginFromPath));

            if (_mvcSetupAction != null)
            {
                services.Configure<MvcOptions>(_mvcSetupAction);
            }
            return services;
        }

        private static Assembly LoadPluginFromPath(string assemblyPath)
        {
             var plugin = PluginLoader.CreateFromAssemblyFile(
                assemblyPath,  
                config =>
                    // this ensures that the version of MVC is shared between this app and the plugin
                    config.PreferSharedTypes = true);
        }

        private static void ConfigureApplicationPartManager(IMvcCoreBuilder mvcBuilder, IEnumerable<Assembly> assemblies)
        {
            mvcBuilder.ConfigureApplicationPartManager((apm) =>
            {
                assemblies.ToList().ForEach(asm => apm.ApplicationParts.Add(new AssemblyPart(asm)));
            });
        }
    }
}