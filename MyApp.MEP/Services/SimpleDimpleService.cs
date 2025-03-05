using Autodesk.Revit.UI;
using Autodesk.Windows;
using Microsoft.Extensions.Logging;
using MyApp.Logging;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaskDialog = Autodesk.Revit.UI.TaskDialog;

namespace MyApp.MEP.Services;
public class SimpleDimpleService()
{
    public void SayHello()
    {
        AppLogger.Info("Ping");
        TaskDialog.Show("Sample", "Hello world");
    }
}

