namespace MVP.Server
{ 
public class Logger
{
    private string path = "logs/server.log";

    public void Log(string message)
    {
        var line = $"[{DateTime.Now}] {message}";
        File.AppendAllText(path, line + "\n");
    }
}
}