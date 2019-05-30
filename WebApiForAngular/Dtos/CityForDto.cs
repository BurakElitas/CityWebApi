using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiForAngular.Models;

namespace WebApiForAngular.Dtos
{
    public class CityForDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public List<Photo> Photos { get; set; }
    }
}
