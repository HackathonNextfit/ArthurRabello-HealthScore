using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BackHackathon.Application.Entities
{
    public class PessoaPresenca
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }
        [JsonPropertyName("CodigoPessoa")]
        public int CodigoPessoa { get; set; }
        [JsonPropertyName("DataHora")]
        public DateTime DataHora { get; set; }
    }
}
