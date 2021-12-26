namespace PM.Api.Logger;

public interface ILogger
{
    void Log(string message);
}

public class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        //Connect remote and log
        Task.Run(() 
            => Console.WriteLine(message));
    }
}

