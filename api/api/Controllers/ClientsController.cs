using api.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Mail;

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
        public ContentResult Home()
        {
            // NotifyClient(new Client { NAME = "Thales", EMAIL = "thaleslimadejesus@gmail.com" }, "Teste");
            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = "<html style=\"background-color: #252630;\">" +
                "<body style=\"text-align:center; margin-top:1%;\">" +
                "<h2 style=\"color:white;\">Visit our main site in:</h2>" +
                "<a style=\"color:white;\" href=\"https://thaleslj.github.io/otanersbank/ \">Otaner's Bank</a>" +
                "</body>" +
                "</html>"
            };
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
                    NotifyClient(client, "We inform that someone just logged on your account!");
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
                NotifyClient(client, "Account successfuly created, Welcome to OTANER'S BANK !!!");
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
                NotifyClient(client, "We inform that your account has been sucessfuly updated!");
                return Ok("Updates sucessfuly applied");
            }
            else if (actionResult == "404")
            {
                return StatusCode(404, "Client not found");
            }
            return StatusCode(500, "An error occurred");
        }


        public static void NotifyClient(Client client, string message)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("otaner.bank@gmail.com");
                    mail.To.Add(client.EMAIL);
                    mail.Subject = "Otaner's Bank - Alert for account " + client.ACCOUNT + " - " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    mail.Body = "<h1 style=\"text-align:center;\">Hey " + client.NAME + "</h1>" +
                        "<p style=\"text-align:center;\">" + message + "</p>";
                    mail.IsBodyHtml = true;
                    // mail.Attachments.Add(new Attachment("C:\\file.zip"));

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential("otaner.bank@gmail.com", "otaner@2020");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
            }
            catch (Exception)
            {
                
            }
        }

    }
}