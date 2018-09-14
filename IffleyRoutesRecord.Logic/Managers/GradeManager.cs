using IffleyRoutesRecord.Logic.DataAccess;
using IffleyRoutesRecord.Logic.DTOs.Responses;
using IffleyRoutesRecord.Logic.Entities;
using IffleyRoutesRecord.Logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
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

            if (grade == null)
            {
                return null;
            }

            return CreateTechGradeResponse(grade);
        }

        public BGradeResponse GetBGrade(int gradeId)
        {
            if (cache.TryRetrieveItemWithId<BGradeResponse>(gradeId, techGrade => techGrade.GradeId, out var gradeResponse))
            {
                return gradeResponse;
            }

            var grade = repository.BGrade.SingleOrDefault(gradeDbo => gradeDbo.Id == gradeId);

            if (grade == null)
            {
                return null;
            }

            return CreateBGradeResponse(grade);
        }

        public PoveyGradeResponse GetPoveyGrade(int gradeId)
        {
            if (cache.TryRetrieveItemWithId<PoveyGradeResponse>(gradeId, techGrade => techGrade.GradeId, out var gradeResponse))
            {
                return gradeResponse;
            }

            var grade = repository.PoveyGrade.SingleOrDefault(gradeDbo => gradeDbo.Id == gradeId);

            if (grade == null)
            {
                return null;
            }

            return CreatePoveyGradeResponse(grade);
        }

        public FurlongGradeResponse GetFurlongGrade(int gradeId)
        {
            if (cache.TryRetrieveItemWithId<FurlongGradeResponse>(gradeId, techGrade => techGrade.GradeId, out var gradeResponse))
            {
                return gradeResponse;
            }

            var grade = repository.FurlongGrade.SingleOrDefault(gradeDbo => gradeDbo.Id == gradeId);

            if (grade == null)
            {
                return null;
            }

            return CreateFurlongGradeResponse(grade);
        }

        public IList<TechGradeResponse> GetTechGrades()
        {
            if (cache.TryRetrieveAllItems<TechGradeResponse>(out var grades))
            {
                return grades.ToList();
            }

            grades = repository.TechGrade
                .Select(CreateTechGradeResponse)
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
                .Select(CreateBGradeResponse)
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
                .Select(CreatePoveyGradeResponse)
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
                .Select(CreateFurlongGradeResponse)
                .OrderBy(grade => grade.Rank);

            cache.CacheListOfItems(grades, CacheItemPriority.High);
            return grades.ToList();
        }

        public TechGradeResponse GetTechGradeOnProblem(int problemId)
        {
            var problem = repository.Problem
                .Include(prob => prob.TechGrade)
                .SingleOrDefault(prob => prob.Id == problemId);

            if (problem.TechGradeId is null)
            {
                return null;
            }

            return new TechGradeResponse()
            {
                GradeId = problem.TechGradeId.Value,
                Name = problem.TechGrade.Name,
                Rank = problem.TechGrade.Rank
            };
        }

        public BGradeResponse GetBGradeOnProblem(int problemId)
        {
            var problem = repository.Problem
                .Include(prob => prob.BGrade)
                .SingleOrDefault(prob => prob.Id == problemId);

            if (problem.BGradeId is null)
            {
                return null;
            }

            return new BGradeResponse()
            {
                GradeId = problem.BGradeId.Value,
                Name = problem.BGrade.Name,
                Rank = problem.BGrade.Rank
            };
        }

        public PoveyGradeResponse GetPoveyGradeOnProblem(int problemId)
        {
            var problem = repository.Problem
                .Include(prob => prob.PoveyGrade)
                .SingleOrDefault(prob => prob.Id == problemId);

            if (problem.PoveyGradeId is null)
            {
                return null;
            }

            return new PoveyGradeResponse()
            {
                GradeId = problem.PoveyGradeId.Value,
                Name = problem.PoveyGrade.Name,
                Rank = problem.PoveyGrade.Rank
            };
        }

        public FurlongGradeResponse GetFurlongGradeOnProblem(int problemId)
        {
            var problem = repository.Problem
                .Include(prob => prob.FurlongGrade)
                .SingleOrDefault(prob => prob.Id == problemId);

            if (problem.FurlongGradeId is null)
            {
                return null;
            }

            return new FurlongGradeResponse()
            {
                GradeId = problem.FurlongGradeId.Value,
                Name = problem.FurlongGrade.Name,
                Rank = problem.FurlongGrade.Rank
            };
        }

        private TechGradeResponse CreateTechGradeResponse(TechGrade grade)
        {
            if (grade is null)
            {
                throw new ArgumentNullException(nameof(grade));
            }

            return new TechGradeResponse()
            {
                GradeId = grade.Id,
                Name = grade.Name,
                Rank = grade.Rank
            };
        }

        private BGradeResponse CreateBGradeResponse(BGrade grade)
        {
            if (grade is null)
            {
                throw new ArgumentNullException(nameof(grade));
            }

            return new BGradeResponse()
            {
                GradeId = grade.Id,
                Name = grade.Name,
                Rank = grade.Rank
            };
        }

        private PoveyGradeResponse CreatePoveyGradeResponse(PoveyGrade grade)
        {
            if (grade is null)
            {
                throw new ArgumentNullException(nameof(grade));
            }

            return new PoveyGradeResponse()
            {
                GradeId = grade.Id,
                Name = grade.Name,
                Rank = grade.Rank
            };
        }

        private FurlongGradeResponse CreateFurlongGradeResponse(FurlongGrade grade)
        {
            if (grade is null)
            {
                throw new ArgumentNullException(nameof(grade));
            }

            return new FurlongGradeResponse()
            {
                GradeId = grade.Id,
                Name = grade.Name,
                Rank = grade.Rank
            };
        }
    }
}
