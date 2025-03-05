using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace MyApp.Shared.Services;

public class SerilogLoggerProvider : ILoggerProvider
{
    private readonly string _defaultCategoryName;

    public SerilogLoggerProvider(string defaultCategoryName = "Default")
    {
        _defaultCategoryName = defaultCategoryName;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new SerilogLogger(categoryName ?? _defaultCategoryName);
    }

    public void Dispose() { }
}
