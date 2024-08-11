using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

internal class Program
{
    

    static async Task Main(string[] args)
    {
        Console.WriteLine("Start program as Sender or Listener? (S/L):");
        string role = Console.ReadLine()?.Trim().ToUpper();

        if (role == "S")
        {
            // Sender
            string remoteIpAddress = "127.0.0.1"; // Ziel-IP-Adresse
            int remotePort = 15000; // Ziel-Port

            Console.Write("Please enter remote IP: ");
            remoteIpAddress = Console.ReadLine();

            Console.Write("Please enter remote Port: ");
            remotePort = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Connecting to server...");

            // Verbindung zum entfernten Server herstellen
            await ConnectToServerAsync(remoteIpAddress, remotePort);
        }
        else if (role == "L")
        {
            // Listener
            int localPort = 15000; // Lokaler Port für den Listener

            Console.Write("Please enter local Port: ");
            localPort = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Starting TCP listener...");

            // Starten des TCP Listeners
            await StartTcpListenerAsync(localPort);
        }
        else
        {
            Console.WriteLine("Invalid input. Please restart the program and choose either 'S' for Sender or 'L' for Listener.");
        }
    }

    private static async Task StartTcpListenerAsync(int port)
    {
        TcpListener listener = new TcpListener(IPAddress.Any, port);
        listener.Start();
        Console.WriteLine($"Listening on port {port}...");

        while (true)
        {
            TcpClient client = await listener.AcceptTcpClientAsync();
            Console.WriteLine("Client connected!");

            _ = Task.Run(async () =>
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    string receivedText = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Received: {receivedText}");
                }

                client.Close();
            });
        }
    }

    private static async Task ConnectToServerAsync(string ipAddress, int port)
    {   
        try
        {
            using (TcpClient client = new TcpClient())
            {
                await client.ConnectAsync(IPAddress.Parse(ipAddress), port);
                Console.WriteLine($"Connected to server {ipAddress}:{port}");

                using (NetworkStream stream = client.GetStream())
                {
                    while (true)
                    {
                        Console.Write("Enter message (type 'Quit' to exit): ");
                        string message = Console.ReadLine();

                        if (string.Equals(message, "Quit", StringComparison.OrdinalIgnoreCase))
                        {
                            break;
                        }

                        byte[] data = Encoding.UTF8.GetBytes(message);
                        await stream.WriteAsync(data, 0, data.Length);
                        Console.WriteLine("Message sent to the server.");
                    }
                }
            }
            Console.WriteLine("Disconnected from server.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}