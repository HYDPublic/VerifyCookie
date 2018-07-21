# This sample demonstrates how to reproduce the cookie missing issue when using Websockets. If HttpHandler is used, the cookie is missed, otherwise, it does not miss.

* Cookie is not missing for the following command
`dotnet run --hub "http://signalrlinux3.southeastasia.cloudapp.azure.com:5050/signalrbench" --useHandler false`

`E:\home\Work\VerifyCookie>dotnet run --hub "http://signalrlinux3.southeastasia.cloudapp.azure.com:5050/signalrbench" --useHandler false
info: Microsoft.AspNetCore.Http.Connections.Client.Internal.WebSocketsTransport[1]
      Starting transport. Transfer mode: Text. Url: 'ws://dev-dayshen-57.servicedev.signalr.net:5001/client/?hub=benchhub&id=ODZ2bHpInmATlW3E6OhLmQ'.
info: Microsoft.AspNetCore.Http.Connections.Client.HttpConnection[3]
      HttpConnection Started.
info: Microsoft.AspNetCore.SignalR.Client.HubConnection[24]
      Using HubProtocol 'json v1'.
info: Microsoft.AspNetCore.SignalR.Client.HubConnection[44]
      HubConnection started.
receive 5 1532168527250
Waiting for result`

* Cookie is missing for the following command
`dotnet run --hub "http://signalrlinux3.southeastasia.cloudapp.azure.com:5050/signalrbench" --useHandler true`

`E:\home\Work\VerifyCookie>dotnet run --hub "http://signalrlinux3.southeastasia.cloudapp.azure.com:5050/signalrbench" --useHandler true
info: Microsoft.AspNetCore.Http.Connections.Client.Internal.WebSocketsTransport[1]
      Starting transport. Transfer mode: Text. Url: 'ws://dev-dayshen-57.servicedev.signalr.net:5001/client/?hub=benchhub&id=BZCV_kzOZpIEKLdmgtzZoQ'.
fail: Microsoft.AspNetCore.Http.Connections.Client.HttpConnection[11]
      Failed to start connection. Error starting transport 'WebSockets'.
System.Net.WebSockets.WebSocketException (997): Unable to connect to the remote server
   at System.Net.WebSockets.WebSocketHandle.ConnectAsyncCore(Uri uri, CancellationToken cancellationToken, ClientWebSocketOptions options)
   at System.Net.WebSockets.ClientWebSocket.ConnectAsyncCore(Uri uri, CancellationToken cancellationToken)
   at Microsoft.AspNetCore.Http.Connections.Client.Internal.WebSocketsTransport.StartAsync(Uri url, TransferFormat transferFormat)
   at Microsoft.AspNetCore.Http.Connections.Client.HttpConnection.StartTransport(Uri connectUrl, HttpTransportType transportType, TransferFormat transferFormat)
Unhandled Exception: System.InvalidOperationException: Unable to connect to the server with any of the available transports.
   at Microsoft.AspNetCore.Http.Connections.Client.HttpConnection.SelectAndStartTransport(TransferFormat transferFormat)
   at Microsoft.AspNetCore.Http.Connections.Client.HttpConnection.StartAsyncCore(TransferFormat transferFormat)
   at Microsoft.AspNetCore.Http.Connections.Client.HttpConnection.StartAsync(TransferFormat transferFormat, CancellationToken cancellationToken)
   at Microsoft.AspNetCore.SignalR.Client.HttpConnectionFactory.ConnectAsync(TransferFormat transferFormat, CancellationToken cancellationToken)
   at Microsoft.AspNetCore.SignalR.Client.HttpConnectionFactory.ConnectAsync(TransferFormat transferFormat, CancellationToken cancellationToken)
   at Microsoft.AspNetCore.SignalR.Client.HubConnection.StartAsyncCore(CancellationToken cancellationToken)
   at Microsoft.AspNetCore.SignalR.Client.HubConnection.StartAsync(CancellationToken cancellationToken)
   at VerifyCookie.Program.VerifyCookie(String url, HttpTransportType transportType, Boolean addHandler) in E:\home\Work\VerifyCookie\Program.cs:line 54
   at VerifyCookie.Program.Main(String[] args) in E:\home\Work\VerifyCookie\Program.cs:line 89`

