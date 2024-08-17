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
                YouTubeUrl = model.Exercise.YouTubeUrl,
                WorkOutId = model.WorkOutId
            };

            using (var db = new ApplicationDbContext())
            {
                db.WorkOutPlans.Add(workoutPlan);
                db.SaveChanges();
            }

            return RedirectToAction("List", "Exercise");
        }


        /// <summary>
        /// Displays a form to create a new exercise.
        /// </summary>
        /// <returns>A view with a form to create a new exercise.</returns>
        /// <example>
        /// GET: Exercise/Create
        /// </example>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Creates a new exercise.
        /// </summary>
        /// <param name="exerciseDto">The ExerciseDto object containing the details of the exercise to be created.</param>
        /// <returns>Redirects to the exercise list if successful, otherwise returns the create view.</returns>
        /// <example>
        /// POST: Exercise/Create
        /// </example>
        [HttpPost]
        public ActionResult Create(ExerciseDto exerciseDto)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = new HttpClient();
                string url = "https://localhost:44306/api/exercisedata/addexercise";

                HttpResponseMessage response = client.PostAsJsonAsync(url, exerciseDto).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("List");
                }
            }

            return View(exerciseDto);
        }

        /// <summary>
        /// Displays a form to edit an existing exercise.
        /// </summary>
        /// <param name="id">The ID of the exercise to edit.</param>
        /// <returns>A view with a form to edit the exercise.</returns>
        /// <example>
        /// GET: Exercise/Edit/{id}
        /// </example>
        public ActionResult Edit(int id)
        {
            HttpClient client = new HttpClient();
            string url = "https://localhost:44306/api/exercisedata/findexercise/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            ExerciseDto exerciseDto = response.Content.ReadAsAsync<ExerciseDto>().Result;

            return View(exerciseDto);
        }

        /// <summary>
        /// Updates an existing exercise.
        /// </summary>
        /// <param name="exerciseDto">The ExerciseDto object containing the updated details of the exercise.</param>
        /// <returns>Redirects to the exercise list if successful, otherwise returns the edit view.</returns>
        /// <example>
        /// POST: Exercise/Edit/{id}
        /// </example>
        [HttpPost]
        public ActionResult Edit(ExerciseDto exerciseDto)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = new HttpClient();
                string url = "https://localhost:44306/api/exercisedata/updateexercise/" + exerciseDto.ExerciseId;

                HttpResponseMessage response = client.PutAsJsonAsync(url, exerciseDto).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("List");
                }
            }

            return View(exerciseDto);
        }

        /// <summary>
        /// Displays a confirmation page to delete an exercise.
        /// </summary>
        /// <param name="id">The ID of the exercise to delete.</param>
        /// <returns>A view to confirm the deletion of the exercise.</returns>
        /// <example>
        /// GET: Exercise/Delete/{id}
        /// </example>
        public ActionResult Delete(int id)
        {
            HttpClient client = new HttpClient();
            string url = "https://localhost:44306/api/exercisedata/findexercise/" + id;

            HttpResponseMessage response = client.GetAsync(url).Result;

            ExerciseDto exerciseDto = response.Content.ReadAsAsync<ExerciseDto>().Result;

            return View(exerciseDto);
        }

        /// <summary>
        /// Deletes an exercise from the database.
        /// </summary>
        /// <param name="id">The ID of the exercise to delete.</param>
        /// <returns>Redirects to the exercise list after successful deletion.</returns>
        /// <example>
        /// POST: Exercise/Delete/{id}
        /// </example>
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            HttpClient client = new HttpClient();
            string url = "https://localhost:44306/api/exercisedata/deleteexercise/" + id;

            HttpResponseMessage response = client.DeleteAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }

            return View();
        }
    }
 }