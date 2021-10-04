using ConhecaApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConhecaApi.Repositories
{
    public class UsuarioMemoryRepository : IUsuarioRepository
    {
        List<Usuario> usuarios = new List<Usuario>();


        public List<Usuario> Read()
        {
            return usuarios;
        }

        public Usuario Read(string id)
        {
            Usuario usuario = usuarios.SingleOrDefault(u => u.Id == id);

            return usuario;
        }

        public void Create(Usuario model)
        {
            usuarios.Add(model);
        }

        public void Update(string id, Usuario model)
        {
            var usuario = this.usuarios.SingleOrDefault(u => u.Id == id);
            //TODO          
        }

        public void Delete(string id)
        {
            var usuario = this.usuarios.SingleOrDefault(u => u.Id == id);
            usuarios.Remove(usuario);
        }


    }
}
