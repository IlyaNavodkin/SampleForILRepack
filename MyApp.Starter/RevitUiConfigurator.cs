using Autodesk.Revit.UI;
using Autodesk.Windows;
using MyApp.Shared.ExternalCommands;
using System;
using System.Linq;
using System.Reflection;
using System.Windows.Media;
using RibbonPanel = Autodesk.Revit.UI.RibbonPanel;

namespace MyApp.Starter;

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
