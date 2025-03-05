using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyApp.Logging;
using MyApp.MEP.Services;
using MyApp.Shared.Services;
using System;

namespace MyApp.MEP.ExternalCommands;

[Transaction(TransactionMode.Manual)]
public sealed class SampleOneExternalCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        try
        {
            var service = new SimpleDimpleService();

            AppLogger.Info("Run command");

            service.SayHello();

            return Result.Succeeded;

        }
        catch (Exception exception)
        {
            AppLogger.Error(exception);

            return Result.Failed;
        }
    }
}
