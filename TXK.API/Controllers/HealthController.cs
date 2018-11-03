using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TXK.API.Filter;

namespace TXK.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TimingFilter]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// Consul健康检查接口，默认返回OK
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "OK";
        }
    }
}