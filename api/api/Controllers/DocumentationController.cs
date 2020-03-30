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
                "<body style=\"text-align:center; padding:0; margin:0;\">" +
                "<iframe src=\"https://thaleslj.github.io/OtanersBank/\" style =\"border:none; height:100%; width: 100%; \"></iframe>" +
                "</head>" +
                "</body>" +
                "</html>"
            };
        }

    }
}