using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConhecaApi.Models
{
    public class PontoTuristicoCategoriaId
    {
        public int PontoTuristicoId { get; set; }
        public int CategoriaId      { get; set; }


        public PontoTuristicoCategoriaId() { }

        public PontoTuristicoCategoriaId(int pontoId, int categoriaId)
        {
            PontoTuristicoId = pontoId;
            CategoriaId = categoriaId;
        }


        public override string ToString()
        {
            return $"PontoId:{PontoTuristicoId} - CategoriaId:{CategoriaId}";
        }

        public override bool Equals(object obj)
        {
            return obj is PontoTuristicoCategoriaId id &&
                   PontoTuristicoId == id.PontoTuristicoId &&
                   CategoriaId == id.CategoriaId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PontoTuristicoId, CategoriaId);
        }


    }
}
