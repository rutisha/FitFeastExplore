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
                    Notes = workOutPlan.Notes
                };

                workOutPlanDtos.Add(workOutPlanDto);
            }

            return workOutPlanDtos;
        }
    }
}
