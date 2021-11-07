using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConhecaApi.Models
{
    public class Usuario : IDomainObject
    {
        public Usuario()
        {
            Status = StatusUsuario.ATIVO.Codigo;
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public int Status { get; set; }
        public string Senha { get; set; }
        public string Email { get; set; }
        public DateTime DataNascimento { get; set; }


        public override bool Equals(object obj)
        {
            return obj is Usuario usuario &&
                    Id == usuario.Id;
        }

        
    }

    
}
