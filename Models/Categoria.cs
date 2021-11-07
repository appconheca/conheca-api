using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConhecaApi.Models
{
    public class Categoria : IDomainObject
    {
        public int Id { get; set; }
        public string Tipo { get; set; }


        public override string ToString()
        {
            return $"{Id} - {Tipo}";
        }

        public override bool Equals(object obj)
        {
            return obj is Categoria categoria &&
                    Id == categoria.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

    }
}
