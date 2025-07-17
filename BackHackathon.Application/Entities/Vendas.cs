using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BackHackathon.Application.Entities
{
    public class Vendas
    {
        [JsonPropertyName("CodigoCliente")]
        public int CodigoCliente { get;set; }
        [JsonPropertyName("Data")]
        public DateTime Data { get; set; }
        [JsonPropertyName("Status")]
        public int Status { get; set; }
        [JsonPropertyName("VendaContrato")]
        public List<VendaContrato> VendaContrato { get; set; }
    }
    public class VendaContrato
    {
        [JsonPropertyName("CodigoContratoBase")]
        public int CodigoContratoBase { get; set; }
    }
}
