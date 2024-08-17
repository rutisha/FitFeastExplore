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
        /// Adds a new workout to the database.
        /// </summary>
        /// <param name="workOutDto">The WorkOutDto object containing the details of the workout to be added.</param>
        /// <returns>An IHttpActionResult indicating the result of the operation.</returns>
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

        /// <summary>
        /// Updates an existing workout in the database.
        /// </summary>
        /// <param name="id">The ID of the workout to be updated.</param>
        /// <param name="workOutDto">The WorkOutDto object containing the updated details of the workout.</param>
        /// <returns>An IHttpActionResult indicating the result of the operation.</returns>
        /// <example>
        /// PUT: api/WorkOutData/UpdateWorkOut/{id}
        /// </example>
        [HttpPut]
        [Route("api/WorkOutData/UpdateWorkOut/{id}")]
        public IHttpActionResult UpdateWorkOut(int id, WorkOutDto workOutDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var workOut = db.WorkOuts.Find(id);
            if (workOut == null)
            {
                return NotFound();
            }

            workOut.WorkOutName = workOutDto.Name;
            db.SaveChanges();

            return Ok(workOutDto);
        }

        /// <summary>
        /// Deletes a workout from the database.
        /// </summary>
        /// <param name="id">The ID of the workout to be deleted.</param>
        /// <returns>An IHttpActionResult indicating the result of the operation.</returns>
        /// <example>
        /// DELETE: api/WorkOutData/DeleteWorkOut/{id}
        /// </example>
        [HttpDelete]
        [Route("api/WorkOutData/DeleteWorkOut/{id}")]
        public IHttpActionResult DeleteWorkOut(int id)
        {
            var workOut = db.WorkOuts.Find(id);
            if (workOut == null)
            {
                return NotFound();
            }

            db.WorkOuts.Remove(workOut);
            db.SaveChanges();

            return Ok();
        }
    }
}
