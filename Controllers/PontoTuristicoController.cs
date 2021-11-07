
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
    [Route("pontoturistico")]
    public class PontoTuristicoController : ControllerBase
    {        
        [HttpGet]
        public List<PontoTuristico> Read([FromServices] IRepository<PontoTuristico> repository)
        {
            return repository.Read();
        }


        [HttpGet("{id}")]
        public PontoTuristico Read([FromServices] IRepository<PontoTuristico> repository, int id)
        {
            return repository.Read(id);
        }


        [HttpPost]
        public void Create([FromServices] IRepository<PontoTuristico> repository, PontoTuristico model)
        {
            repository.Create(model);
        }
                
        [HttpPut("{id}")]
        public void Update([FromServices] IRepository<PontoTuristico> repository, int id, PontoTuristico model)
        {
            repository.Update(id, model);
        }

        [HttpDelete("{id}")]
        public void Delete([FromServices] IRepository<PontoTuristico> repository, int id)
        {
            repository.Delete(id);
        }
    }
}
