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
                    BodyPart = Exercise.BodyPart
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
            var query = $"SELECT * FROM Exercises WHERE ExerciseName LIKE '%{searchString}%'";
            var exercises = db.Exercises.SqlQuery(query).ToList();

            List<ExerciseDto> ExerciseDtos = new List<ExerciseDto>();

            foreach (Exercise exercise in exercises)
            {
                ExerciseDto exerciseDto = new ExerciseDto
                {
                    ExerciseId = exercise.ExerciseId,
                    ExerciseName = exercise.ExerciseName,
                    Reps = exercise.Reps,
                    sets = exercise.sets,
                    BodyPart = exercise.BodyPart
                };

                ExerciseDtos.Add(exerciseDto);
            }

            return ExerciseDtos;
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
                BodyPart = Exercise.BodyPart
            };

            return ExerciseDto;
        }
    }
}