using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [EnableCors("AllowMyOrigin")]
    [Route("[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private ClientsRepository _repository = new ClientsRepository();

        // GET Client/count
        [EnableCors("AllowMyOrigin")]
        [HttpGet("count")]
        public IActionResult Get_CountClients()
        {
            return Ok((_repository.countClients()).ToString());
        }

        // GET Clients
        [EnableCors("AllowMyOrigin")]
        [HttpGet]
        public IActionResult Get_Clients()
        {
            var actionResult = _repository.listClients();
            if (actionResult == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(actionResult);
            }
        }

        // GET Client/CPF
        [EnableCors("AllowMyOrigin")]
        [HttpGet("{CPF}")]
        public IActionResult Get_Client(string CPF)
        {
            var actionResult = _repository.listClient(CPF);
            if (actionResult == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(actionResult);
            }
        }

        // POST api/Client
        [EnableCors("AllowMyOrigin")]
        [HttpPost]
        public string Post()
        {
            return "POST";

        }

        // PUT api/Client
        [EnableCors("AllowMyOrigin")]
        [HttpPut]
        public string Put()
        {
            return "PUT";

        }

        // DELETE api/Client
        [EnableCors("AllowMyOrigin")]
        [HttpDelete]
        public string Delete()
        {
            return "DELETE";

        }

    }
}