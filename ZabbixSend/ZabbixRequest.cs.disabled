using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ZabbixSend.Models;

namespace ZabbixSend
{
    public class ZabbixRequest
    {
        private string Server { get; set; }
        private int Port { get; set; }
        private int TimeOut { get; set; }

        public ZabbixRequest(string ZabbixServer, int ZabbixPort, int ZabbixTimeOut = 500)
        {
            Server = ZabbixServer;
            Port = ZabbixPort;
            TimeOut = ZabbixTimeOut;
        }

        public async Task<ZabbixResponse> Send(string ZabbixHost, string ZabbixKey, string ZabbixValue)
        {
            ZabbixSendBody data = new()
            {
                Request = "sender data",
                Data = [new(ZabbixHost, ZabbixKey, ZabbixValue)]
            };
            string JsonRequest = JsonConvert.SerializeObject(data);
            using (TcpClient TCPc = new(Server, Port))
            using (NetworkStream Stream = TCPc.GetStream())
            {
                byte[] Header = Encoding.ASCII.GetBytes("ZBXD\x01");
                byte[] DataLen = BitConverter.GetBytes((long)JsonRequest.Length);
                byte[] Content = Encoding.ASCII.GetBytes(JsonRequest);
                byte[] Message = new byte[Header.Length + DataLen.Length + Content.Length];
                Buffer.BlockCopy(Header, 0, Message, 0, Header.Length);
                Buffer.BlockCopy(DataLen, 0, Message, Header.Length, DataLen.Length);
                Buffer.BlockCopy(Content, 0, Message, Header.Length + DataLen.Length, Content.Length);

                Stream.Write(Message, 0, Message.Length);
                Stream.Flush();
                int counter = 0;
                while (!Stream.DataAvailable)
                {
                    if (counter < TimeOut)
                    {
                        counter++;
                        await Task.Delay(50);
                    } else
                    {
                        throw new TimeoutException();
                    }
                }

                byte[] ResponseBytes = new Byte[1024];
                Stream.Read(ResponseBytes, 0, ResponseBytes.Length);
                string StringResponse = Encoding.UTF8.GetString(ResponseBytes);
                string JsonResponse = StringResponse.Substring(StringResponse.IndexOf('{'));
                return JsonConvert.DeserializeObject<ZabbixResponse>(JsonResponse);
            }
        }
    }
}
