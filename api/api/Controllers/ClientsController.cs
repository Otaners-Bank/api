using api.Models;
using Microsoft.AspNetCore.Cors;
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
            return Ok(_repository.CountClients());
        }

        // GET Clients = search all clients
        [EnableCors("AllowMyOrigin")]
        [HttpGet]
        public IActionResult Get_Clients()
        {
            var actionResult = _repository.ListClients();
            if (actionResult == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(actionResult);
            }
        }

        // GET Client/CPF = search a client by CPF
        [EnableCors("AllowMyOrigin")]
        [HttpGet("{CPF}")]
        public IActionResult Get_Client(string CPF)
        {
            var actionResult = _repository.ListClient(CPF);
            if (actionResult == null)
            {
                return Ok("Client doens't exists");
            }
            else
            {
                return Ok(actionResult);
            }
        }

        // POST Client + body = insert a new client
        [EnableCors("AllowMyOrigin")]
        [HttpPost]
        public IActionResult Post([FromBody] Client client)
        {
            string actionResult = _repository.InsertClient(client);
            return Ok(actionResult);
        }

        // PUT Client/CPF + body = update a client
        [EnableCors("AllowMyOrigin")]
        [HttpPut("{CPF}")]
        public IActionResult Put([FromBody] Client client, string CPF)
        {
            string actionResult = _repository.UpdateClient(CPF, client);
            return Ok(actionResult);
        }

        // DELETE Client/CPF = delete a client
        [EnableCors("AllowMyOrigin")]
        [HttpDelete("{CPF}")]
        public IActionResult Delete(string CPF)
        {
            string actionResult = _repository.DeleteClient(CPF);
            return Ok(actionResult);
        }

    }
}