using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FitFeastExplore.Models;

namespace FitFeastExplore.Controllers
{
    public class WorkOutPlanController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Displays a list of all workout plans.
        /// </summary>
        /// <returns>A view with a list of workout plans.</returns>
        /// <example>
        /// GET: WorkOutPlan/List
        /// </example>
        public ActionResult List()
        {
            var workoutPlans = db.WorkOutPlans.ToList();
            return View(workoutPlans);
        }


        /// <summary>
        /// Displays a form for editing a specific workout plan.
        /// </summary>
        /// <param name="id">The ID of the workout plan to edit.</param>
        /// <returns>A view with a form for editing the workout plan.</returns>
        /// <example>
        /// GET: WorkOutPlan/Edit/{id}
        /// </example>
        [Authorize]
        public ActionResult Edit(int id)
        {
            var workoutPlan = db.WorkOutPlans.Find(id);
            if (workoutPlan == null)
            {
                return HttpNotFound();
            }

            var viewModel = new WorkOutPlanDto
            {
                WorkOutPlanID = workoutPlan.WorkOutPlanID,
                ExerciseName = workoutPlan.ExerciseName,
                Reps = workoutPlan.Reps,
                sets = workoutPlan.sets,
                BodyPart = workoutPlan.BodyPart,
                YouTubeUrl = workoutPlan.YouTubeUrl,
                Notes = workoutPlan.Notes
            };

            return View(viewModel);
        }

        /// <summary>
        /// Displays a form for creating a new workout plan.
        /// </summary>
        /// <returns>A view with a form for creating a new workout plan.</returns>
        /// <example>
        /// GET: WorkOutPlan/Create
        /// </example>
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Creates a new workout plan.
        /// </summary>
        /// <param name="workOutPlanDto">The workout plan data transfer object containing workout plan details.</param>
        /// <returns>Redirects to the workout details if successful, otherwise returns the create view with validation errors.</returns>
        /// <example>
        /// POST: WorkOutPlan/Create
        /// </example>
        [HttpPost]
        [Authorize]
        public ActionResult Create(WorkOutPlanDto workOutPlanDto)
        {
            if (ModelState.IsValid)
            {
                var workoutPlan = new WorkOutPlan
                {
                    ExerciseName = workOutPlanDto.ExerciseName,
                    Reps = workOutPlanDto.Reps,
                    sets = workOutPlanDto.sets,
                    BodyPart = workOutPlanDto.BodyPart,
                    YouTubeUrl = workOutPlanDto.YouTubeUrl,
                    Notes = workOutPlanDto.Notes,
                    WorkOutId = workOutPlanDto.WorkOutPlanID
                };

                db.WorkOutPlans.Add(workoutPlan);
                db.SaveChanges();

                return RedirectToAction("Details", "WorkOut", new { id = workoutPlan.WorkOutId });
            }

            return View(workOutPlanDto);
        }

        /// <summary>
        /// Edits a specific workout plan.
        /// </summary>
        /// <param name="workOutPlanDto">The workout plan data transfer object containing updated workout plan details.</param>
        /// <returns>Redirects to the workout details if successful, otherwise returns the edit view with validation errors.</returns>
        /// <example>
        /// POST: WorkOutPlan/Edit/{id}
        /// </example>
        [HttpPost]
        [Authorize]
        public ActionResult Edit(WorkOutPlanDto workOutPlanDto)
        {
            if (ModelState.IsValid)
            {
                var workoutPlan = db.WorkOutPlans.Find(workOutPlanDto.WorkOutPlanID);
                if (workoutPlan == null)
                {
                    return HttpNotFound();
                }

                workoutPlan.ExerciseName = workOutPlanDto.ExerciseName;
                workoutPlan.Reps = workOutPlanDto.Reps;
                workoutPlan.sets = workOutPlanDto.sets;
                workoutPlan.BodyPart = workOutPlanDto.BodyPart;
                workoutPlan.YouTubeUrl = workOutPlanDto.YouTubeUrl;
                workoutPlan.Notes = workOutPlanDto.Notes;

                db.SaveChanges();

                return RedirectToAction("Details", "WorkOut", new { id = workoutPlan.WorkOutId });
            }

            return View(workOutPlanDto);
        }

        /// <summary>
        /// Displays a confirmation page for deleting a specific workout plan.
        /// </summary>
        /// <param name="id">The ID of the workout plan to delete.</param>
        /// <returns>A view with workout plan details to confirm deletion.</returns>
        /// <example>
        /// GET: WorkOutPlan/Delete/{id}
        /// </example>
        [Authorize]
        public ActionResult Delete(int id)
        {
            var workoutPlan = db.WorkOutPlans.Find(id);
            if (workoutPlan == null)
            {
                return HttpNotFound();
            }

            var viewModel = new WorkOutPlanDto
            {
                WorkOutPlanID = workoutPlan.WorkOutPlanID,
                ExerciseName = workoutPlan.ExerciseName,
                Reps = workoutPlan.Reps,
                sets = workoutPlan.sets,
                BodyPart = workoutPlan.BodyPart,
                YouTubeUrl = workoutPlan.YouTubeUrl,
                Notes = workoutPlan.Notes
            };

            return View(viewModel);
        }

        /// <summary>
        /// Deletes a specific workout plan.
        /// </summary>
        /// <param name="id">The ID of the workout plan to delete.</param>
        /// <returns>Redirects to the workout details after deletion.</returns>
        /// <example>
        /// POST: WorkOutPlan/Delete/{id}
        /// </example>
        [HttpPost, ActionName("Delete")]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var workoutPlan = db.WorkOutPlans.Find(id);
                if (workoutPlan == null)
                {
                    return HttpNotFound();
                }

                db.WorkOutPlans.Remove(workoutPlan);
                db.SaveChanges();

                return RedirectToAction("Details", "WorkOut", new { id = workoutPlan.WorkOutId });
            }
        }
    }
}
