using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackHackathon.Application.Services.Dtos
{
    public class ResponseApi<T>
    {
        public T Content { get; set; }
    }
}