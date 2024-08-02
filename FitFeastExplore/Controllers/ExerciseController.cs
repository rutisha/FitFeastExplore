using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using FitFeastExplore.Models;

namespace FitFeastExplore.Controllers
{
    public class ExerciseController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Displays a list of exercises with optional search functionality.
        /// </summary>
        /// <param name="searchString">The search string to filter exercises.</param>
        /// <returns>A view with a list of exercises.</returns>
        /// <example>
        /// GET: Exercise/List
        /// </example>
        public ActionResult List(string searchString)
        {
            HttpClient client = new HttpClient();
            string url;

            if (string.IsNullOrEmpty(searchString))
            {
                url = "https://localhost:44306/api/exercisedata/listexercises";
            }
            else
            {
                url = $"https://localhost:44306/api/exercisedata/searchexercises?searchString={searchString}";
            }

            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<ExerciseDto> Exercises = response.Content.ReadAsAsync<IEnumerable<ExerciseDto>>().Result;

            ViewBag.search = searchString;
            return View(Exercises);
        }

        /// <summary>
        /// Displays details of a specific exercise.
        /// </summary>
        /// <param name="id">The ID of the exercise to show.</param>
        /// <returns>A view with exercise details and associated workouts.</returns>
        /// <example>
        /// GET: Exercise/Show/{id}
        /// </example>
        public ActionResult Show(int id)
        {
            HttpClient client = new HttpClient();
            string url = "https://localhost:44306/api/exercisedata/findexercise/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            ExerciseDto Exercise = response.Content.ReadAsAsync<ExerciseDto>().Result;

            string workoutsUrl = "https://localhost:44306/api/workoutdata/listworkouts";
            HttpResponseMessage workoutsResponse = client.GetAsync(workoutsUrl).Result;
            IEnumerable<WorkOutDto> WorkOuts = workoutsResponse.Content.ReadAsAsync<IEnumerable<WorkOutDto>>().Result;

            var viewModel = new ExerciseWorkOutViewModel
            {
                Exercise = Exercise,
                WorkOuts = WorkOuts.Select(w => new SelectListItem
                {
                    Value = w.WorkOutId.ToString(),
                    Text = w.Name
                }).ToList()
            };

            return View(viewModel);
        }

        /// <summary>
        /// Adds an exercise to a workout plan.
        /// </summary>
        /// <param name="model">The view model containing exercise and workout plan details.</param>
        /// <returns>Redirects to the exercise list after adding the exercise to the workout plan.</returns>
        /// <example>
        /// POST: Exercise/AddToWorkoutPlan
        /// </example>
        [HttpPost]
        public ActionResult AddToWorkoutPlan(ExerciseWorkOutViewModel model)
        {
            WorkOutPlan workoutPlan = new WorkOutPlan
            {
                ExerciseId = model.Exercise.ExerciseId,
                ExerciseName = model.Exercise.ExerciseName,
                Reps = model.Exercise.Reps,
                sets = model.Exercise.sets,
                BodyPart = model.Exercise.BodyPart,
                WorkOutId = model.WorkOutId
            };

            using (var db = new ApplicationDbContext())
            {
                db.WorkOutPlans.Add(workoutPlan);
                db.SaveChanges();
            }

            return RedirectToAction("List", "Exercise");
        }
    }
 }