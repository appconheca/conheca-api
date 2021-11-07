using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConhecaApi.Models
{
    public class Endereco : IDomainObject
    {
        public int Id { get; set; }
        public int PontoTuristicoId { get; set; }

        public string Numero { get; set; }
        public string Logradouro { get; set; }
        public string Complemento { get; set; }
        public string Referencia { get; set; }
        public string CEP { get; set; }
        public Cidade Cidade { get; set; }

        public Endereco() { }


        public override string ToString()
        {
            return $"PontoTuristicoId:{PontoTuristicoId} - Id{Id}";
        }
    }
}
