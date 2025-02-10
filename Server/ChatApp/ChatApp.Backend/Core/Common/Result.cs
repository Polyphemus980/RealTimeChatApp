namespace ChatApp.Backend.Core.Common;

public record Result<T>(
    bool IsSuccess,
    T? Data = default,
    string? ErrorMessage = null,
    int StatusCode = StatusCodes.Status200OK
)
{
    public static Result<T> Success(T data, int statusCode = StatusCodes.Status200OK) =>
        new(true, data, null, statusCode);

    public static Result<T> Failure(
        string errorMessage,
        int statusCode = StatusCodes.Status400BadRequest
    ) => new(false, default, errorMessage, statusCode);
}
