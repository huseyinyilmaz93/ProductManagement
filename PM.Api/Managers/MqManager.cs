namespace PM.Api.Managers;
public interface IMqManager
{
    void EnsureMqAvailibity();
    void Publish<T>(T contract) where T : class;
}

public class DummyMqManager : IMqManager
{
    private bool _isAvailable = true;
    public void EnsureMqAvailibity()
    {
        if (!_isAvailable)
            throw new Exception();
    }

    public void Publish<T>(T contract) where T : class
    {
            
    }
}

public class RabbitMqManager : IMqManager
{
    public void EnsureMqAvailibity()
    {
        throw new NotImplementedException();
    }

    public void Publish<T>(T contract) where T : class
    {
        throw new NotImplementedException();
    }
}

