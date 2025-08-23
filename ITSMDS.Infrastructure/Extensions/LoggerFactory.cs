

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace ITSMDS.Infrastructure.Extensions;

public static class StaticLoggerFactory
{
    private static ILoggerFactory _loggerFactory;

    public static void Initialize(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
    }

    public static ILogger GetLogger<T>()
    {
        return _loggerFactory?.CreateLogger<T>() ?? NullLogger<T>.Instance;
    }

    public static ILogger GetLogger(string className)
    {
        return _loggerFactory?.CreateLogger(className) ?? NullLogger.Instance;
    }
}

//public static class StaticLoggerFactory
//{
//    private static ILoggerFactory _loggerFactory;
//    public static void Initialize(ILoggerFactory loggerFactory)
//    {
//        _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
//    }

//    public static ILogger GetLogger<T>()
//    {
//        if (_loggerFactory == null)
//        {
//            throw new InvalidOperationException("StaticLoggerFactory is not initialized.");

//        }
//        return _loggerFactory.CreateLogger<T>();
//    }
//    public static ILogger GetLogger(string className)
//    {
//        if (_loggerFactory == null)
//        {
//            throw new InvalidOperationException("StaticLoggerFactory is not initialized.");

//        }
//        return _loggerFactory.CreateLogger(className);
//    }
//}
