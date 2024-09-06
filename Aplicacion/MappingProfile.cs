using Aplicacion.Cursos;
using AutoMapper;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Curso,CursoDto>()
                //Para que el mapeo desde la base de datos a la clase CursoDto incluya los instructores
            .ForMember(x=>x.Instructores, y=>y.MapFrom(z=>z.InstructoresLink.Select(a=>a.Instructor).ToList()));

            CreateMap<CursoInstructor, CursoInstructorDto>();
            CreateMap<Instructor,InstructorDto>();
            
            
           
        }
    }
}
