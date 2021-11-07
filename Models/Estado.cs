using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConhecaApi.Models
{
    public class Estado : IDomainObject
    {
        public string Uf { get; set; }
        public string Nome { get; set; }
    }
}
