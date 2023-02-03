using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Reflection.Metadata.Ecma335;
using Region = NZWalks.API.Models.Domain.Region;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        //=================================CONSTRUCTOR===============================================================

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        //=====================================Controller Methods=====================================================
        [HttpGet]
        [Authorize(Roles ="reader")]
        public async Task<IActionResult> GetAllRegions()
        {

            var regions = await regionRepository.GetAllAsync();
           
            var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);

            return Ok(regionsDTO);
        }

        [HttpGet]
        [Authorize(Roles = "reader")]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetRegionByIdAsync(Guid id)
        {

            var found = await regionRepository.GetAsync(id);
            if (found == null)
                return NotFound();

            var regionDTO = mapper.Map<Models.DTO.Region>(found);

            return Ok(regionDTO);
        }


        [HttpPost]
        [ActionName("AddRegionAsync")]
        [Authorize(Roles ="writer")]
        public async Task<IActionResult> AddRegionAsync(AddRegionRequest addRegionRequest)
        {
            //// validate the request from the client 
            //if (!ValidateAddRegionAsync(addRegionRequest))
            //{
            //    return BadRequest(ModelState); 
            //}

            // convert data from request(DTO)->domain model
            var region = new Models.Domain.Region
            {
                Code = addRegionRequest.Code,
                Name = addRegionRequest.Name,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Population = addRegionRequest.Population
            };
            //2.pass the data to the repository
            region = await regionRepository.AddAsync(region);
            //3.convert the data back to DTO
            var regionDTO = mapper.Map<Models.DTO.Region>(region);

            return CreatedAtAction(nameof(AddRegionAsync), new { id = regionDTO.Id }, regionDTO);


        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles ="writer")]
        public async Task<IActionResult> DeleteRegionByIdAsync(Guid id)
        {
            //1. take data from db 
            var region = await regionRepository.DeleteAsync(id);
            // 2. if data = null, return null
            if (region == null)
                return NotFound();
            // 3. if not null, convert data from domain -> dto 
            var regionDTO = mapper.Map<Models.DTO.Region>(region);

            return Ok(regionDTO);
        }


        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles="writer")]
        public async Task<IActionResult> UpdateRegionByIdAsync([FromRoute]Guid id,
            [FromBody]UpdateRegionRequest updateRegionRequest)

        { // validate the client input 
            //if (!ValidateUpdateRegionAsync(updateRegionRequest))
            //{
            //    return BadRequest(ModelState);
            //}
            //1.convert dto -> domain
            var region = new Models.Domain.Region()
            {

                Code = updateRegionRequest.Code,
                Name = updateRegionRequest.Name,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Population = updateRegionRequest.Population


            };


            //2. update region using repository
            region=await  regionRepository.UpdateAsync(id, region);
            //3.if null , return null
            if (region == null)
            {
                return NotFound();

            }
            //4.convert domain ->dto and return back ok response.
            var regionDTO = mapper.Map<Models.DTO.Region>(region);
            return Ok(regionDTO);
        }




        #region Private Methods

        private bool ValidateAddRegionAsync(AddRegionRequest addRegionRequest)
        {
            // validtaing the full request 
            if (addRegionRequest == null)
            {
                ModelState.AddModelError(nameof(addRegionRequest),
                $"Add Region Data is required ");

                return false;
            }

            // validtaing Code property 
            if (string.IsNullOrWhiteSpace(addRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Code),
                    $"{nameof(addRegionRequest.Code)} must not be empty OR null OR whitespace");
            }

            // validating Name property 
            if (string.IsNullOrWhiteSpace(addRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Name),
                    $"{nameof(addRegionRequest.Name)} must not be empty or null or whitespace ");
            }

            // validating the area property 
            if (addRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Area),
                    $"{nameof(addRegionRequest.Area)} must be greater than 0 ");

            }

            // validating the latitiude property 
            if (addRegionRequest.Lat <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Lat),
                    $"{nameof(addRegionRequest.Lat)} must be greater than 0 ");
            }

            // validating the longitude property 
            if (addRegionRequest.Long <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Long),
                    $"{nameof(addRegionRequest.Long)} must be greater than 0 ");
            }

            // validating the populatino property 
            if (addRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Population),
                    $"{nameof(addRegionRequest.Population)} must not be less than 0 ");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }


        private bool ValidateUpdateRegionAsync(UpdateRegionRequest updateRegionRequest)
        {
            // validate the entire request 
            if (updateRegionRequest == null)
            {
                ModelState.AddModelError(nameof(updateRegionRequest),
                    $"Update Region Data is Required");

                return false;
            }

            //validating code property 
            if (string.IsNullOrWhiteSpace(updateRegionRequest.Code)) 
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Code),
                    $"{nameof(updateRegionRequest.Code)} should not be an empty space OR null OR whitespace");
            }

            if (string.IsNullOrWhiteSpace(updateRegionRequest.Name)) 
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Name),
                    $"{nameof(updateRegionRequest.Name)} should not be an empty space or null or whitespace");
            }

            if (updateRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Area),
                    $"{nameof(updateRegionRequest)} should not be less than 0 OR = 0");
            }

            if (updateRegionRequest.Lat <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Lat),
                    $"{nameof(updateRegionRequest.Lat)} should not be less than 0 OR = 0");
            }

            if (updateRegionRequest.Long <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Long),
                    $"{nameof(updateRegionRequest.Long)} must not be less than 0 OR = 0");
            }

            if (updateRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Population),
                    $"{nameof(updateRegionRequest.Population)} should not be less than 0 ");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        
        }
        #endregion

    }
}
    

