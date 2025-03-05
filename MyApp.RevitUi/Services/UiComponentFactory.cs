using Autodesk.Revit.UI;
using Autodesk.Windows;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Reflection;
using System.Windows.Media;
using RibbonPanel = Autodesk.Revit.UI.RibbonPanel;

namespace MyApp.RevitUi.Services;
public class UiComponentFactory
{
    private readonly ILogger _logger;

    public UiComponentFactory(ILogger logger)
    {
        _logger = logger;
    }

    public PushButtonData CreatePushButton(string buttonName,
            string classFullName,
            string assemblyPath,
            ImageSource smallIcon = null,
            ImageSource largeIcon = null,
            string toolTip = null,
            ImageSource toolTipImage = null,
            string longDescription = null)
    {
        _logger.LogInformation("Создаю кнопку для команды: {ClassName}", classFullName);

        var pushButtonName = buttonName;
        var externalCommandName = classFullName.Split('.').Last();

        var pushButtonData = new PushButtonData(
            externalCommandName,
            pushButtonName,
            assemblyPath,
            classFullName
        );

        pushButtonData.LargeImage = largeIcon;
        pushButtonData.Image = smallIcon;
        pushButtonData.ToolTip = toolTip;
        pushButtonData.ToolTipImage = toolTipImage;
        pushButtonData.LongDescription = longDescription;

        return pushButtonData;
    }

    public RibbonPanel CreateRibbonPanel(UIControlledApplication application, RibbonTab ribbonTab,
        string panelName, string panelTittle)
    {
        _logger.LogInformation("Факторю риббон панель");

        RibbonPanel ribbonPanel = null;
        foreach (var internalRibbonPanel in ribbonTab.Panels)
        {
            var ribbonSource = internalRibbonPanel.Source;
            if (ribbonSource.Name != panelName) continue;
            ribbonPanel = application.GetRibbonPanels(ribbonTab.Name)
                .FirstOrDefault(name => name.Name == panelName);
            break;
        }

        if (ribbonPanel is null)
        {
            ribbonPanel = application.CreateRibbonPanel(ribbonTab.Name, panelName);
            ribbonPanel.Title = panelTittle;
        }

        return ribbonPanel;
    }

    public RibbonTab CreateRibbonTab(UIControlledApplication application, string tabPanelName)
    {
        _logger.LogInformation("Факторю таб");

        var ribbonControlTabs = ComponentManager.Ribbon.Tabs;
        var ribbonTab = ribbonControlTabs.FirstOrDefault(tab => tab.Name == tabPanelName);

        if (ribbonTab is null)
        {
            application.CreateRibbonTab(tabPanelName);
            ribbonTab = ribbonControlTabs.FirstOrDefault(tab => tab.Name == tabPanelName);
        }

        return ribbonTab;
    }
}
