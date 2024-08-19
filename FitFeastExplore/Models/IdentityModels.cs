using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FitFeastExplore.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Tour> Tours { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<WorkOut> WorkOuts { get; set; }
        public DbSet<WorkOutPlan> WorkOutPlans { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Ensure this is called first

            // Configuring the many-to-many relationship between Recipe and Ingredient
            modelBuilder.Entity<Recipe>()
                        .HasMany(r => r.Ingredients)
                        .WithMany(i => i.Recipes)
                        .Map(m =>
                        {
                            m.ToTable("RecipeIngredients");
                            m.MapLeftKey("RecipeId");
                            m.MapRightKey("IngredientId");
                        });
        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}