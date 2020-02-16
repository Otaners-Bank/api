﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using api.Models.Admins;
using api.Models;
using System.Net.Mail;
using System.Net;

namespace api.Controllers
{
    [EnableCors("AllowMyOrigin")]
    [Route("[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private AdminsRepository _repository = new AdminsRepository();

        // Admin Methods
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
                    NotifyAdmin(_repository.SearchAdmin(adm.CPF), "We inform that someone just logged on your account!");
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

        // GET - Search/CPF = Search a admin by CPF
        [EnableCors("AllowMyOrigin")]
        [HttpGet("Search/{CPF}")]
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

        // PUT - /Admins/Update/CPF + body = Update a admin
        [EnableCors("AllowMyOrigin")]
        [HttpPut("Update/{CPF}")]
        public IActionResult UpdateAdmin([FromBody] Admin admin, string CPF)
        {
            string actionResult = _repository.UpdateAdmin(CPF, admin);

            if (actionResult == "200")
            {
                NotifyAdmin(admin, "We inform that your account has been sucessfuly updated!");
                return Ok("Updates sucessfuly applied");
            }
            else if (actionResult == "404")
            {
                return StatusCode(404, "Admin not found");
            }
            return StatusCode(500, "An error occurred");
        }

        // Clients Methods
        // POST - /Admins/Clients/Insert + body = insert a new active client
        [EnableCors("AllowMyOrigin")]
        [HttpPost("Clients/Insert")]
        public IActionResult InsertClient([FromBody] Client client)
        {
            client.STATUS = "1";
            string actionResult = _repository.Admin_RegisterClient(client);

            if (actionResult == "200")
            {
                NotifyClient(client, "An Admin created a account to you, Welcome to OTANER'S BANK !!!");
                return Ok("Sucessfuly registered");
            }
            else if (actionResult == "400")
            {
                return StatusCode(400, "Client already registered");
            }
            return StatusCode(500, "An error occurred");
        }

        // PUT - /Admins/Clients/Update/CPF + body = update a active client
        [EnableCors("AllowMyOrigin")]
        [HttpPut("Clients/Update/{CPF}")]
        public IActionResult UpdateClient([FromBody] Client client, string CPF)
        {
            string actionResult = _repository.Admin_UpdateClient(CPF, client);

            if (actionResult == "403")
            {
                return StatusCode(403, "This account is inactived, and must be active to be update");
            }
            else if (actionResult == "200")
            {
                NotifyClient(client, "We inform that an Admin updated your info account !");
                return Ok("Updates sucessfuly applied");
            }
            else if (actionResult == "404")
            {
                return StatusCode(404, "Client not found");
            }
            return StatusCode(500, "An error occurred");
        }

        // GET - /Admins/Clients/Search/CPF = search a client by CPF
        [EnableCors("AllowMyOrigin")]
        [HttpGet("Clients/Search/{CPF}")]
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

        // GET /Admins/Clients/Search/All = search all clients
        [EnableCors("AllowMyOrigin")]
        [HttpGet("Clients/Search/All")]
        public IActionResult SearchAllClients()
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


        // Active Clients Methods
        // GET /Admins/Clients/Active/Search/All = search all active clients
        [EnableCors("AllowMyOrigin")]
        [HttpGet("Clients/Active/Search/All")]
        public IActionResult SearchActiveClients()
        {
            var actionResult = _repository.Admin_SearchActiveClients();
            if (actionResult == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(actionResult);
            }
        }

        // GET - /Clients/Active/CountTotal = count the total of active clients inserted
        [EnableCors("AllowMyOrigin")]
        [HttpGet("Clients/Active/CountTotal")]
        public IActionResult CountActiveClientsAccounts()
        {
            try
            {
                return StatusCode(200, _repository.Admin_CountActiveClientsAccounts());
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred");
            }
        }

        // DELETE - /Clients/ActiveClient/CPF = active a client by CPF
        [EnableCors("AllowMyOrigin")]
        [HttpDelete("Clients/ActiveClient/{CPF}")]
        public IActionResult ActiveClient(string CPF)
        {
            try
            {
                string actionResult = _repository.Admin_ActiveClient(CPF);

                if (actionResult == "200")
                {
                    NotifyClient(_repository.Admin_SearchClient(CPF), "We inform that an Admin has activated your account !");
                    return Ok("Client successfuly activated");
                }
                else if (actionResult == "404")
                {
                    return StatusCode(404, "Client not found");
                }
                else
                {
                    return StatusCode(500, "An error occurred");
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred");
            }
        }


        // Inactive Clients Methods
        // GET /Admins/Clients/Active/Search/All = search all active clients
        [EnableCors("AllowMyOrigin")]
        [HttpGet("Clients/Inactive/Search/All")]
        public IActionResult SearchInactiveClients()
        {
            var actionResult = _repository.Admin_SearchInactiveClients();
            if (actionResult == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(actionResult);
            }
        }

        // GET - /Clients/Active/CountTotal = count the total of inactive clients inserted
        [EnableCors("AllowMyOrigin")]
        [HttpGet("Clients/Inactive/CountTotal")]
        public IActionResult CountInactiveClientsAccounts()
        {
            try
            {
                return StatusCode(200, _repository.Admin_CountInactiveClientsAccounts());
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred");
            }
        }

        // DELETE - /Clients/InactiveClient/CPF = inactive a client by CPF
        [EnableCors("AllowMyOrigin")]
        [HttpDelete("Clients/InactiveClient/{CPF}")]
        public IActionResult InactiveClient(string CPF)
        {
            try
            {
                string actionResult = _repository.Admin_InactiveClient(CPF);

                if (actionResult == "200")
                {
                    NotifyClient(_repository.Admin_SearchClient(CPF), "We inform that an Admin has inactivated your account !");
                    return Ok("Client successfuly inactivated");
                }
                else if (actionResult == "404")
                {
                    return StatusCode(404, "Client not found");
                }
                else
                {
                    return StatusCode(500, "An error occurred");
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred");
            }
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

        public static void NotifyAdmin(Admin admin, string message)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("otaner.bank@gmail.com");
                    mail.To.Add(admin.EMAIL);
                    mail.Subject = "Otaner's Bank - Alert for your Admin Account - " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    mail.Body = "<h1 style=\"text-align:center;\">Hello " + admin.NAME + "</h1>" +
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
