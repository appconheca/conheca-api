using ConhecaApi.Models;
using ConhecaApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConhecaApi.Controllers
{
    [ApiController]
    [Route("categoria")]
    public class CategoriaController : ControllerBase
    {        
        [HttpPost]
        public void Create([FromServices] IRepository<Categoria> repository, Categoria model)
        {
            repository.Create(model);
        }


        [HttpPut("{id}")]
        public void Update([FromServices] IRepository<Categoria> repository, int id, Categoria model)
        {
            repository.Update(id, model);
        }

        [HttpDelete("{id}")]
        public void Delete([FromServices] IRepository<Categoria> repository, int id)
        {
            repository.Delete(id);
        }


        [HttpGet]
        public List<Categoria> Read([FromServices] IRepository<Categoria> repository)
        {
            return repository.Read();
        }


        [HttpGet("{id}")]
        public Categoria Read([FromServices] IRepository<Categoria> repository, int id)
        {
            return repository.Read(id);
        }


    }
}
