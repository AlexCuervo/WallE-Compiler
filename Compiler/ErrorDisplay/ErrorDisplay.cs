public class ErrorDisplay(string message) : Exception
{
    string message = message;
    public string getMessage => message;
}