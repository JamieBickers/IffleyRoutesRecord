using IffleyRoutesRecord.Models.DTOs.Responses;
using System.Collections.Generic;

namespace IffleyRoutesRecord.Logic.Interfaces
{
    /// <summary>
    /// Basic CRUD opreations for style symbols
    /// </summary>
    public interface IStyleSymbolManager
    {
        /// <summary>
        /// Gets a style symbol by its ID
        /// </summary>
        /// <param name="styleSymbolId">The style symbol ID</param>
        /// <returns>Te style symbol</returns>
        StyleSymbolResponse GetStyleSymbol(int styleSymbolId);

        /// <summary>
        /// Gets all style symbols
        /// </summary>
        /// <returns>A list of all style symbols</returns>
        IEnumerable<StyleSymbolResponse> GetStyleSymbols();
    }
}