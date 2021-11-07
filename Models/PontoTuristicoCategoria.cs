using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConhecaApi.Models
{
    public class PontoTuristicoCategoria
    {
        public PontoTuristicoCategoriaId Pk { get; set; }
        public PontoTuristico PontoTuristico { get; set; }
        public Categoria Categoria { get; set; }
    
        public PontoTuristicoCategoria() { }
        
        public PontoTuristicoCategoria(PontoTuristico pontoTuristico, Categoria categoria) 
        {
            PontoTuristico = pontoTuristico ?? throw new ArgumentNullException(nameof(pontoTuristico));
            Categoria = categoria ?? throw new ArgumentNullException(nameof(Categoria));

            this.Pk = new PontoTuristicoCategoriaId(pontoTuristico.Id, categoria.Id);
        }


        // Override's Methods     ---------------------------------------------------------------------------

        public override string ToString()
        {
            return this.Pk.ToString();
        }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;

            if (obj == null || !(obj is PontoTuristicoCategoria))
            {
                return false;
            }

            PontoTuristicoCategoria that = (PontoTuristicoCategoria)obj;
            return this.PontoTuristico.Equals(that.PontoTuristico) &&
                   this.Categoria.Equals(that.Categoria);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PontoTuristico, Categoria);
        }
    }



}
