using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BackHackathon.Application.Entities
{
    public class Treino
    {
        [JsonPropertyName("CodigoCliente")]
        public int CodigoCliente { get; set; }
        [JsonPropertyName("Status")]
        public int Status { get; set; }
        [JsonPropertyName("DataCriacao")]
        public DateTime DataCriacao { get; set; }
    }
}
