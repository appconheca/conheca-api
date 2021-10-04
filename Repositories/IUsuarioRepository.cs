using ConhecaApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConhecaApi.Repositories
{
    public interface IUsuarioRepository
    {
        List<Usuario> Read();

        Usuario Read(string id);

        void Create(Usuario model);

        void Update(string id, Usuario model);

        void Delete(string id);
    }
}
