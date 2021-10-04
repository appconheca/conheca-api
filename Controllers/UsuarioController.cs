
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConhecaApi.Models;
using ConhecaApi.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace ConhecaApi.Controllers
{
    [Authorize]    
    //[Route("[controller]")]
    [ApiController]
    [Route("usuario")]
    public class UsuarioController : ControllerBase
    {
        // GET http://localhost:5000/usuario
        [HttpGet]
        public List<Usuario> Read([FromServices] IUsuarioRepository repository)
        {
            return repository.Read();
        }


        [HttpGet("{id}")]
        public Usuario Read([FromServices] IUsuarioRepository repository, string id)
        {
            return repository.Read(id);
        }


        [HttpPost]
        public void Create([FromServices] IUsuarioRepository repository, Usuario model)
        {
            repository.Create(model);
        }

        //PUT http://localhost:5000/usuario/0000-0000-0000-00000
        [HttpPut("{id}")]
        public void Update([FromServices] IUsuarioRepository repository, string id, Usuario model)
        {
            repository.Update(id, model);
        }

        [HttpDelete("{id}")]
        public void Delete([FromServices] IUsuarioRepository repository, string id)
        {
            repository.Delete(id);
        }
    }
}
