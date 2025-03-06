using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace MyApp.MEP.ExternalCommands;

public partial class PickPointViewModel : ObservableObject
{
    private readonly Document _document;
    private readonly UIDocument _uiDocument;
    private readonly HttpRestService _httpRestService;
    private readonly RevitTask _revitTask = new();
    private readonly ExternalEventHandler _handler;
    private readonly WindowStateService _windowStateService;

    public ObservableCollection<LineDto> Lines { get; } = new();

    // Событие для закрытия окна
    public event Action? RequestClose;

    public PickPointViewModel(Document document, UIDocument uiDocument, HttpRestService httpRestService, ExternalEventHandler eventHandler, WindowStateService windowStateService)
    {
        _document = document;
        _uiDocument = uiDocument;
        _httpRestService = httpRestService;
        _handler = eventHandler;
        _windowStateService = windowStateService;
    }

    [RelayCommand]
    private async Task PickPointsAndDrawLine()
    {

        await _revitTask.Run(app =>
        {
            try
            {
                _windowStateService.HideWindow();

                var uidoc = app.ActiveUIDocument;
                var doc = uidoc.Document;

                var firstPoint = uidoc.Selection.PickPoint("Выберите первую точку.");
                var secondPoint = uidoc.Selection.PickPoint("Выберите вторую точку.");

                if (firstPoint == null || secondPoint == null)
                    return;

                using (var trans = new Transaction(doc, "Создание 2D-линии"))
                {
                    trans.Start();
                    var line = Line.CreateBound(firstPoint, secondPoint);
                    doc.Create.NewDetailCurve(uidoc.ActiveView, line);
                    trans.Commit();
                }

                Lines.Add(new LineDto(firstPoint, secondPoint));
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Ошибка", ex.Message);
            }
            finally
            {
                _windowStateService.ShowWindow();
            }
        });
    }

    [RelayCommand]
    private void Commit()
    {
        _handler.SetLines(new List<LineDto>(Lines));

        _windowStateService.CloseWindow();
    }
}

