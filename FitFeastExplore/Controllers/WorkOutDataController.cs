using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FitFeastExplore.Models;

namespace FitFeastExplore.Controllers
{
    public class WorkOutDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns a list of workout plans
        /// </summary>
        /// <returns>An array of workout plans</returns>
        /// <example>
        /// GET: /api/WorkOutData/ListWorkOuts => <WorkOutDto>
        /// <Name>Chest</Name>
        /// <WorkOutId>15</WorkOutId>
        /// </WorkOutDto>
        /// </example>
        [HttpGet]
        [Route("api/WorkOutData/ListWorkOuts")]
        public List<WorkOutDto> ListWorkOuts()
        {
            List<WorkOut> workOuts = db.WorkOuts.ToList();

            List<WorkOutDto> workOutDtos = new List<WorkOutDto>();

            foreach (WorkOut workOut in workOuts)
            {
                WorkOutDto workOutDto = new WorkOutDto
                {
                    WorkOutId = workOut.WorkOutId,
                    Name = workOut.WorkOutName
                };

                workOutDtos.Add(workOutDto);
            }

            return workOutDtos;
        }

        /// <summary>
        /// Adds a new workout plan
        /// </summary>
        /// <param name="workOutDto">The workout plan to add</param>
        /// <example>
        /// POST: /api/WorkOutData/AddWorkOut
        /// </example>
        [HttpPost]
        [Route("api/WorkOutData/AddWorkOut")]
        public IHttpActionResult AddWorkOut(WorkOutDto workOutDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            WorkOut workOut = new WorkOut
            {
                WorkOutName = workOutDto.Name
            };

            db.WorkOuts.Add(workOut);
            db.SaveChanges();

            return Ok(workOutDto);
        }
    }
}
