using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Data;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultyController : Controller
    {   //==============================READ ONLY FIELDS=======================================================
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        //================================CONSTRUCTOR==========================================================
        public WalkDifficultyController(IWalkDifficultyRepository 
            walkDifficultyRepository, IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }
        //================================CONTROLLER METHODS=====================================================

        [HttpGet]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetAllWalkDifficulty()
        {
            // use repo & get data 
            var walkDifficulty = await walkDifficultyRepository.GetAllAsync();

            var walkDifficultyDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDifficulty);

            return Ok(walkDifficultyDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetWalkDifficultyByIdAsync(Guid id)
        { // use repo to call fxn 
            var walkDifficulty = await walkDifficultyRepository.GetAsync(id);
            // if null , return not found 
            if (walkDifficulty == null)
            { return NotFound(); }
            // if not null , convert to DTO & return .
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);

            return Ok(walkDifficultyDTO);


        }

        [HttpPost]
        [ActionName("AddWalkDifficultyAsync")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> AddWalkDifficultyAsync([FromBody] 
        AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            //// validate the incoming request 
            //if (!ValidateAddWalkDifficultyAsync(addWalkDifficultyRequest))
            //{
            //    return BadRequest(ModelState);
            //}
            // dto -> domain 
            var walkDifficulty = new Models.Domain.WalkDifficulty {
                Code = addWalkDifficultyRequest.Code

            };
            walkDifficulty = await walkDifficultyRepository.AddAsync(walkDifficulty);
            // domain -> dto
            var walkDifficultyDTO = mapper.Map<Models.Domain.WalkDifficulty>(walkDifficulty);

            return CreatedAtAction(nameof(AddWalkDifficultyAsync),
                new { id = walkDifficultyDTO.Id }, walkDifficultyDTO);


        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> DeleteWalkDifficultyByIdAsync(Guid id)
        {
            var walkDifficulty = await walkDifficultyRepository.DeleteAsync(id);

            if (walkDifficulty != null)
            {
                var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);
                return Ok(walkDifficultyDTO);
            }

            return NotFound();

        }

        [HttpPatch]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]

        public async Task<IActionResult> UpdateWalkDifficultyById([FromRoute]Guid id,
            [FromBody]UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            // validate the incoming request 
            //if (!ValidateUpdateWalkDifficultyById(updateWalkDifficultyRequest))
            //{ 
            //    return BadRequest(ModelState);  
            //}
            
            // dto -> domain 
            var walkDifficulty = new Models.Domain.WalkDifficulty
            {

                Code = updateWalkDifficultyRequest.Code
            };
            // save inside db using repo & created fxn 
            walkDifficulty = await walkDifficultyRepository.UpdateAsync(id, walkDifficulty);

            // convert back to dto & return to client 
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);
            return Ok(walkDifficultyDTO);

        }

        #region Private Methods

        private bool  ValidateAddWalkDifficultyAsync(AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            if (addWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest),
                    $"{nameof(addWalkDifficultyRequest)} add data is necessary ");
                return false;
            }

            if (string.IsNullOrWhiteSpace(addWalkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest.Code),
                    $"{nameof(addWalkDifficultyRequest.Code)} should not be empty or whitespace or null");
            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;  }

        private bool ValidateUpdateWalkDifficultyById(UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        
        {
            if (updateWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest),
                    $"{nameof(updateWalkDifficultyRequest)} update data is necessary");
            }

            if (string.IsNullOrWhiteSpace(updateWalkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest.Code),
                    $"{nameof(updateWalkDifficultyRequest.Code)} should not be a whitespace OR null OR empty string");
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
