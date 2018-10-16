namespace IffleyRoutesRecord.DatabaseOptions
{
    /// <summary>
    /// The type of database registered at startup
    /// </summary>
    internal enum DatabaseType
    {
        /// <summary>
        /// An actual persistent SQL database
        /// </summary>
        Real,

        /// <summary>
        /// An in memory database without persistence
        /// </summary>
        Memory
    }
}
