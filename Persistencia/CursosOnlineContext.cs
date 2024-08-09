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

        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<CursoInstructor> CursoInstructors { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Precio> Precios { get; set; }
        
    }
}
