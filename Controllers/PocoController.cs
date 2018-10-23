using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreModelBinderDemo.Controllers
{
    [Route("api/pocos")]
    [ApiController]
    public class PocoController : ControllerBase
    {    
        [HttpGet("")]
        public ActionResult<ActionResult<IList<MyPoco>>> GetAll()
        {
            return Ok(MyPocoRepository.Pocos);
        }
  
        [HttpGet("{id}")]
        public ActionResult<ActionResult<MyPoco>> Get(MyPoco poco)
        {
            if (poco is null)
            {
                return NotFound();
            }

            return Ok(poco);
        }

    }
}
