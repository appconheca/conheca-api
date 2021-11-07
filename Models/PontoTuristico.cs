using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConhecaApi.Models
{
    public class PontoTuristico : IDomainObject
    {
        public int Id { get; set; }

        public string Nome { get; set; }
        public string Descricao { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Data { get; set; }
        public Usuario Usuario { get; set; }
        public Endereco Endereco { get; set; }
        public List<PontoTuristicoCategoria> Categorias { get; set; }

        public PontoTuristico()
        {
            Data = DateTime.Now;
            Categorias = new List<PontoTuristicoCategoria>();
        }


        public void RemoverCategoria(Categoria categoria)
        {
            IEnumerator<PontoTuristicoCategoria> enumerator = Categorias.GetEnumerator();
            while (enumerator.MoveNext())
            {
                PontoTuristicoCategoria pc = enumerator.Current;

                if (pc.PontoTuristico.Equals(this) && pc.Categoria.Equals(categoria))
                {
                    // Remove da coleção de combustíveis
                    enumerator.Dispose();
                    // Garbage Collector - recolher memória
                    pc.PontoTuristico = null;
                    pc.Categoria = null;
                }
            }
        }


        public override string ToString()
        {
            return $"ponto_id: {Id}";
        }


        public override bool Equals(object obj)
        {
            return obj is PontoTuristico ponto &&
                Id == ponto.Id;
        }


        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

    }
}
