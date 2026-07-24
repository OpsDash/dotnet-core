namespace OpsDash.Core.WebApi.Results;

public record Success;
public record Success<T>(T Value);

public union Result(Success, BaseError);
public union Result<T>(Success<T>, BaseError);