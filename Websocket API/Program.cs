using System.Net;
using System.Net.Sockets;
using Websocket_API;

string s = "{\"event\": \"subscribe\",\"channel\": [\"ticker\"], \"symbols\": [\"all\"]}";
var client = new WsClient();
await client.OpenConnectionAsync(CancellationToken.None);
await client.SendAsync(s, CancellationToken.None);
await client.HandleMessagesAsync(CancellationToken.None);