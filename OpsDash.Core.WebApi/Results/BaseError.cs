namespace OpsDash.Core.WebApi.Results;

public abstract record BaseError(
    string Message,
    int HttpStatusCode,
    string HttpTitle,
    IDictionary<string, object?>? HttpExtensions = null
);