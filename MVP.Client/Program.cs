using MVP.Client;

class Program
{

    static async Task Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;


        var client = new Client();
        await client.Start("localhost", 5001);
    }
}