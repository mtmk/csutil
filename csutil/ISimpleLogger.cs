namespace csutil
{
    public interface ISimpleLogger
    {
        void Log(string tag, string format, params object[] args);
        void Log(string tag, string message);
    }
}