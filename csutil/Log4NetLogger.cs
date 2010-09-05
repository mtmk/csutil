namespace csutil
{
    public class Log4NetLogger : ISimpleLogger
    {
        public void Log(string tag, string format, params object[] args)
        {
            log4net.LogManager.GetLogger(tag).InfoFormat(format, args);
        }

        public void Log(string tag, string message)
        {
            log4net.LogManager.GetLogger(tag).Info(message);
        }

    }
}