using System;

namespace FQueue.FileSystem
{
    public abstract class Command : ICommand
    {
#warning TODO - unit tests
        public string Name { get; }
        public bool WasExecuted { get; private set; }
        public bool WasRolledBack { get; private set; }
        public bool InputDataSet { get; protected set; }

        protected Command(string name)
        {
            Name = name;
        }

        protected abstract bool ExecuteWithNoGuard();
        protected abstract void RollbackWithNoGuard();

        public bool Execute()
        {
            if (WasExecuted)
            {
                throw new InvalidOperationException("Command has already been executed");
            }

            if (WasRolledBack)
            {
                throw new InvalidOperationException("Command has already been rolled back");
            }

            if (!InputDataSet)
            {
                throw new InvalidOperationException("Command has no input data set (even if it's no data)");
            }

            WasExecuted = true;

            return ExecuteWithNoGuard();
        }

        public void Rollback()
        {
            if (!WasExecuted)
            {
                throw new InvalidOperationException("Command has not been executed");
            }

            WasRolledBack = true;

            RollbackWithNoGuard();
        }
    }
}