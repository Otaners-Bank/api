using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using api.Models.Admins;
using api.Models;

namespace api.Controllers
{
    [EnableCors("AllowMyOrigin")]
    [Route("[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private AdminsRepository _repository = new AdminsRepository();

        // POST /Admins/Login + body = Login Method
        [EnableCors("AllowMyOrigin")]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] Admin adm)
        {
            try
            {
                string PASSWORD = "";

                try
                {
                    PASSWORD = _repository.SearchAdmin(adm.CPF).PASSWORD;
                    if (PASSWORD == "" || PASSWORD == string.Empty)
                    {
                        return StatusCode(404, "Account not found");
                    }
                }
                catch (Exception)
                {
                    return StatusCode(404, "Account not found");
                }


                if (PASSWORD == adm.PASSWORD)
                {
                    return Ok("Welcome");
                }
                else
                {
                    return StatusCode(400, "Invalid Credentials");
                }

            }
            catch (Exception)
            {
                return StatusCode(500, "We had an unknown error");
            }
        }

        // GET - /CPF = search a admin by CPF
        [EnableCors("AllowMyOrigin")]
        [HttpGet("{CPF}")]
        public IActionResult SearchAdmin(string CPF)
        {
            var actionResult = _repository.SearchAdmin(CPF);
            if (actionResult == null)
            {
                return StatusCode(404, "Admin not found");
            }
            else
            {
                return Ok(actionResult);
            }
        }


        // PUT - /Admins/Clients/CPF + body = update a client
        [EnableCors("AllowMyOrigin")]
        [HttpPut("{CPF}")]
        public IActionResult UpdateAdmin([FromBody] Admin admin, string CPF)
        {
            string actionResult = _repository.UpdateAdmin(CPF, admin);

            if (actionResult == "200")
            {
                return Ok("Updates sucessfuly applied !");
            }
            else if (actionResult == "404")
            {
                return StatusCode(404, "Admin not found");
            }
            return StatusCode(500, "An error occurred");
        }



        // GET /Admins/Clients = search all clients
        [EnableCors("AllowMyOrigin")]
        [HttpGet("Clients")]
        public IActionResult SearchClients()
        {
            var actionResult = _repository.Admin_SearchClients();
            if (actionResult == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(actionResult);
            }
        }


        // GET - /Clients/CPF = search a client by CPF
        [EnableCors("AllowMyOrigin")]
        [HttpGet("Clients/{CPF}")]
        public IActionResult SearchClient(string CPF)
        {
            var actionResult = _repository.Admin_SearchClient(CPF);
            if (actionResult == null)
            {
                return StatusCode(404, "Client not found");
            }
            else
            {
                return Ok(actionResult);
            }
        }


        // POST - /Admins/Client + body = insert a new client
        [EnableCors("AllowMyOrigin")]
        [HttpPost("Clients")]
        public IActionResult InsertClient([FromBody] Client client)
        {
            string actionResult = _repository.Admin_RegisterClient(client);

            if (actionResult == "200")
            {
                return Ok("Sucessfuly registered !");
            }
            else if (actionResult == "400")
            {
                return StatusCode(400, "Client already registered");
            }
            return StatusCode(500, "An error occurred");
        }

        // PUT - /Admins/Clients/CPF + body = update a client
        [EnableCors("AllowMyOrigin")]
        [HttpPut("Clients/{CPF}")]
        public IActionResult UpdateClient([FromBody] Client client, string CPF)
        {
            string actionResult = _repository.Admin_UpdateClient(CPF, client);

            if (actionResult == "200")
            {
                return Ok("Updates sucessfuly applied !");
            }
            else if (actionResult == "404")
            {
                return StatusCode(404, "Client not found");
            }
            return StatusCode(500, "An error occurred");
        }

    }
}
