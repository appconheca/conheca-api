using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConhecaApi.Models
{
    public class Imagem
    {
        public int Url { get; set; }
        public Usuario Usuario { get; set; }
        public PontoTuristico PontoTuristico { get; set; }
    }
}


