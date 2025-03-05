using Autodesk.Revit.DB;
using Microsoft.Extensions.Logging;
using MyApp.Logging;
using MyApp.MEP.Services;
using System;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace MyApp.MEP.Updaters;

public class ElementChangeLoggerUpdater : IUpdater
{
    private readonly UpdaterId _updaterId;
    private readonly SimpleDimpleService _simpleDimpleService;

    public ElementChangeLoggerUpdater(AddInId addInId, SimpleDimpleService simpleDimpleService)
    {
        _updaterId = new UpdaterId(addInId, new Guid("F9C8E4BB-6E40-4AE5-947B-21C8F421A78A"));
        _simpleDimpleService = simpleDimpleService;
    }

    public void Execute(UpdaterData data)
    {
        var doc = data.GetDocument();

        foreach (var id in data.GetModifiedElementIds())
        {
            Element element = doc.GetElement(id);
            AppLogger.Info($"Modified Element: ID = {id.IntegerValue}, Type = {element?.GetType().Name ?? "Unknown"}");
        }

        foreach (var id in data.GetAddedElementIds())
        {
            Element element = doc.GetElement(id);
            AppLogger.Info($"Added Element: ID = {id.IntegerValue}, Type = {element?.GetType().Name ?? "Unknown"}");
        }

        foreach (var id in data.GetDeletedElementIds())
        {
            AppLogger.Info($"Deleted Element ID = {id.IntegerValue}");
        }
    }

    public string GetAdditionalInformation() => "Logs changes to elements.";
    public ChangePriority GetChangePriority() => ChangePriority.FloorsRoofsStructuralWalls;
    public UpdaterId GetUpdaterId() => _updaterId;
    public string GetUpdaterName() => "ElementChangeLoggerUpdater";
}
