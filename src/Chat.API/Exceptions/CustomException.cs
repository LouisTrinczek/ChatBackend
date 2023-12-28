namespace Chat.API.Exceptions;

public class CustomException : Exception
{
    public CustomException(string message)
        : base(message) { }
}
