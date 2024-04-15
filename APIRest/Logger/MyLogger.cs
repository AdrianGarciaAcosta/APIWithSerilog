using Serilog;
using System.Diagnostics;

namespace APIRest.Logger
{
    /// <summary>
    /// Logger personalizado que usa Serilog para escribir en fichero de texto
    /// </summary>
    public class MyLogger : IMyLogger
    {
        private readonly Serilog.Core.Logger _logger;

        public MyLogger()
        {
            // -----------------------------------------------------------------------------------------------
            // Configuración de Serilog
            // -----------------------------------------------------------------------------------------------
            // MinimumLevel.Debug()
            // - Indica el valor mímino del nivel de log que queremos controlar
            // WriteTo.File
            // - Permite la escritura en fichero
            // - Shared: true -> permite la múltiple escritura en el fichero indicado
            //                   si tenemos el logger como singleton no es necesario
            // - Requiere de nugget individual
            // Existen otras opciones que requieren más nuggets como son: WriteTo.Debug(), WriteTo.Console()
            // -----------------------------------------------------------------------------------------------
            _logger = new LoggerConfiguration()
                .MinimumLevel.Debug() // Set minimum level to log
                .WriteTo.File(@"C:\Dev\logs\api-logs.log")
                .CreateLogger();
        }

        /// <summary>
        /// Realiza la escritura de un mensaje de log
        /// </summary>
        /// <param name="message">Mensaje que será escrito en el log</param>
        public void Log(string message)
        {
            var callerName = GetCallerName();

            _logger.Verbose($"[CALLER: { callerName }] Verbose message: { message }");
            _logger.Debug($"[CALLER: { callerName }] Debug message: {message}");
            _logger.Information($"[CALLER: { callerName }] Info message: {message}");
            _logger.Warning($"[CALLER: { callerName }] Warning message: {message}");
            _logger.Error($"[CALLER: { callerName }] Error message: {message}");
            _logger.Fatal($"[CALLER: { callerName }] Fatal message: {message}");
        }

        /// <summary>
        /// Obtención del método que usa el logger
        /// </summary>
        /// <returns>Nombre del método que ha usado el logger</returns>
        private static string GetCallerName()
        {
            StackTrace trace = new StackTrace(StackTrace.METHODS_TO_SKIP + 2);

            var frame = trace.GetFrame(0);
            var caller = frame?.GetMethod();

            return caller?.Name ?? string.Empty;
        }
    }
}
