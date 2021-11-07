using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConhecaApi.Models
{
    public class Cidade : IDomainObject
    {
        public int Id { get; set; }

        public string Nome { get; set; }
        public Estado Estado { get; set; }
    }
}

