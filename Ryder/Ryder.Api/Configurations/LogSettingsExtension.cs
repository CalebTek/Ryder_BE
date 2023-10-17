using Serilog;

namespace Ryder.Api.Configurations
{
    public static class LogSettingsExtension
    {
        public static void SetupSeriLog(this IServiceCollection services, IConfiguration config)
        {
            // The full URL
            //string redisUrl =
            //    "rediss://red-ck4c6iuct0pc739o9dc0:7pyX7QnZuEeEg4Qso9TfobxgkmjIxLvo@oregon-redis.render.com:6379";

            //// Parse the URL using System.Uri
            //Uri uri = new Uri(redisUrl);

            //// Extract the components
            //string scheme = uri.Scheme; // rediss
            //string username = uri.UserInfo.Split(':')[0]; // red-ck4c6iuct0pc739o9dc0
            //string password = uri.UserInfo.Split(':')[1]; // 7pyX7QnZuEeEg4Qso9TfobxgkmjIxLvo
            //string hostname = uri.Host; // oregon-redis.render.com
            //int port = uri.Port; // 6379

            //// Construct the connection string
            //string connectionString = $"{hostname}:{port},{password}";

            //// Use the connection string to connect to Redis
            //var connection = ConnectionMultiplexer.Connect(hostname);

            //var db = connection.GetDatabase();
            //db.ListLeftPush("logs", "Log message: This is a log entry.");
            //connection.Close();


            //var redisConfiguration = new RedisConfiguration();
            //redisConfiguration.Host = "127.0.0.1:6379";

            var logger = new LoggerConfiguration()
                .MinimumLevel.Information() // Set minimum log level
                .WriteTo.Console() // Log to the console (optional)
                //.WriteTo.Sink(new RedisSink(
                // "oregon-redis.render.com:6379"))
                .CreateLogger();

            logger.Information("This is an informational message....");
        }
    }
}

//public class RedisSink : ILogEventSink, IDisposable
//{
//    private readonly ConnectionMultiplexer _redis;
//    private readonly IDatabase _db;
//    private const string ListKey = "logs";

//    public RedisSink(string connectionString)
//    {
//        _redis = ConnectionMultiplexer.Connect(connectionString);
//        _db = _redis.GetDatabase();
//    }

//    public void Emit(LogEvent logEvent)
//    {
//        var message = logEvent.RenderMessage();
//        _db.ListLeftPush(ListKey, message);
//    }

//    public void Dispose()
//    {
//        _redis.Dispose();
//    }
//}