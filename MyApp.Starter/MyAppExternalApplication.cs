using Autodesk.Revit.UI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyApp.Shared.Services;
using MyApp.Starter.Helpers;
using Serilog;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace MyApp.Starter;

public class MyAppExternalApplication : IExternalApplication
{
    public Result OnShutdown(UIControlledApplication application)
    {
        Log.CloseAndFlush();

        return Result.Succeeded;
    }

    private Assembly LoadEmbeddedAssembly(object sender, ResolveEventArgs args)
    {
        string resourceName = $"{Assembly.GetExecutingAssembly().GetName().Name}.Costura.{new AssemblyName(args.Name).Name}.dll";
        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
        {
            if (stream == null)
                return null;

            byte[] assemblyData = new byte[stream.Length];
            stream.Read(assemblyData, 0, assemblyData.Length);
            return Assembly.Load(assemblyData);
        }
    }

    public Result OnStartup(UIControlledApplication uIControlledApplication)
    {
        try
        {
            AppDomain.CurrentDomain.AssemblyResolve += LoadEmbeddedAssembly;

            var configurator = RevitUiConfigurator.GetInstance();
            configurator.ConfigureRevitUiComponents(uIControlledApplication);

            var serilogAssemblies = AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(a => a.FullName.Contains("Serilog"))
            .Select(a => a.FullName)
            .ToList();

            LoggerInitHelper.Init();
            SampleServiceProvider.SetUp(uIControlledApplication);

            var logger = SampleServiceProvider
                .ServiceProvider.GetRequiredService<ILogger<MyAppExternalApplication>>();

            logger.LogInformation($"Load services GUID {SampleServiceProvider.Guid}");
            logger.LogInformation($"Domain NAME {AppDomain.CurrentDomain.FriendlyName}");
            logger.LogInformation("Startup complete");

            return Result.Succeeded;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Startup failed");

            Autodesk.Revit.UI.TaskDialog.Show("Ribbon Sample", ex.ToString());

            return Result.Failed;
        }
    }
}
