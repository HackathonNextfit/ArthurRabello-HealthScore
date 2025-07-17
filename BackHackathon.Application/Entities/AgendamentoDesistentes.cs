using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BackHackathon.Application.Entities
{
    public class AgendamentoDesistentes
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }
        [JsonPropertyName("AgendaParticipantesCancelados")]
        public List<AgendaParticipanteCancelado> AgendaParticipantesCancelados { get; set; }

    }
    public class AgendaParticipanteCancelado
    {
        [JsonPropertyName("CodigoCliente")]
        public int CodigoCliente { get; set; }
        [JsonPropertyName("Status")]
        public int Status { get; set; }
    }
}
