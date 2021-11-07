using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConhecaApi.Models
{
    /** 
     * Singleton Pattern 
     */

    sealed public class StatusUsuario
    {
        public int Codigo { get; }
        public string Descricao { get; }

        static public IList<StatusUsuario> lista = new List<StatusUsuario>();

        static public StatusUsuario ATIVO   = new StatusUsuario(1, "Ativo");
        static public StatusUsuario INATIVO = new StatusUsuario(2, "Inativo");

        private StatusUsuario(int codigo, string descricao)
        {
            Codigo = codigo;
            Descricao = descricao;
            lista.Add(this);
        }


        static public StatusUsuario getInstance(int codigo)
        {
            foreach (StatusUsuario s in lista)
            {
                if (s.Codigo == codigo)
                {
                    return s;
                }
            }
            return null;
        }


        public override string ToString()
        {
            return $"{Codigo} - {Descricao}";
        }

    }
}
