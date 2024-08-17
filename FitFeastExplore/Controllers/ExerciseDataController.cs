using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using FitFeastExplore.Models;

namespace FitFeastExplore.Controllers
{
    public class ExerciseDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Retrieves a list of exercises.
        /// </summary>
        /// <returns>A list of ExerciseDto objects.</returns>
        /// <example>
        /// GET: api/ExerciseData/ListExercises =>
        /// <ExerciseDto>
        /// <BodyPart>Abs</BodyPart>
        /// <ExerciseId>1</ExerciseId>
        /// <ExerciseName>Ab Crunch</ExerciseName>
        /// <Reps>10</Reps>
        /// <sets>3</sets>
        /// </ExerciseDto>
        /// </example>
        [HttpGet]
        [Route("api/ExerciseData/ListExercises")]
        public List<ExerciseDto> ListExercises()
        {
            List<Exercise> Exercises = db.Exercises.ToList();

            List<ExerciseDto> ExerciseDtos = new List<ExerciseDto>();

            foreach (Exercise Exercise in Exercises)
            {
                ExerciseDto ExerciseDto = new ExerciseDto
                {
                    ExerciseId = Exercise.ExerciseId,
                    ExerciseName = Exercise.ExerciseName,
                    Reps = Exercise.Reps,
                    sets = Exercise.sets,
                    BodyPart = Exercise.BodyPart,
                    YouTubeUrl = Exercise.YouTubeUrl
            };

                ExerciseDtos.Add(ExerciseDto);
            }

            return ExerciseDtos;
        }

        /// <summary>
        /// Searches for exercises that match the given search string.
        /// </summary>
        /// <param name="searchString">The search string to filter exercises.</param>
        /// <returns>A list of ExerciseDto objects that match the search criteria.</returns>
        /// <example>
        /// GET: api/ExerciseData/SearchExercises?searchString=Chest =>
        /// <ExerciseDto>
        /// <BodyPart>Chest</BodyPart>
        /// <ExerciseId>49</ExerciseId>
        /// <ExerciseName>Dumbbell Chest Fly</ExerciseName>
        /// <Reps>12</Reps>
        /// <sets>3</sets>
        /// </ExerciseDto>
        /// </example>
        [HttpGet]
        [Route("api/ExerciseData/SearchExercises")]
        public List<ExerciseDto> SearchExercises(string searchString)
        {
            // Use LINQ to filter exercises based on the search string
            var exercises = db.Exercises.Where(e => e.ExerciseName.ToLower().Contains(searchString.ToLower())).ToList();

            List<ExerciseDto> exerciseDtos = new List<ExerciseDto>();

            foreach (Exercise exercise in exercises)
            {
                ExerciseDto exerciseDto = new ExerciseDto
                {
                    ExerciseId = exercise.ExerciseId,
                    ExerciseName = exercise.ExerciseName,
                    Reps = exercise.Reps,
                    sets = exercise.sets,
                    BodyPart = exercise.BodyPart,
                    YouTubeUrl = exercise.YouTubeUrl
                };

                exerciseDtos.Add(exerciseDto);
            }

            return exerciseDtos;
        }


        /// <summary>
        /// Retrieves details of a specific exercise by ID.
        /// </summary>
        /// <param name="id">The ID of the exercise to retrieve.</param>
        /// <returns>An ExerciseDto object with the exercise details.</returns>
        /// <example>
        /// GET: api/ExerciseData/FindExercise/5 =>
        /// <ExerciseDto xmlns:i="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://schemas.datacontract.org/2004/07Fitness_Management.Models">
        /// <BodyPart>Abs</BodyPart>
        /// <ExerciseId>5</ExerciseId>
        /// <ExerciseName>Bent Knee Windscreen Wiper</ExerciseName>
        /// <Reps>10</Reps>
        /// <sets>3</sets>
        /// </ExerciseDto>
        /// </example>
        [HttpGet]
        [Route("api/ExerciseData/FindExercise/{id}")]
        public ExerciseDto FindExercise(int Id)
        {
            Exercise Exercise = db.Exercises.Find(Id);

            ExerciseDto ExerciseDto = new ExerciseDto
            {
                ExerciseId = Exercise.ExerciseId,
                ExerciseName = Exercise.ExerciseName,
                Reps = Exercise.Reps,
                sets = Exercise.sets,
                BodyPart = Exercise.BodyPart,
                YouTubeUrl = Exercise.YouTubeUrl
            };

            return ExerciseDto;
        }

        /// <summary>
        /// Adds a new exercise to the database.
        /// </summary>
        /// <param name="exerciseDto">The ExerciseDto object containing the details of the exercise to be added.</param>
        /// <returns>An IHttpActionResult indicating the result of the operation.</returns>
        /// <example>
        /// POST: api/ExerciseData/AddExercise
        /// </example>
        [HttpPost]
        [Route("api/ExerciseData/AddExercise")]
        public IHttpActionResult AddExercise(ExerciseDto exerciseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Exercise exercise = new Exercise
            {
                ExerciseName = exerciseDto.ExerciseName,
                Reps = exerciseDto.Reps,
                sets = exerciseDto.sets,
                BodyPart = exerciseDto.BodyPart,
                YouTubeUrl = exerciseDto.YouTubeUrl
            };

            db.Exercises.Add(exercise);
            db.SaveChanges();

            return Ok(exerciseDto);
        }

        /// <summary>
        /// Updates an existing exercise in the database.
        /// </summary>
        /// <param name="id">The ID of the exercise to be updated.</param>
        /// <param name="exerciseDto">The ExerciseDto object containing the updated details of the exercise.</param>
        /// <returns>An IHttpActionResult indicating the result of the operation.</returns>
        /// <example>
        /// PUT: api/ExerciseData/UpdateExercise/{id}
        /// </example>
        [HttpPut]
        [Route("api/ExerciseData/UpdateExercise/{id}")]
        public IHttpActionResult UpdateExercise(int id, ExerciseDto exerciseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var exercise = db.Exercises.Find(id);
            if (exercise == null)
            {
                return NotFound();
            }

            exercise.ExerciseName = exerciseDto.ExerciseName;
            exercise.Reps = exerciseDto.Reps;
            exercise.sets = exerciseDto.sets;
            exercise.BodyPart = exerciseDto.BodyPart;
            exercise.YouTubeUrl = exerciseDto.YouTubeUrl;

            db.SaveChanges();

            return Ok(exerciseDto);
        }

        /// <summary>
        /// Deletes an exercise from the database.
        /// </summary>
        /// <param name="id">The ID of the exercise to be deleted.</param>
        /// <returns>An IHttpActionResult indicating the result of the operation.</returns>
        /// <example>
        /// DELETE: api/ExerciseData/DeleteExercise/{id}
        /// </example>
        [HttpDelete]
        [Route("api/ExerciseData/DeleteExercise/{id}")]
        public IHttpActionResult DeleteExercise(int id)
        {
            var exercise = db.Exercises.Find(id);
            if (exercise == null)
            {
                return NotFound();
            }

            db.Exercises.Remove(exercise);
            db.SaveChanges();

            return Ok();
        }
    }
}