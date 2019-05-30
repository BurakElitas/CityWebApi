using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiForAngular.Data;
using WebApiForAngular.Dtos;
using WebApiForAngular.Models;

namespace WebApiForAngular.Controllers
{
    [Produces("application/json")]
    [Route("api/Cities")]
    public class CitiesController : Controller
    {
        IRepository _repo;
        IMapper _mapper;
        public CitiesController(IRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult GetCities()
        {
            var cities = _repo.ListQueryable<City>().Include(c => c.Photos).ToList();
            var CityForListDto = _mapper.Map<List<CityListDto>>(cities);
            return Ok(CityForListDto);
        }

        [Route("Add")]
        [HttpPost]
        public ActionResult Add([FromBody]City city)
        {
            _repo.Insert<City>(city);
            return Ok(city);
        }

        [HttpGet]
        [Route("detail")]
        public IActionResult GetCityById(int id)
        {
            var city = _repo.ListQueryable<City>().Include(c => c.Photos).FirstOrDefault(x => x.Id == id);

            var cityForDto = _mapper.Map<CityForDto>(city);
            return Ok(cityForDto);
        }

        [HttpGet]
        [Route("photos")]
        public IActionResult GetPhotos(int cityid)
        {
            var photos = _repo.List<Photo>(x => x.CityId == cityid);
            return Ok(photos);

        }
        
    }
}