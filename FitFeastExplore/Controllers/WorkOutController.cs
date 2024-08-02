using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using FitFeastExplore.Models;

namespace FitFeastExplore.Controllers
{
    public class WorkOutController : Controller
    {
        /// <summary>
        /// Displays a list of all workouts.
        /// </summary>
        /// <returns>A view with a list of workouts.</returns>
        /// <example>
        /// GET: WorkOut/List
        /// </example>
        public ActionResult List()
        {
            HttpClient client = new HttpClient();
            string url = "https://localhost:44306/api/WorkOutData/ListWorkOuts";

            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<WorkOutDto> workOuts = response.Content.ReadAsAsync<IEnumerable<WorkOutDto>>().Result;
            return View(workOuts);
        }

        /// <summary>
        /// Displays a form for creating a new workout.
        /// </summary>
        /// <returns>A view with a form for creating a new workout.</returns>
        /// <example>
        /// GET: WorkOut/Create
        /// </example>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Creates a new workout.
        /// </summary>
        /// <param name="workOutDto">The workout data transfer object containing workout details.</param>
        /// <returns>Redirects to the list of workouts if successful, otherwise returns the create view with validation errors.</returns>
        /// <example>
        /// POST: WorkOut/Create
        /// </example>
        [HttpPost]
        public ActionResult Create(WorkOutDto workOutDto)
        {
            HttpClient client = new HttpClient();
            string url = "https://localhost:44306/api/WorkOutData/AddWorkOut";

            HttpResponseMessage response = client.PostAsJsonAsync(url, workOutDto).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List", "Exercise");
            }

            return View(workOutDto);
        }

        /// <summary>
        /// Displays the details of a specific workout.
        /// </summary>
        /// <param name="id">The ID of the workout to display.</param>
        /// <returns>A view with workout details and associated exercises.</returns>
        /// <example>
        /// GET: WorkOut/Details/{id}
        /// </example>
        public ActionResult Details(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var workout = db.WorkOuts.Find(id);
                if (workout == null)
                {
                    return HttpNotFound();
                }

                var exercises = db.WorkOutPlans.Where(wp => wp.WorkOutId == id).ToList();

                var viewModel = new WorkOutDetailsViewModel
                {
                    WorkOut = new WorkOutDto
                    {
                        WorkOutId = workout.WorkOutId,
                        Name = workout.WorkOutName
                    },
                    Exercises = exercises.Select(e => new ExerciseDto
                    {
                        ExerciseId = e.ExerciseId,
                        ExerciseName = e.ExerciseName,
                        Reps = e.Reps,
                        sets = e.sets,
                        BodyPart = e.BodyPart
                    }).ToList()
                };

                return View(viewModel);
            }
        }

        /// <summary>
        /// Displays a form for editing a specific workout.
        /// </summary>
        /// <param name="id">The ID of the workout to edit.</param>
        /// <returns>A view with a form for editing the workout.</returns>
        /// <example>
        /// GET: WorkOut/Edit/{id}
        /// </example>
        public ActionResult Edit(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var workout = db.WorkOuts.Find(id);
                if (workout == null)
                {
                    return HttpNotFound();
                }

                var viewModel = new WorkOutDto
                {
                    WorkOutId = workout.WorkOutId,
                    Name = workout.WorkOutName
                };

                return View(viewModel);
            }
        }

        /// <summary>
        /// Edits a specific workout.
        /// </summary>
        /// <param name="workOutDto">The workout data transfer object containing updated workout details.</param>
        /// <returns>Redirects to the list of workouts if successful, otherwise returns the edit view with validation errors.</returns>
        /// <example>
        /// POST: WorkOut/Edit/{id}
        /// </example>
        [HttpPost]
        public ActionResult Edit(WorkOutDto workOutDto)
        {
            if (ModelState.IsValid)
            {
                using (var db = new ApplicationDbContext())
                {
                    var workout = db.WorkOuts.Find(workOutDto.WorkOutId);
                    if (workout == null)
                    {
                        return HttpNotFound();
                    }

                    workout.WorkOutName = workOutDto.Name;
                    db.SaveChanges();

                    return RedirectToAction("List");
                }
            }

            return View(workOutDto);
        }

        /// <summary>
        /// Displays a confirmation page for deleting a specific workout.
        /// </summary>
        /// <param name="id">The ID of the workout to delete.</param>
        /// <returns>A view with workout details to confirm deletion.</returns>
        /// <example>
        /// GET: WorkOut/Delete/{id}
        /// </example>
        public ActionResult Delete(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var workout = db.WorkOuts.Find(id);
                if (workout == null)
                {
                    return HttpNotFound();
                }

                var viewModel = new WorkOutDto
                {
                    WorkOutId = workout.WorkOutId,
                    Name = workout.WorkOutName
                };

                return View(viewModel);
            }
        }

        /// <summary>
        /// Deletes a specific workout.
        /// </summary>
        /// <param name="id">The ID of the workout to delete.</param>
        /// <returns>Redirects to the list of workouts after deletion.</returns>
        /// <example>
        /// POST: WorkOut/Delete/{id}
        /// </example>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var workout = db.WorkOuts.Find(id);
                if (workout == null)
                {
                    return HttpNotFound();
                }

                db.WorkOuts.Remove(workout);
                db.SaveChanges();

                return RedirectToAction("List");
            }
        }
    }
}