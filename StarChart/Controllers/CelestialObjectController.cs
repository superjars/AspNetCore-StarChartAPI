using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

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

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.CelestialObjects.FirstOrDefault(c => c.Id == id);

            if (celestialObject == null)
            {
                return NotFound();
            }

            var satellites = _context.CelestialObjects.Where(c => c.Id == celestialObject.OrbitedObjectId).ToList();

            celestialObject.Satellites = satellites;

            return Ok(celestialObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects.Where(c => c.Name == name).ToList(); ;

            if (celestialObjects.Count == 0)
            {
                return NotFound();
            }

            foreach (var celestialObject in celestialObjects)
            {
                var satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == celestialObject.Id).ToList();

                celestialObject.Satellites = satellites;
            }

            return Ok(celestialObjects);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects.ToList();

            foreach (var celestialObject in celestialObjects)
            {
                var satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == celestialObject.Id).ToList();

                celestialObject.Satellites = satellites;
            }

            return Ok(celestialObjects);
        }
    }
}
