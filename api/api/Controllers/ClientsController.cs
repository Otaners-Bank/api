using api.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;

namespace api.Controllers
{
    [EnableCors("AllowMyOrigin")]
    [Route("[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private ClientsRepository _repository = new ClientsRepository();

        // GET - /Clients = The inicial page of API
        [EnableCors("AllowMyOrigin")]
        [HttpGet]
        public string Home()
        {
            return "Visit our main site in https://thaleslj.github.io/otanersbank/";
        }

        // POST /Clients/Login + body = Login Method only for active accounts
        [EnableCors("AllowMyOrigin")]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] Client client)
        {
            try
            {
                string PASSWORD = "";

                try
                {
                    Client cli = _repository.SearchAccount(client.ACCOUNT);
                    PASSWORD = cli.PASSWORD;

                    if (cli.STATUS == "0")
                    {
                        return StatusCode(403, "This account is inactived");
                    }

                    if (PASSWORD == "" || PASSWORD == string.Empty)
                    {
                        return StatusCode(404, "Account not found");
                    }

                }
                catch (Exception)
                {
                    return StatusCode(404, "Account not found");
                }


                if (PASSWORD == client.PASSWORD)
                {
                    return StatusCode(200, "Welcome");
                }
                else
                {
                    return StatusCode(403, "Invalid Credentials");
                }

            }
            catch (Exception)
            {
                return StatusCode(500, "We had an unknown error");
            }
        }

        // GET - /Clients/CountTotal = Count the total of clients inserted
        [EnableCors("AllowMyOrigin")]
        [HttpGet("CountTotal")]
        public IActionResult CountClientsAccounts()
        {
            try
            {
                return StatusCode(200, _repository.CountClientsAccounts());
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred");
            }
        }

        // GET - /Clients/Search/CPF = Search a active client by CPF
        [EnableCors("AllowMyOrigin")]
        [HttpGet("Search/{CPF}")]
        public IActionResult SearchClient(string CPF)
        {
            var actionResult = _repository.SearchClient(CPF);
            if (actionResult == null)
            {
                return StatusCode(404, "Client not found");
            }
            else if (actionResult.STATUS == "0")
            {
                return StatusCode(403, "This account is inactived");
            }
            else
            {
                return StatusCode(200, actionResult);
            }
        }

        // POST - /Clients/Register + body = Insert a new client active
        [EnableCors("AllowMyOrigin")]
        [HttpPost("Register")]
        public IActionResult InsertClient([FromBody] Client client)
        {
            client.STATUS = "1";
            string actionResult = _repository.RegisterClient(client);

            if (actionResult == "200")
            {
                return Ok("Sucessfuly registered");
            }
            else if (actionResult == "400")
            {
                return StatusCode(400, "Client already registered");
            }
            return StatusCode(500, "An error occurred");
        }

        // PUT - /Client/Update/CPF + body = Update a active client
        [EnableCors("AllowMyOrigin")]
        [HttpPut("Update/{CPF}")]
        public IActionResult UpdateClient([FromBody] Client client, string CPF)
        {
            string actionResult;

            if (_repository.SearchClient(CPF).STATUS == "0")
            {
                return StatusCode(403, "This account is inactived");
            }

            actionResult = _repository.UpdateClient(CPF, client);

            if (actionResult == "200")
            {
                return Ok("Updates sucessfuly applied");
            }
            else if (actionResult == "404")
            {
                return StatusCode(404, "Client not found");
            }
            return StatusCode(500, "An error occurred");
        }

    }
}