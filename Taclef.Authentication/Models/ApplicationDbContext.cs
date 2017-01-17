using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;

namespace Taclef.Authentication.Models
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(string nameOrConnection) : base(nameOrConnection)
		{
			
		}

		public ApplicationDbContext() : this("DefaultConnection")
		{
			
		}

		public static ApplicationDbContext Create()
		{
			return new ApplicationDbContext();
		}

		public DbSet<ApplicationUser> ApplicationUsers { get; set; }
		public DbSet<ApplicationRole> ApplicationRoles { get; set; }
		public DbSet<LoginProvider> LoginProviders { get; set; }
		public DbSet<ConsumerApplication> ConsumerApplications { get; set; }
		public DbSet<UserLogin> UserLogins { get; set; }
		public DbSet<SchoolBoard> Boards { get; set; }
		public DbSet<School> Schools { get; set; }

		public T Create<T>(Action<T> initializer = null) where T : UuidEntity 
		{
			var set = Set<T>();
			if (set == null) throw new ApplicationException("No DbSet of type " + typeof(T).AssemblyQualifiedName);
			var entity = set.Create();
			entity.Uuid = Guid.NewGuid();
			if (initializer != null)
			{
				initializer(entity);
			}
			return entity;
		}

		public T CreateAdd<T>(Action<T> initializer = null) where T : UuidEntity
		{
			var set = Set<T>();
			if (set == null) throw new ApplicationException("No DbSet of type " + typeof(T).AssemblyQualifiedName);
			var entity = set.Create();
			entity.Uuid = Guid.NewGuid();
			if (initializer != null)
			{
				initializer(entity);
			}
			set.Add(entity);
			return entity;
		}

		public T CreateSave<T>(Action<T> initializer = null) where T : UuidEntity
		{
			var entity = CreateAdd(initializer);
			SaveChanges();
			return entity;
		}

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();

            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {



                    ApplicationLogger.LogError(
                        string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        ApplicationLogger.LogError(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage));
                    }
                }
                throw;
            }
            catch (Exception exception)
            {
                ApplicationLogger.LogError(exception);
                throw;
            }


        }
	}

    public class ApplicationLogger
    {
        private static readonly string FilePath = HttpContext.Current.Server.MapPath("~/App_Data/App.log");
        public static void LogError(string logContent)
        {

            using (StreamWriter writer = File.AppendText(FilePath))
            {
                writer.WriteLine(logContent);
                writer.Flush();
            }

        }



        public static void LogError(Exception exception)
        {

            using (StreamWriter writer = File.AppendText(FilePath))
            {
                writer.WriteLine(exception.ToString());
                writer.Flush();
            }
        }
    }
}