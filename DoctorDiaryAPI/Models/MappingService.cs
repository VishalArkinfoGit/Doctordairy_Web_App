using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace DoctorDiaryAPI.Models
{
    public class MappingService
    {
        /// <summary>
        /// Converts model from class F to class T
        /// </summary>
        /// <typeparam name="T">To Class</typeparam>
        /// <typeparam name="F">From Class</typeparam>
        /// <returns>model of type class T</returns>


        public TDestination Map<TSource, TDestination>(TSource obj)
        {
            var Config = new AutoMapper.MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<TDestination, TSource>();
                    cfg.ValidateInlineMaps = false;
                });

            var mapper = Config.CreateMapper();

            return mapper.Map<TDestination>(obj);
        }
    }
}