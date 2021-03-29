using System.Collections.Generic;

namespace FQueue.FileSystem
{
    public class CommandChain : Command, ICommandChain
    {
        private readonly List<ICommand> _commands = new List<ICommand>();

        public CommandChain()
            : base(nameof(CommandChain))
        {
        }

        protected override bool ExecuteWithNoGuard()
        {
            foreach (ICommand command in _commands)
            {
                if (!command.Execute())
                {
                    return false;
                }
            }

            return true;
        }

        protected override void RollbackWithNoGuard()
        {
            for (int i = _commands.Count - 1; i >= 0; --i)
            {
                ICommand command = _commands[i];

                if (command.WasExecuted)
                {
                    command.Rollback();
                }
            }
        }

        public void Add(ICommand command)
        {
            _commands.Add(command);
            InputDataSet = true;
        }
    }
}