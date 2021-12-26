namespace PM.Api.Managers;

public abstract class AbstractRemoteConnectionManager
{
    private bool _isAlive;
    private readonly object _lock = new object();

    protected void SetAliveStatus(bool isAlive)
    {
        lock (_lock)
            _isAlive = isAlive;
    }

    protected bool GetAliveStatus()
    {
        lock (_lock)
            return _isAlive;
    }
}

