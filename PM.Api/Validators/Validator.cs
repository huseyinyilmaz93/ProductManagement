using MongoDB.Bson;
using PM.Api.Exceptions;

namespace PM.Api.Validators;

public interface IValidator
{
    void ValidateObjectId(string id);
}

public class Validator : IValidator
{
    public void ValidateObjectId(string id)
    {
        if (!ObjectId.TryParse(id, out _))
            throw new Validate.ObjectIdException(id);
    }
}

