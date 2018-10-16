namespace IffleyRoutesRecord.DatabaseOptions
{
    /// <summary>
    /// Options for populating the database with data on startup
    /// </summary>
    internal enum DatabasePrePopulationOptions
    {
        /// <summary>
        /// No data will be added
        /// </summary>
        None,

        /// <summary>
        /// Only static data (holds, grades, and style symbols) will be added
        /// </summary>
        OnlyStaticData,

        /// <summary>
        /// Static data and problems will be added
        /// </summary>
        StaticDataAndExistingProblems
    }
}
