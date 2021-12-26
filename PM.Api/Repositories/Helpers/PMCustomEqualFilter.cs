using MongoDB.Driver;

namespace PM.Api.Repositories.Helpers;

public interface ICustomFilter<T>
{
    ICustomFilter<T> Filter<V>(bool propertyHasValue, string propertyName, V value);
    FilterDefinition<T> GetFilter();
}

public class PMCustomEqualFilter<T> : ICustomFilter<T>
{
    private FilterDefinition<T> _filter;
    private FilterDefinitionBuilder<T> _builder;

    public PMCustomEqualFilter()
    {
        _builder = Builders<T>.Filter;
        _filter = _builder.Empty;

    }
    public ICustomFilter<T> Filter<V>(bool propertyHasValue, string propertyName, V value)
    {
        if (propertyHasValue)
        {
            var currentFilter = _builder.Eq(propertyName, value);
            _filter &= currentFilter;
        }
        return this;
    }

    public FilterDefinition<T> GetFilter()
    {
        return _filter;
    }
}
