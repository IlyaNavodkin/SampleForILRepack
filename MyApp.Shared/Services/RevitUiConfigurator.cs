using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Windows;
using Microsoft.Extensions.Logging;
using MyApp.Logging;
using MyApp.RevitUi.Services;
using System.Reflection;

namespace MyApp.Shared.Services;

public partial class RevitUiConfigurator
{
    private readonly UiComponentFactory _uiComponentCreateUtil;
    private readonly UIControlledApplication _uIControlledApplication;

    public RevitUiConfigurator(UIControlledApplication uIControlledApplication,
        UiComponentFactory uiComponentCreateUtil)
    {
        _uiComponentCreateUtil = uiComponentCreateUtil;
        _uIControlledApplication = uIControlledApplication;
    }

    public RibbonTab ConfigureRevitUiComponents()
    {
        AppLogger.Info("Делаю кнопочки");

        var bimdataTab = _uiComponentCreateUtil.CreateRibbonTab(_uIControlledApplication, "MyApp.MEP");

        var uploadPanel = _uiComponentCreateUtil.CreateRibbonPanel(_uIControlledApplication, bimdataTab,
            "qwer",
            "Test panel for MyApp.MEP");

        var assemblyPath = Assembly.GetExecutingAssembly().Location;

        var uploadModelButton = _uiComponentCreateUtil.CreatePushButton(
            "Запустить тестовую команду",
            "MyApp.MEP.ExternalCommands.SampleOneExternalCommand",
            assemblyPath,
            null,
            null,
            "Стартуем"
        );


        uploadPanel.AddItem(uploadModelButton);

        return bimdataTab;
    }
}
