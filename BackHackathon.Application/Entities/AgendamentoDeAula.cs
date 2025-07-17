using System.Text.Json.Serialization;

namespace BackHackathon.Application.Entities
{
    public class AgendamentoDeAula
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }
        [JsonPropertyName("Status")]
        public int Status { get; set; }
        [JsonPropertyName("Participantes")]
        public List<Participantes> Participantes { get; set; } = new List<Participantes>();
        [JsonPropertyName("DataInicial")]
        public DateTime DataInicial { get; set; }
        [JsonPropertyName("DataFinal")]
        public DateTime DataFinal { get; set; }
    }
    public class Participantes { 
        [JsonPropertyName("CodigoCliente")]
        public int CodigoCliente { get; set; }
        [JsonPropertyName("Status")]
        public int Status { get; set; }

    }

}
