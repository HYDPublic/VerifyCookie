using CommandLine;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace VerifyCookie
{
    class Program
    {
        static async Task VerifyCookie(string url, HttpTransportType transportType, bool addHandler)
        {
            var cookies = new CookieContainer();
            var httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
                MaxConnectionsPerServer = 200000,
                MaxAutomaticRedirections = 200000,
                CookieContainer = cookies,
            };
            var hubConnectionBuilder = new HubConnectionBuilder()
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                })
                .WithUrl(url, httpConnectionOptions =>
                {
                    if (addHandler)
                    {
                        httpConnectionOptions.HttpMessageHandlerFactory = _ => httpClientHandler;
                    }

                    httpConnectionOptions.Cookies = cookies;
                    httpConnectionOptions.Transports = transportType;
                    httpConnectionOptions.CloseTimeout = TimeSpan.FromMinutes(100);
                });
            var connection = hubConnectionBuilder.Build();
            var method = "echo";
            var received = false;
            connection.On(method, (string uid, string time) =>
            {
                Console.WriteLine($"receive {uid} {time}");
                received = true;
            });
            CancellationTokenSource closedTokenSource = null;

            connection.Closed += e =>
            {
                // This should never be null by the time this fires
                closedTokenSource.Cancel();

                Console.WriteLine("Connection closed...");
                return Task.CompletedTask;
            };
            await Task.WhenAll(connection.StartAsync());
            await connection.SendAsync(method, "abc", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            closedTokenSource = new CancellationTokenSource();
            while (!received && !closedTokenSource.Token.IsCancellationRequested)
            {
                await Task.Delay(1000);
                Console.WriteLine("Waiting for result");
            }
            closedTokenSource?.Dispose();
        }
        static void Main(string[] args)
        {
            bool isInputValid = true;
            var argsOption = new ArgsOption();
            var result = Parser.Default.ParseArguments<ArgsOption>(args)
                .WithParsed(options => argsOption = options)
                .WithNotParsed(error => {
                    isInputValid = false;
                    Console.WriteLine($"Input is invalid for {error}");
                });
            if (!isInputValid)
            {
                return;
            }

            var transportType = HttpTransportType.WebSockets;
            switch (argsOption.TransportType)
            {
                case "LongPolling":
                    transportType = HttpTransportType.LongPolling;
                    break;
                case "ServerSentEvents":
                    transportType = HttpTransportType.ServerSentEvents;
                    break;
            }
            VerifyCookie(argsOption.HubUrl, transportType, Boolean.Parse(argsOption.UseHandler)).GetAwaiter().GetResult();
        }
    }
}
