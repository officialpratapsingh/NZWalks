using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;
        private readonly IRegionRepository regionRepository;
        private readonly IWalkDifficultyRepository walkDifficultyRepository;

        //===============================CONSTRUCTOR=======================================================
        public WalksController(IWalkRepository walkRepository,
            IMapper mapper,
            IRegionRepository regionRepository,
            IWalkDifficultyRepository walkDifficultyRepository)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
            this.regionRepository = regionRepository;
            this.walkDifficultyRepository = walkDifficultyRepository;
        }
        //==================================Controller Methods==============================================

        [HttpGet]
        [Authorize(Roles ="reader")]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            var walks = await walkRepository.GetAllAsync();
            var walkDTO = mapper.Map<List<Models.DTO.Walk>>(walks);
            return Ok(walkDTO);

        }

        [HttpGet]
        [Route("{id:guid}")]
        [Authorize(Roles = "reader")]

        public async Task<IActionResult> GetWalksByIdAsync(Guid id)
        {
            var walk = await walkRepository.GetAsync(id);



            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);

            return Ok(walkDTO);


        }

        [HttpPost]
        [ActionName("AddWalkAsync")]
        [Authorize(Roles = "writer")]

        public async Task<IActionResult> AddWalkAsync(Models.DTO.AddWalkRequest addWalkRequest)
        {
            // validate the input from the client 
            if (!(await ValidateAddWalkAsync(addWalkRequest)))
            {
                return BadRequest(ModelState);
            }
            // convert dto -> domain model 
            var walk = new Models.Domain.Walk
            {
                Name = addWalkRequest.Name,
                Length = addWalkRequest.Length,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId
            };

            // use repo to add to db calling the created fxn 
            walk = await walkRepository.AddAsync(walk);

            // map back the domain -> dto
            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);

            // return back to client 
            return CreatedAtAction(nameof(AddWalkAsync), new { id = walkDTO.Id }, walkDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles ="writer")]

        public async Task<IActionResult> UpdateWalksByIdAsync([FromRoute] Guid id, [FromBody] UpdateWalkRequest updateWalkRequest)
        {
            // validate the incoming request first 
            if (!(await ValidateUpdateWalksByIdAsync(updateWalkRequest)))
            {
                return BadRequest(ModelState);
            }
            // convert dto -> domain
            var walk = new Models.Domain.Walk
            {
                Name = updateWalkRequest.Name,
                Length = updateWalkRequest.Length,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId
            };
            // pass & save details to repo
            walk = await walkRepository.UpdateAsync(id, walk);

            if (walk == null)
                return NotFound();
            // convert domain -> dto 
            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);
            //return back to client 
            return Ok(walkDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles ="writer")]

        public async Task<IActionResult> DeleteWalkByIdAsync(Guid id) 
        {
            // call repository and call the fxn in implementation 
            var walk = await walkRepository.DeleteAsync(id);
            // if null , return not found
            if (walk == null) { return NotFound(); }

            //if not, map it back to the dto & return deleted walk record to client
            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);
            return Ok(walkDTO);

        }

        #region PrivateMethods 

        private async Task<bool> ValidateAddWalkAsync(AddWalkRequest addWalkRequest) {

            //if (addWalkRequest == null)
            //{
            //    ModelState.AddModelError(nameof(addWalkRequest),
            //        $"need data for Adding walks");
            //    return false;
            //}
            //// validating name property 
            //if (string.IsNullOrWhiteSpace(addWalkRequest.Name))
            //{
            //    ModelState.AddModelError(nameof(addWalkRequest.Name),
            //        $"{nameof(addWalkRequest.Name)} should not be NULL OR whitespace or emptyspace");
            //}
            //// validating Length property 
            //if (addWalkRequest.Length < 0)
            //{
            //    ModelState.AddModelError(nameof(addWalkRequest.Length),
            //        $"{nameof(addWalkRequest.Length)} should not be less than 0 ");
            //}
            
            // validating region ID
            var region = await regionRepository.GetAsync(addWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.RegionId),
                    $"{nameof(addWalkRequest.RegionId)} is an invalid  ID . ");
            }

            // validating walkdifficulty ID
            var walkDifficulty = await walkDifficultyRepository.GetAsync(addWalkRequest.WalkDifficultyId);
            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId),
                    $"{nameof(addWalkRequest.WalkDifficultyId)} is an invalid ID");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        
        }

        private async Task<bool> ValidateUpdateWalksByIdAsync(UpdateWalkRequest updateWalkRequest)
        {
            //if (updateWalkRequest == null)
            //{
            //    ModelState.AddModelError(nameof(updateWalkRequest),
            //        $"{nameof(updateWalkRequest)} update data should not be null");
            //}

            //if (string.IsNullOrWhiteSpace(updateWalkRequest.Name))
            //{
            //    ModelState.AddModelError(nameof(updateWalkRequest.Name),
            //        $"{nameof(updateWalkRequest.Name)} should not be empty or whitespace or null");
            //}

            //if (updateWalkRequest.Length < 0)
            //{
            //    ModelState.AddModelError(nameof(updateWalkRequest.Length),
            //        $"{nameof(updateWalkRequest.Length)} should not be less than 0 ");
            //}
            // chekcing the region ID property of updateWalk request 
            var region = await regionRepository.GetAsync(updateWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.RegionId),
                    $"{nameof(updateWalkRequest.RegionId)} is an invalid ID");
            }

            // chekcing the walkdifficulty ID property of updateWalk request 
            var walkDifficulty = walkDifficultyRepository.GetAsync(updateWalkRequest.WalkDifficultyId);
            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.WalkDifficultyId),
                    $"{nameof(updateWalkRequest.WalkDifficultyId)} is an invalid ID");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true; }


        #endregion

    }
}

