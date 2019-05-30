using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiForAngular.Dtos
{
    public class CityListDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public string PhotoUrl { get; set; }
    }
}
