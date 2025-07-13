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
        private int _score = 700;
        public int Score
        {
            get => _score;
            set
            {
                if (value < 0)
                    _score = 0;
                else if (value > 1000)
                    _score = 1000;
                else
                    _score = value;
            }
        }
        public string Faixa { get; set; } = "Engajamento médio";
    }

    public class ClienteParametro {

        [JsonPropertyName("Status")]
        public int Status { get; set; }
    }


}
