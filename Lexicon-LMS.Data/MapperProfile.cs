using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Lexicon_LMS.Core.Entities.ViewModel;
using Lexicon_LMS.Core.Entities;

namespace Lexicon_LMS.Data
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<TeacherModuleViewModel, Module>();
            CreateMap<TeacherViewModel, Activity>();
            CreateMap<ActivityListViewModel, Activity>();
            CreateMap<Activity, ActivityListViewModel>();
            CreateMap<CourseViewModel, Course>();
            CreateMap<Course, CourseViewModel>();

            CreateMap<User, StudentViewModel>();
            CreateMap<User, StudentCourseViewModel>();
            CreateMap<Module, ModuleViewModel>();
            CreateMap<Document, DocumentViewModel>();

        }

    }
}

