using Dominio;
using Microsoft.EntityFrameworkCore;


namespace Persistencia
{
    public class CursosOnlineContext : DbContext
    {
        public CursosOnlineContext(DbContextOptions options) : base(options)
        {
        }

        // Crear las tablas en la base de datos
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //indicando que la tabla Curso tiene una llave primaria compuesta
            modelBuilder.Entity<CursoInstructor>().HasKey(ci => new { ci.InstructorId, ci.CursoId });
        }

        public DbSet<Curso> Curso { get; set; }
        public DbSet<Comentario> Comentario { get; set; }
        public DbSet<CursoInstructor> CursoInstructor { get; set; }
        public DbSet<Instructor> Instructor { get; set; }
        public DbSet<Precio> Precio { get; set; }
        
    }
}
