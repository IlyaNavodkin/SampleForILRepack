using Autodesk.Revit.UI;
using Autodesk.Windows;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaskDialog = Autodesk.Revit.UI.TaskDialog;

namespace MyApp.Shared.Services;
public class SimpleDimpleService(ILogger<SimpleDimpleService> _logger)
{
    public void SayHello()
    {
        TaskDialog.Show("Sample", "Hello world");
    }
}

