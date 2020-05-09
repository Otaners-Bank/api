using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using api.Models.Google;
using System.Net;

namespace api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DocumentationController : ControllerBase
    {
        // GET - /Clients = The inicial page of API
        [EnableCors("AllowMyOrigin")]
        [HttpGet]
        public ContentResult Home()
        {
            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = "<html style=\"background-color: #252630;\">" +
                "<head> <meta charset=\"UTF-8\">" +
                "</head>" +
                "<body style=\"text-align:center; padding:0; margin:0;\">" +
                "<h1 style=\"margin-top: 5%; color:white;\">Otaner's Bank API</h1>" +
                "</body>" +
                "</html>"
            };
        }

    }
}