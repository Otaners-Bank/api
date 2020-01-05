using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [EnableCors("AllowMyOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        // GET api/Client
        [EnableCors("AllowMyOrigin")]
        [HttpGet]
        public string Get()
        {
            return "GET";

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