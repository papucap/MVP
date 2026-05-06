using MVP.Client;

class Program
{
    static async Task Main(string[] args)
    {
        var client = new Client();
        await client.Start("localhost", 5001);
    }
}