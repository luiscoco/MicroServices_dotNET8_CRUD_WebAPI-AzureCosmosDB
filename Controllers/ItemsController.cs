using Microsoft.AspNetCore.Mvc;
using AzureCosmosCRUDWebAPI.Models;
using AzureCosmosCRUDWebAPI.Services;
using System.Threading.Tasks;

namespace AzureCosmosCRUDWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FamilyController : ControllerBase
    {
        private readonly CosmosDbService _cosmosDbService;

        public FamilyController(CosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        // POST api/family
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Family family)
        {
            if (family == null)
            {
                return BadRequest("Family cannot be null");
            }

            await _cosmosDbService.AddItemAsync(family);
            return CreatedAtAction(nameof(Get), new { id = family.Id }, family);
        }

        // GET api/family/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id, string partitionKeyValue)
        {
            var family = await _cosmosDbService.GetItemAsync(id, partitionKeyValue);
            if (family == null)
            {
                return NotFound();
            }

            return Ok(family);
        }

        // PUT api/family/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Family family)
        {
            if (family == null || family.Id != id)
            {
                return BadRequest();
            }

            await _cosmosDbService.UpdateItemAsync(id, family);
            return NoContent();
        }

        // DELETE api/family/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, string partitionKeyValue)
        {
            await _cosmosDbService.DeleteItemAsync(id, partitionKeyValue);
            return NoContent();
        }

        // GET api/family
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var families = await _cosmosDbService.GetItemsAsync("SELECT * FROM c");
            return Ok(families);
        }
    }
}
