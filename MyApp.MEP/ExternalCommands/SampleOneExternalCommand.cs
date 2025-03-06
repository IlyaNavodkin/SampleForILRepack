using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyApp.Logging;
using MyApp.MEP.Services;
using MyApp.Shared.Services;
using System;
using System.Web.UI.WebControls;
using System.Windows.Media.Media3D;
using CommunityToolkit.Mvvm.DependencyInjection;
using System.Windows;

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
            var revitTask = new RevitTask();

            var uiApp = commandData.Application;
            var uiDoc = uiApp.ActiveUIDocument;
            var doc = uiDoc.Document;

            var httpService = new HttpRestService();
            var handler = new ExternalEventHandler(httpService);

            var windowStateService = new WindowStateService();
            var viewModel = new PickPointViewModel(doc, uiDoc, httpService, handler, windowStateService);
            var window = new TestWindow(viewModel);

            windowStateService.SetActiveWindow(window);

            window.Show();


            return Result.Succeeded;

        }
        catch (Exception exception)
        {
            AppLogger.Error(exception);

            return Result.Failed;
        }
    }
}


public class WindowStateService
{
    private Window window;

    public void SetActiveWindow(Window window)
    {
        this.window = window;
    }

    public void ShowWindow()
    {
        window.Topmost = true;
        window.Show();
    }

    public void HideWindow()
    {
        window.Hide();
    }

    public void CloseWindow()
    {
        window.Close();
    }
}