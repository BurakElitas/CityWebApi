using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiForAngular.Data;
using WebApiForAngular.Models;

namespace WebApiForAngular.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        DataContext _context;
        IRepository _repo;
        public ValuesController(DataContext context,IRepository repo)
        {
            _context = context;
            _repo = repo;

        }
        // GET api/values
        [HttpGet]
        public JsonResult Get()
        {

          List<User> users = _repo.List<User>();
          return Json(users);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
