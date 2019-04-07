using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
   
    
     
    [Route ("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
           _context = context;
        }

        [HttpGet ("{id:int}", Name = "GetById")]//This method should have an `HttpGet` attribute with an value of `"{id:int}"` and the `Name` property set to `"GetById"`.
        //[Name]
        public IActionResult GetById(int id)//Create a new method `GetById`.This method should accept a parameter of type `int` named `id`.This method should have a return type of `IActionResult` 
        {
            var celestialObject = _context.CelestialObjects.Find(id);//This method should return `NotFound` there is no `CelestialObject` with an `Id` property that matches the parameter
            if (celestialObject == null)
                return NotFound();
            celestialObject.Satellites = _context.CelestialObjects.Where(CelestialObjects => celestialObject.OrbitedObjectId == id).ToList(); //This method should also set the `Satellites` property to any `CelestialObjects` who's `OrbitedObjectId` is the current `CelestialObject`'s `Id`.
           return Ok(celestialObject);//This method should return an `Ok` with a value of the `CelestialObject` who's `Id` property matches the `id` parameter.
        }

        //Create the `GetByName` method
    
        [HttpGet ("{name}")]//This method should have an `HttpGet` attribute with a value of `"{name}"`.
        public IActionResult GetByName(string name)//This method should have a return type of `IActionResult`.This method should accept a parameter of type `string` named `name`. 
        {
            var celestialObjects = _context.CelestialObjects.Where(CelestialObjects => CelestialObjects.Name == name).ToList();
            if (!celestialObjects.Any())
                return NotFound();//This method should return `NotFound` there is no `CelestialObject` with an `Name` property that matches the `name` parameter.
            foreach(var celestialObject in celestialObjects)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(CelestialObjects =>  celestialObject.OrbitedObjectId == celestialObject.Id).ToList(); //This method should also set the `Satellites` property for each `CelestialObject` who's `OrbitedObjectId` is the current `CelestialObject`'s `Id`.
            }
              return Ok(celestialObjects);//This method should return an `Ok` with a value of the list of `CelestialObject` who's `Name` property matches the `name` parameter
        }

       
        [HttpGet] //This method should have an `HttpGet` attribute.
        public IActionResult GetAll() //Create the `GetAll` method. This method should have a return type of `IActionResult`.
        {
            var celestialObjects = _context.CelestialObjects.ToList();
            foreach(var celestialObject in celestialObjects)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(CelestialObject => celestialObject.OrbitedObjectId == celestialObject.Id).ToList();//This method should also set the `Satellites` property for each of the `CelestialObject`s returned.
            }
            return Ok(celestialObjects);// This method should return `Ok` with a value of all `CelestialObjects`s.
        }

        /*
         [ ] Create `CelestialObjectControllers`'s Post, Put, Patch, and Delete actions
  - [ ] Create the `Create` method
    - This method should have a return type of `IActionResult` .
    - This method should accept a parameter of type `[FromBody]CelestialObject`. (Note: You will need to add a `using` directive for `StarChart.Models`) 
    - This method should have an `HttpPost` attribute. 
    - This method should add the provided `CelestialObject` to the `CelestialObjects` `DbSet` then `SaveChanges`.
    - This method should return a `CreatedAtRoute` with the arguments 
      - `"GetById"`
      - A new `object` with an `id` of the `CelestialObject`'s `Id` (note: use the `new { }` format)
      - The newly created `CelestialObject`.
         */
    }
}
