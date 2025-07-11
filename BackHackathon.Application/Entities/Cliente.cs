using System.Text.Json.Serialization;

namespace BackHackathon.Application.Entities
{
    public class Cliente
    {
        [JsonPropertyName("Inativo")]
        public bool Inativo { get; set; }
        [JsonPropertyName("Nome")]
        public string Nome { get; set; }
        [JsonPropertyName("Id")]
        public int Id { get; set; }
        [JsonPropertyName("ClienteParametro")]
        public ClienteParametro ClienteParametro { get; set; }
        public int Score { get; set; } = 700;
        public string Faixa { get; set; } = "Engajamento médio";
    }

    public class ClienteParametro {

        [JsonPropertyName("Status")]
        public int Status { get; set; }
    }


}
