using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{



    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]//This method should have an `HttpGet` attribute with an value of `"{id:int}"` and the `Name` property set to `"GetById"`.
        //[Name]
        public IActionResult GetById(int id)//Create a new method `GetById`.This method should accept a parameter of type `int` named `id`.This method should have a return type of `IActionResult` 
        {
            var celestialObject = _context.CelestialObjects.Find(id);//This method should return `NotFound` there is no `CelestialObject` with an `Id` property that matches the parameter
            if (celestialObject == null)
                return NotFound();
            celestialObject.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == id).ToList(); //This method should also set the `Satellites` property to any `CelestialObjects` who's `OrbitedObjectId` is the current `CelestialObject`'s `Id`.
            return Ok(celestialObject);//This method should return an `Ok` with a value of the `CelestialObject` who's `Id` property matches the `id` parameter.
        }

        //Create the `GetByName` method

        [HttpGet("{name}")]//This method should have an `HttpGet` attribute with a value of `"{name}"`.
        public IActionResult GetByName(string name)//This method should have a return type of `IActionResult`.This method should accept a parameter of type `string` named `name`. 
        {
            var celestialObjects = _context.CelestialObjects.Where(e => e.Name == name).ToList();
            if (!celestialObjects.Any())
                return NotFound();//This method should return `NotFound` there is no `CelestialObject` with an `Name` property that matches the `name` parameter.
            foreach (var celestialObject in celestialObjects)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == celestialObject.Id).ToList(); //This method should also set the `Satellites` property for each `CelestialObject` who's `OrbitedObjectId` is the current `CelestialObject`'s `Id`.
            }
            return Ok(celestialObjects);//This method should return an `Ok` with a value of the list of `CelestialObject` who's `Name` property matches the `name` parameter
        }


        [HttpGet] //This method should have an `HttpGet` attribute.
        public IActionResult GetAll() //Create the `GetAll` method. This method should have a return type of `IActionResult`.
        {
            var celestialObjects = _context.CelestialObjects.ToList();
            foreach (var celestialObject in celestialObjects)//This method should also set the `Satellites` property for each of the `CelestialObject`s returned.
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == celestialObject.Id).ToList();
            }
            return Ok(celestialObjects);// This method should return `Ok` with a value of all `CelestialObjects`s.
        }


        [HttpPost]// Create `CelestialObjectControllers`'s Post, Put, Patch, and Delete actions. This method should have an `HttpPost` attribute.      
        public IActionResult Create([FromBody]CelestialObject celestialObject)//Create the `Create` method. This method should have a return type of `IActionResult`  This method should accept a parameter of type `[FromBody]CelestialObject`. (Note: You will need to add a `using` directive for `StarChart.Models`)
        {
            _context.CelestialObjects.Add(celestialObject);//This method should add the provided `CelestialObject` to the `CelestialObjects` `DbSet` then `SaveChanges`.
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { id = celestialObject.Id }, celestialObject);//This method should return a `CreatedAtRoute` with the arguments  `"GetById"`. A new `object` with an `id` of the `CelestialObject`'s `Id` (note: use the `new { }` format). The newly created `CelestialObject`.
        }

       
        [HttpPut("{id}")] //This method should have the `HttpPut` attribute with a value of `"{id}"`.
        public IActionResult Update(int id, CelestialObject celestialObject)//Create the `Update` method.This method should have a return type of `IActionResult`.This method should accept a parameter of type `int` named `id` and a parameter of type `CelestialObject`.
        {
            var existingObject = _context.CelestialObjects.Find(id);//This method should locate the `CelestialObject` with an `Id` that matches the provided `int` parameter. 
            if (existingObject == null)
                return NotFound();// If no match is found return `NotFound`.
            existingObject.Name = celestialObject.Name;//If a match is found set it's `Name`, `OrbitalPeriod`, and `OrbitedObjectId` properties based on the provided `CelestialObject` parameter.
            existingObject.OrbitalPeriod = celestialObject.OrbitalPeriod; 
            existingObject.OrbitedObjectId = celestialObject.OrbitedObjectId;
            _context.CelestialObjects.Update(existingObject);// Call `Update` on the `CelestialObjects` `DbSet` with an argument of the updated `CelestialObject`, 
            _context.SaveChanges();//and then call `SaveChanges`.
            return NoContent();//This method should return `NoContent`.
        }

        
        [HttpPatch("{id}/{name}")]//This method should have the `HttpPatch` attribute with an argument of `"{id}/{name}"`. 
    public IActionResult RenameObject(int id, string name) //Create the `RenameObject` method. This method should have a return type of `IActionResult`.This method should accept a parameter of type `int` named `id` and a parameter of type `string` named `name`. 
    {
            var objectsToRename = _context.CelestialObjects.Find(id);//This method should locate the `CelestialObject` with an `Id` that matches the provided `int` parameter. 
            if (objectsToRename == null)//If no match is found return `NotFound`.
                return NotFound();
            objectsToRename.Name = name;//If a match is found set it's `Name` property to the provided `name` parameter.
            _context.CelestialObjects.Update(objectsToRename);
            _context.SaveChanges();//Then call `Update` on the `CelestialObjects` `DbSet` with an argument of the updated `CelestialObject`, and then call `SaveChanges`.
            return NoContent();//This method should return `NoContent`.
        }

      
        //Create the `Delete` method
    [HttpDelete ("{id}")]
        public IActionResult Delete(int id) //This method should have a return type of `IActionResult` This method should accept a parameter of type `int` named `id`. 
        {
           // var toBeFoundobjects = _context.CelestialObjects.Where(e => e.Id == id || e.OrbitedObjectId == id).ToList()
            var toBeFoundobjects = _context.CelestialObjects.Where(e => e.Id == id || e.OrbitedObjectId == id);// This method should get a `List` of all `CelestialObject`s who either have an `Id` or `OrbitedObjectId`that matches the provided parameter. 
            if (!toBeFoundobjects.Any()) //If there are no matches it should return `NotFound`.
                return NotFound();
                _context.CelestialObjects.RemoveRange(toBeFoundobjects); //If there are matching `CelestialObject`s call `RemoveRange` on the `CelestialObjects` `DbSet` with an argument of the list of matching `CelestialObject`s. Then call `SaveChanges`.
                _context.SaveChanges();
                return NoContent();//This method should return `NoContent`.
                    }
    }
}
