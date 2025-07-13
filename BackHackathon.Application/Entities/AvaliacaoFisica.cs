using System.Text.Json.Serialization;

namespace BackHackathon.Application.Entities
{
    public class AvaliacaoFisica
    {
        [JsonPropertyName("CodigoCliente")]
        public int CodigoCliente { get; set; }
        [JsonPropertyName("Inativo")]
        public bool Inativo { get; set; }
        [JsonPropertyName("DataValidade")]
        public DateTime DataValidade { get; set; }
    }
}
