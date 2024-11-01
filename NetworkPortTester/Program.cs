using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
            string remoteIpAddress = "127.0.0.1"; // Default target IP address
            int remotePort = 15000; // Default target port

            // Get remote IP address from user
            Console.Write("Please enter remote IP: ");
            remoteIpAddress = Console.ReadLine();

            // Get remote port from user
            Console.Write("Please enter remote Port: ");
            remotePort = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Connecting to server...");

            // Connect to the remote server
            await ConnectToServerAsync(remoteIpAddress, remotePort);
        }
        else if (role == "L")
        {
            // Listener
            int localPort = 15000; // Default local port for listener

            // Get local port from user
            Console.Write("Please enter local Port: ");
            localPort = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Starting TCP listener...");

            // Start the TCP listener
            await StartTcpListenerAsync(localPort);
        }
        else
        {
            Console.WriteLine("Invalid input. Please restart the program and choose either 'S' for Sender or 'L' for Listener.");
        }
    }

    private static async Task StartTcpListenerAsync(int port)
    {
        // Create a TCP listener on the specified port
        TcpListener listener = new TcpListener(IPAddress.Any, port);
        listener.Start();
        Console.WriteLine($"Listening on port {port}...");

        while (true)
        {
            // Accept incoming client connection
            TcpClient client = await listener.AcceptTcpClientAsync();
            Console.WriteLine("Client connected!");

            // Handle client connection in a separate task
            _ = Task.Run(async () =>
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead;

                // Get the IP address of the connected client
                string remoteIp = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();

                // Read data from the client
                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    string receivedText = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    // Display the received message along with the timestamp and client IP address
                    Console.WriteLine($"[{timeStamp}] Received from {remoteIp}: {receivedText}");
                }

                // Close the client connection
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
                // Attempt to connect to the server
                await client.ConnectAsync(IPAddress.Parse(ipAddress), port);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Connected to server {ipAddress}:{port}");
                Console.ResetColor();

                using (NetworkStream stream = client.GetStream())
                {
                    while (true)
                    {
                        // Get message from user to send to server
                        Console.Write("Enter message (type 'Quit' to exit): ");
                        string message = Console.ReadLine();

                        // Exit the loop if the user types 'Quit'
                        if (string.Equals(message, "Quit", StringComparison.OrdinalIgnoreCase))
                        {
                            break;
                        }

                        // Send the message to the server
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
            // Handle any errors that occur during connection
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
