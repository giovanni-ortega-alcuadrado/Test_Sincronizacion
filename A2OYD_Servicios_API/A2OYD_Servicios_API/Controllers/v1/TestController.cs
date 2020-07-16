using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2OYD_Servicios_API.Controllers
{
    [Route("A2/v1/ProbarServicio")]
    [ApiController]
    public class TestController:ControllerBase
    {
        [HttpGet]
        public ActionResult Get_ProbarServicio()
        {
            return Ok("El servicio responde");
        }

    }
}
