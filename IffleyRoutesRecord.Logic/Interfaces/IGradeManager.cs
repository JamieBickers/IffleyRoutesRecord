using IffleyRoutesRecord.Models.DTOs.Responses;
using System.Collections.Generic;

namespace IffleyRoutesRecord.Logic.Interfaces
{
    /// <summary>
    /// Basic CRUD opreations for grades
    /// </summary>
    public interface IGradeManager
    {
        /// <summary>
        /// Gets the Tech grade with the given ID
        /// </summary>
        /// <param name="gradeId">The ID of the grade to get</param>
        /// <returns>The Tech grade</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        TechGradeResponse GetTechGrade(int gradeId);

        /// <summary>
        /// Gets the B grade with the given ID
        /// </summary>
        /// <param name="gradeId">The ID of the grade to get</param>
        /// <returns>The B grade</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        BGradeResponse GetBGrade(int gradeId);

        /// <summary>
        /// Gets the Povey grade with the given ID
        /// </summary>
        /// <param name="gradeId">The ID of the grade to get</param>
        /// <returns>The Povey grade</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        PoveyGradeResponse GetPoveyGrade(int gradeId);

        /// <summary>
        /// Gets the Furlong grade with the given ID
        /// </summary>
        /// <param name="gradeId">The ID of the grade to get</param>
        /// <returns>The Furlong grade</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        FurlongGradeResponse GetFurlongGrade(int gradeId);

        /// <summary>
        /// Gets all Tech grades
        /// </summary>
        /// <returns>All Tech grades</returns>
        IList<TechGradeResponse> GetTechGrades();

        /// <summary>
        /// Gets all B grades
        /// </summary>
        /// <returns>All B grades</returns>
        IList<BGradeResponse> GetBGrades();

        /// <summary>
        /// Gets all Povey grades
        /// </summary>
        /// <returns>All Povey grades</returns>
        IList<PoveyGradeResponse> GetPoveyGrades();

        /// <summary>
        /// Gets all Furlong grades
        /// </summary>
        /// <returns>All Furlong grades</returns>
        IList<FurlongGradeResponse> GetFurlongGrades();
    }
}