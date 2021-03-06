
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
    //[Authorize]    
    //[Route("[controller]")]
    [ApiController]
    [Route("usuario")]
    public class UsuarioController : ControllerBase
    {
        [HttpGet]
        public List<Usuario> Read([FromServices] IRepository<Usuario> repository)
        {
            return repository.Read();
        }


        [HttpGet("{id}")]
        public Usuario Read([FromServices] IRepository<Usuario> repository, int id)
        {
            return repository.Read(id);
        }


        [HttpPost]
        public void Create([FromServices] IRepository<Usuario> repository, Usuario model)
        {
            repository.Create(model);
        }


        [HttpPut("{id}")]
        public void Update([FromServices] IRepository<Usuario> repository, int id, Usuario model)
        {
            repository.Update(id, model);
        }

        [HttpDelete("{id}")]
        public void Delete([FromServices] IRepository<Usuario> repository, int id)
        {
            repository.Delete(id);
        }
    }
}
