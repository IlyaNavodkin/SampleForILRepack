using Autodesk.Revit.UI;
using Autodesk.Windows;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace MyApp.Shared.Services;

public partial class RevitUiConfigurator
{
    private readonly UiComponentFactory _uiComponentCreateUtil;
    private readonly ILogger _logger;

    public RevitUiConfigurator(UiComponentFactory uiComponentCreateUtil, ILogger logger)
    {
        _uiComponentCreateUtil = uiComponentCreateUtil;
        _logger = logger;
    }

    public RibbonTab ConfigureRevitUiComponents(UIControlledApplication uiControlledApplication)
    {
        _logger.LogInformation("Делаю кнопочки");

        var bimdataTab = _uiComponentCreateUtil.CreateRibbonTab(uiControlledApplication, "MyApp.MEP");

        var uploadPanel = _uiComponentCreateUtil.CreateRibbonPanel(uiControlledApplication, bimdataTab,
            "qwer",
            "Test panel for MyApp.MEP");

        var assemblyPath = Assembly.GetExecutingAssembly().Location;

        var uploadModelButton = _uiComponentCreateUtil.CreatePushButton(
            "Запустить тестовую команду",
            "MyApp.Commands.SampleOneExternalCommand",
            assemblyPath,         
            null,
            null,
            "Стартуем"
        );


        uploadPanel.AddItem(uploadModelButton);

        return bimdataTab;
    }
}