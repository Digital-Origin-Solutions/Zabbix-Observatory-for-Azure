using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZabbixSend.Models
{
    public class ZabbixSendBody
    {
        [JsonProperty("request")]
        public string Request { get; set; } = "sender data";
        [JsonProperty("data")]
        public ZabbixSendData[] Data { get; set; }
    }
    public class ZabbixSendData(string ZabbixHost, string ZabbixKey, string ZabbixValue)
    {
        [JsonProperty("host")]
        public string Host { get; set; } = ZabbixHost;
        [JsonProperty("key")]
        public string Key { get; set; } = ZabbixKey;
        [JsonProperty("value")]
        public string Value { get; set; } = ZabbixValue;
    }
}
