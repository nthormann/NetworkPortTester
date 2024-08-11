
# TCP Client-Server Application

This project is a simple C# console application that demonstrates TCP client-server communication. The application can be run in two modes: **Sender** or **Listener**. 

## Features

- **Sender Mode**: Allows the user to send messages to a TCP server.
- **Listener Mode**: Acts as a TCP server, listening for incoming connections and displaying received messages.

## Getting Started

### Prerequisites

- .NET Core SDK installed on your machine.

### Running the Application

1. Clone the repository to your local machine.
2. Open the project in your preferred IDE (e.g., Visual Studio, VS Code).
3. Build the project to restore dependencies and compile the code.
4. Run the application.

### Usage

When you start the application, it will prompt you to choose between **Sender** and **Listener** mode:

- **Sender (S)**:
  - The application will prompt you to enter the IP address and port of the remote server you want to connect to.
  - Once connected, you can start typing messages. Each message will be sent to the server.
  - To exit, type `Quit`.

- **Listener (L)**:
  - The application will prompt you to enter the local port on which it should listen for incoming connections.
  - The application will then wait for a client to connect. When a client sends a message, it will be displayed in the console.

### Example

#### Sender Mode:

```sh
Start program as Sender or Listener? (S/L): S
Please enter remote IP: 127.0.0.1
Please enter remote Port: 15000
Connecting to server...
Connected to server 127.0.0.1:15000
Enter message (type 'Quit' to exit): Hello, Server!
Message sent to the server.
Enter message (type 'Quit' to exit): Quit
Disconnected from server.
```

#### Listener Mode:

```sh
Start program as Sender or Listener? (S/L): L
Please enter local Port: 15000
Starting TCP listener...
Listening on port 15000...
Client connected!
Received: Hello, Server!
```

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request or open an issue to discuss any changes or improvements.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- This application serves as a basic example of TCP client-server communication in C#.
