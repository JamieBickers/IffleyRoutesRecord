using IffleyRoutesRecord.Logic.DataAccess;
using IffleyRoutesRecord.Logic.Exceptions;
using IffleyRoutesRecord.Logic.Interfaces;
using IffleyRoutesRecord.Logic.StaticHelpers;
using IffleyRoutesRecord.Models.DTOs.Responses;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;

namespace IffleyRoutesRecord.Logic.Managers
{
    public class GradeManager : IGradeManager
    {
        private readonly IffleyRoutesRecordContext repository;
        private readonly IMemoryCache cache;

        public GradeManager(IffleyRoutesRecordContext repository, IMemoryCache cache)
        {
            this.repository = repository;
            this.cache = cache;
        }

        public TechGradeResponse GetTechGrade(int gradeId)
        {
            if (cache.TryRetrieveItemWithId<TechGradeResponse>(gradeId, techGrade => techGrade.GradeId, out var gradeResponse))
            {
                return gradeResponse;
            }

            var grade = repository.TechGrade.SingleOrDefault(gradeDbo => gradeDbo.Id == gradeId);

            if (grade is null)
            {
                throw new EntityNotFoundException($"No tech grade with ID {gradeId} was found.");
            }

            return Mapper.Map(grade);
        }

        public BGradeResponse GetBGrade(int gradeId)
        {
            if (cache.TryRetrieveItemWithId<BGradeResponse>(gradeId, techGrade => techGrade.GradeId, out var gradeResponse))
            {
                return gradeResponse;
            }

            var grade = repository.BGrade.SingleOrDefault(gradeDbo => gradeDbo.Id == gradeId);

            if (grade is null)
            {
                throw new EntityNotFoundException($"No B grade with ID {gradeId} was found.");
            }

            return Mapper.Map(grade);
        }

        public PoveyGradeResponse GetPoveyGrade(int gradeId)
        {
            if (cache.TryRetrieveItemWithId<PoveyGradeResponse>(gradeId, techGrade => techGrade.GradeId, out var gradeResponse))
            {
                return gradeResponse;
            }

            var grade = repository.PoveyGrade.SingleOrDefault(gradeDbo => gradeDbo.Id == gradeId);

            if (grade is null)
            {
                throw new EntityNotFoundException($"No Povey grade with ID {gradeId} was found.");
            }

            return Mapper.Map(grade);
        }

        public FurlongGradeResponse GetFurlongGrade(int gradeId)
        {
            if (cache.TryRetrieveItemWithId<FurlongGradeResponse>(gradeId, techGrade => techGrade.GradeId, out var gradeResponse))
            {
                return gradeResponse;
            }

            var grade = repository.FurlongGrade.SingleOrDefault(gradeDbo => gradeDbo.Id == gradeId);

            if (grade is null)
            {
                throw new EntityNotFoundException($"No Furlong grade with ID {gradeId} was found.");
            }

            return Mapper.Map(grade);
        }

        public IList<TechGradeResponse> GetTechGrades()
        {
            if (cache.TryRetrieveAllItems<TechGradeResponse>(out var grades))
            {
                return grades.ToList();
            }

            grades = repository.TechGrade
                .Select(Mapper.Map)
                .OrderBy(grade => grade.Rank);

            cache.CacheListOfItems(grades, CacheItemPriority.High);
            return grades.ToList();
        }

        public IList<BGradeResponse> GetBGrades()
        {
            if (cache.TryRetrieveAllItems<BGradeResponse>(out var grades))
            {
                return grades.ToList();
            }

            grades = repository.BGrade
                .Select(Mapper.Map)
                .OrderBy(grade => grade.Rank);

            cache.CacheListOfItems(grades, CacheItemPriority.High);
            return grades.ToList();
        }

        public IList<PoveyGradeResponse> GetPoveyGrades()
        {
            if (cache.TryRetrieveAllItems<PoveyGradeResponse>(out var grades))
            {
                return grades.ToList();
            }

            grades = repository.PoveyGrade
                .Select(Mapper.Map)
                .OrderBy(grade => grade.Rank);

            cache.CacheListOfItems(grades, CacheItemPriority.High);
            return grades.ToList();
        }

        public IList<FurlongGradeResponse> GetFurlongGrades()
        {
            if (cache.TryRetrieveAllItems<FurlongGradeResponse>(out var grades))
            {
                return grades.ToList();
            }

            grades = repository.FurlongGrade
                .Select(Mapper.Map)
                .OrderBy(grade => grade.Rank);

            cache.CacheListOfItems(grades, CacheItemPriority.High);
            return grades.ToList();
        }
    }
}
