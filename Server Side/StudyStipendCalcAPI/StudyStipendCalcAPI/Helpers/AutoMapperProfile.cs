using System;
using AutoMapper;
using StudyStipendCalcAPI.DTOs;
using StudyStipendCalcAPI.DTOs.StudentDtos;
using StudyStipendCalcAPI.DTOs.UserDTOs;
using StudyStipendCalcAPI.Models;

namespace StudyStipendCalcAPI.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Student, StudentDto>();
            CreateMap<StudentDto, Student>();
            //CreateMap<Student, StudentResponseDto>();
            //CreateMap<StudentResponseDto, Student>();
            CreateMap<LoginUserDto, User>();
            CreateMap<RegisterUserDto, User>();
            CreateMap<UserResponseDto, User>();
            CreateMap<User, UserResponseDto>();
            CreateMap<UserModifyDto, User>();
            CreateMap<Universities, UniversitiesDto>();
            CreateMap<UniversitiesDto, Universities>();
            CreateMap<Roles, RolesDto>();
            CreateMap<RolesDto, Roles>();
            CreateMap<Student, StudentGenerateDto>();

        }
    }
}
