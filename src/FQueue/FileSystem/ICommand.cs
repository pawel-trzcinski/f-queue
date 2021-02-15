namespace FQueue.FileSystem
{
    public interface ICommand
    {
        /// <summary>
        /// Indicates if execution was started for this command
        /// </summary>
        bool WasExecuted { get; }

        /// <summary>
        /// Indicates if rollback was started for this command
        /// </summary>
        bool WasRolledBack { get; }

        /// <returns><b>true</b> if execution was successfull</returns>
        bool Execute();
        void Rollback();
    }
}