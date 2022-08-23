using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Websocket_API
{
    public class WsClient
    {
        ClientWebSocket wsClient = new ClientWebSocket();

        public async Task OpenConnectionAsync(CancellationToken token)
        {

            wsClient.Options.KeepAliveInterval = TimeSpan.Zero;

            wsClient.Options.DangerousDeflateOptions = new WebSocketDeflateOptions
            {
                ServerContextTakeover = true,
                ClientMaxWindowBits = 15
            };

            await wsClient.ConnectAsync(new Uri("wss://ws.poloniex.com/ws/public"), token).ConfigureAwait(false);
        }

        public async Task SendAsync(string message, CancellationToken token)
        {
            var messageBuffer = Encoding.UTF8.GetBytes(message);
            await wsClient.SendAsync(new ArraySegment<byte>(messageBuffer), WebSocketMessageType.Text, true, token).ConfigureAwait(false);
        }

        //Receiving messages
        private async Task ReceiveMessageAsync(byte[] buffer)
        {
            while (true)
            {
                try
                {
                    var result = await wsClient.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None).ConfigureAwait(false);

                    //Here is the received message as string
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                     Console.WriteLine(message);
                    if (result.EndOfMessage) break;
                }
                catch (Exception ex)
                {
                    break;
                }
            }
        }

        public async Task HandleMessagesAsync(CancellationToken token)
        {
            var buffer = new byte[1000000000];
            while (wsClient.State == WebSocketState.Open)
            {
                await ReceiveMessageAsync(buffer);
            }
            if (wsClient.State != WebSocketState.Open)
            {
                //_logger.LogInformation("Connection closed. Status: {s}", WsClient.State.ToString());
                // Your logic if state is different than `WebSocketState.Open`
            }
        }

    }
}
