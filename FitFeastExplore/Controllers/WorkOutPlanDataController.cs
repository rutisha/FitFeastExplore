using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FitFeastExplore.Models;

namespace FitFeastExplore.Controllers
{
    public class WorkOutPlanDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Retrieves a list of all workout plans.
        /// </summary>
        /// <returns>A list of WorkOutPlanDto objects.</returns>
        /// <example>
        /// GET: api/WorkOutPlanData/ListWorkOutPlans
        /// </example>
        [HttpGet]
        [Route("api/WorkOutPlanData/ListWorkOutPlans")]
        public List<WorkOutPlanDto> ListWorkOutPlans()
        {
            List<WorkOutPlan> workOutPlans = db.WorkOutPlans.ToList();

            List<WorkOutPlanDto> workOutPlanDtos = new List<WorkOutPlanDto>();

            foreach (WorkOutPlan workOutPlan in workOutPlans)
            {
                WorkOutPlanDto workOutPlanDto = new WorkOutPlanDto
                {
                    WorkOutPlanID = workOutPlan.WorkOutPlanID,
                    ExerciseName = workOutPlan.ExerciseName,
                    Reps = workOutPlan.Reps,
                    sets = workOutPlan.sets,
                    BodyPart = workOutPlan.BodyPart,
                    YouTubeUrl = workOutPlan.YouTubeUrl,
                    Notes = workOutPlan.Notes
                };

                workOutPlanDtos.Add(workOutPlanDto);
            }

            return workOutPlanDtos;
        }

        /// <summary>
        /// Adds a new workout plan to the database.
        /// </summary>
        /// <param name="workOutPlanDto">The WorkOutPlanDto object containing the details of the workout plan to be added.</param>
        /// <returns>An IHttpActionResult indicating the result of the operation.</returns>
        /// <example>
        /// POST: api/WorkOutPlanData/AddWorkOutPlan
        /// </example>
        [HttpPost]
        [Route("api/WorkOutPlanData/AddWorkOutPlan")]
        public IHttpActionResult AddWorkOutPlan(WorkOutPlanDto workOutPlanDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            WorkOutPlan workOutPlan = new WorkOutPlan
            {
                ExerciseName = workOutPlanDto.ExerciseName,
                Reps = workOutPlanDto.Reps,
                sets = workOutPlanDto.sets,
                BodyPart = workOutPlanDto.BodyPart,
                YouTubeUrl = workOutPlanDto.YouTubeUrl,
                Notes = workOutPlanDto.Notes
            };

            db.WorkOutPlans.Add(workOutPlan);
            db.SaveChanges();

            return Ok(workOutPlanDto);
        }

        /// <summary>
        /// Updates an existing workout plan in the database.
        /// </summary>
        /// <param name="id">The ID of the workout plan to be updated.</param>
        /// <param name="workOutPlanDto">The WorkOutPlanDto object containing the updated details of the workout plan.</param>
        /// <returns>An IHttpActionResult indicating the result of the operation.</returns>
        /// <example>
        /// PUT: api/WorkOutPlanData/UpdateWorkOutPlan/{id}
        /// </example>
        [HttpPut]
        [Route("api/WorkOutPlanData/UpdateWorkOutPlan/{id}")]
        public IHttpActionResult UpdateWorkOutPlan(int id, WorkOutPlanDto workOutPlanDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var workOutPlan = db.WorkOutPlans.Find(id);
            if (workOutPlan == null)
            {
                return NotFound();
            }

            workOutPlan.ExerciseName = workOutPlanDto.ExerciseName;
            workOutPlan.Reps = workOutPlanDto.Reps;
            workOutPlan.sets = workOutPlanDto.sets;
            workOutPlan.BodyPart = workOutPlanDto.BodyPart;
            workOutPlan.YouTubeUrl = workOutPlanDto.YouTubeUrl;
            workOutPlan.Notes = workOutPlanDto.Notes;

            db.SaveChanges();

            return Ok(workOutPlanDto);
        }

        /// <summary>
        /// Deletes a workout plan from the database.
        /// </summary>
        /// <param name="id">The ID of the workout plan to be deleted.</param>
        /// <returns>An IHttpActionResult indicating the result of the operation.</returns>
        /// <example>
        /// DELETE: api/WorkOutPlanData/DeleteWorkOutPlan/{id}
        /// </example>
        [HttpDelete]
        [Route("api/WorkOutPlanData/DeleteWorkOutPlan/{id}")]
        public IHttpActionResult DeleteWorkOutPlan(int id)
        {
            var workOutPlan = db.WorkOutPlans.Find(id);
            if (workOutPlan == null)
            {
                return NotFound();
            }

            db.WorkOutPlans.Remove(workOutPlan);
            db.SaveChanges();

            return Ok();
        }
    }
}
