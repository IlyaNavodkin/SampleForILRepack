using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyApp.Shared.Services;
using System;

namespace MyApp.MEP.ExternalCommands;

[Transaction(TransactionMode.Manual)]
public sealed class SampleOneExternalCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        //var service = new SimpleDimpleService();

        var externalCommandLogger = SampleServiceProvider.ServiceProvider
            .GetRequiredService<ILogger<SampleOneExternalCommand>>();

        externalCommandLogger.LogInformation("Run command");

        try
        {
            //service.SayHello();

            TaskDialog.Show("Message", "Hello");

            return Result.Succeeded;

        }
        catch (Exception exception)
        {
            externalCommandLogger.LogError(exception, null);

            return Result.Failed;
        }
    }
}
