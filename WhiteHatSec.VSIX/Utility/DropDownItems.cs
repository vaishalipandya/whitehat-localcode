namespace WhiteHatSec.VSIX.Utility
{
    /// <summary>
    /// </summary>
    public enum State
    {
        /// <summary>
        ///     The open.
        /// </summary>
        Open = 0,

        /// <summary>
        ///     The closed.
        /// </summary>
        Closed = 1,

        /// <summary>
        ///     The both.
        /// </summary>
        Both = 2
    }

    /// <summary>
    /// </summary>
    public enum Severity
    {
        /// <summary>
        ///     All.
        /// </summary>
        All = 0,

        /// <summary>
        ///     The critical.
        /// </summary>
        Critical = 1,

        /// <summary>
        ///     The high.
        /// </summary>
        High = 2,

        /// <summary>
        ///     The medium.
        /// </summary>
        Medium = 3,

        /// <summary>
        ///     The low.
        /// </summary>
        Low = 4,

        /// <summary>
        ///     The informational.
        /// </summary>
        Note = 5
    }
}