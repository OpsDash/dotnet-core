namespace OpsDash.Core.WebApi.Results;

public sealed record class ValidationError(string Message, IDictionary<string, object?>? HttpExtensions = null)
    : BaseError(Message, 400, "Validation Failed", HttpExtensions);

public sealed record class NotFoundError(string Message, IDictionary<string, object?>? HttpExtensions = null)
    : BaseError(Message, 404, "Not Found", HttpExtensions);

public sealed record class ConflictError(string Message, IDictionary<string, object?>? HttpExtensions = null)
    : BaseError(Message, 409, "Conflict", HttpExtensions);