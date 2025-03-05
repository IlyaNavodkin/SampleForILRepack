using Autodesk.Revit.UI;
using Autodesk.Windows;
using Microsoft.Extensions.Logging;
using MyApp.Logging;
using System;
using System.Linq;
using System.Reflection;
using System.Windows.Media;
using RibbonPanel = Autodesk.Revit.UI.RibbonPanel;

namespace MyApp.RevitUi.Services;
public class UiComponentFactory
{
    public PushButtonData CreatePushButton(string buttonName,
            string classFullName,
            string assemblyPath,
            ImageSource smallIcon = null,
            ImageSource largeIcon = null,
            string toolTip = null,
            ImageSource toolTipImage = null,
            string longDescription = null)
    {
        AppLogger.Info("Starting Create Button");

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
        AppLogger.Info("Starting Create Ribbon Panel");

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
        AppLogger.Info("Starting Create Ribbon Tab");

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
