using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApiForAngular.Data;
using WebApiForAngular.Dtos;
using WebApiForAngular.Helpers;
using WebApiForAngular.Models;

namespace WebApiForAngular.Controllers
{
    [Produces("application/json")]
    [Route("api/cities/{cityid}/photos")]
    public class PhotosController : Controller
    {
        IMapper _mapper;
        IRepository _repo;
        IOptions<CloudinarySetting> _cloudinaryConfig;

        Cloudinary _cloudinary;
        public PhotosController(IMapper mapper,IRepository repo,IOptions<CloudinarySetting> cloudinaryConfig)
        {
            _mapper = mapper;
            _repo = repo;
            _cloudinaryConfig = cloudinaryConfig;

            Account account = new Account
            {
                Cloud = _cloudinaryConfig.Value.Name,
                ApiKey = cloudinaryConfig.Value.ApiKey,
                ApiSecret = cloudinaryConfig.Value.ApiSecret
            };

            this._cloudinary= new Cloudinary(account);
        }

        public IActionResult AddPhotoForCity(int cityId,[FromForm]PhotoForCreationDto photoForCreationDto)
        {
            City city = _repo.Find<City>(x => x.Id == cityId);
            if (city == null)
                return BadRequest("Could not fint the city");

            var currentUserId =int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);


            if(currentUserId!=city.UserId)
            {
                return Unauthorized();
            }

            var File = photoForCreationDto.File;
            var uploadResult = new ImageUploadResult();

            if (File.Length>0)
            {
                using (var stream = File.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(File.Name,stream)
                    };

                     uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

           var photo=_mapper.Map<Photo>(photoForCreationDto);
            photo.City = city;

            if(!city.Photos.Any(x=>x.IsMain==true))
            {
                photo.IsMain = true;
            }
            city.Photos.Add(photo);

            if(_repo.Save()>0)
            {
                PhotoForReturnDto photoForReturn = _mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute("GetPhoto", new { id = photo.Id }, photoForReturn);
            }

            return BadRequest("Could not add the photo");
            

        }

        [HttpGet("{id}",Name ="GetPhoto")]
        public IActionResult GetPhoto(int id)
        {
            Photo photo = _repo.Find<Photo>(x => x.Id == id);
            PhotoForReturnDto photoForReturn = _mapper.Map<PhotoForReturnDto>(photo);
            return Ok(photoForReturn);

        }
    }
}