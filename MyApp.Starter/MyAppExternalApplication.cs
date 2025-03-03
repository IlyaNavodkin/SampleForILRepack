using Autodesk.Revit.UI;
using Autodesk.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyApp.Shared.ExternalCommands;
using MyApp.Shared.Services;
using MyApp.Starter.Helpers;
using Serilog;
using System;
using System.Linq;
using System.Reflection;
using System.Windows.Media;
using RibbonPanel = Autodesk.Revit.UI.RibbonPanel;

namespace MyApp.Starter;

public class MyAppExternalApplication : IExternalApplication
{
    public Result OnShutdown(UIControlledApplication application)
    {
        Log.CloseAndFlush();

        return Result.Succeeded;
    }

    public Result OnStartup(UIControlledApplication uIControlledApplication)
    {
        try
        {
            var configurator = RevitUiConfigurator.GetInstance();
            configurator.ConfigureRevitUiComponents(uIControlledApplication);

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


public class RevitUiConfigurator
{
    private static readonly Lazy<RevitUiConfigurator> Instance =
        new Lazy<RevitUiConfigurator>(() => new RevitUiConfigurator());

    public static RevitUiConfigurator GetInstance() => Instance.Value;

    public RibbonTab ConfigureRevitUiComponents(UIControlledApplication uiControlledApplication)
    {
        var uiService = UiComponentCreateUtil.GetInstance();

        var bimdataTab = uiService.CreateRibbonTab(uiControlledApplication, "BIDTP");

        var uploadPanel = uiService.CreateRibbonPanel(uiControlledApplication, bimdataTab,
            "BIDTP_PN1",
            "Выбрать файл WPF клиента");

        var uploadModelButton = uiService
            .CreatePushButton<SampleOneExternalCommand>
            (
                "Выбрать файл WPF клиента",
                null,
                null,
                "Выбирает файл WPF клиента и запускает его - передавая необходимые аргументы"
            );


        uploadPanel.AddItem(uploadModelButton);

        return bimdataTab;
    }


    public class UiComponentCreateUtil
    {
        private static readonly Lazy<UiComponentCreateUtil> Instance =
            new Lazy<UiComponentCreateUtil>(() => new UiComponentCreateUtil());

        public static UiComponentCreateUtil GetInstance() => Instance.Value;

        public PushButtonData CreatePushButton<T>(string buttonName,
            ImageSource smallIcon = null, ImageSource largeIcon = null, string toolTip = null, ImageSource toolTipImage = null,
            string longDescription = null)
            where T : IExternalCommand
        {
            var pushButtonName = buttonName;
            var externalCommandName = typeof(T).Name;
            var className = typeof(T).FullName;

            var pushButtonData = new PushButtonData(externalCommandName,
                pushButtonName, Assembly.GetAssembly(typeof(T)).Location, className);

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
}
