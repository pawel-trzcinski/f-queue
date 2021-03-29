using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FQueue.FileSystem;
using NUnit.Framework;

namespace FQueue.Tests.FileSystem
{
    [TestFixture]
    public class CommandChainTests
    {
        private class TestCommand : Command
        {
            private readonly Func<bool> _executeAction;
            private readonly Action _rollbackAction;

            public TestCommand(string name, Func<bool> executeAction, Action rollbackAction)
                : base(name)
            {
                _executeAction = executeAction;
                _rollbackAction = rollbackAction;
                InputDataSet = true;
            }

            protected override bool ExecuteWithNoGuard()
            {
                return _executeAction();
            }

            protected override void RollbackWithNoGuard()
            {
                _rollbackAction();
            }
        }


        [TestCase(1, -1)]
        [TestCase(1, 0)]
        [TestCase(2, -1)]
        [TestCase(2, 0)]
        [TestCase(2, 1)]
        [TestCase(3, -1)]
        [TestCase(3, 0)]
        [TestCase(3, 1)]
        [TestCase(3, 2)]
        [TestCase(10, -1)]
        [TestCase(10, 0)]
        [TestCase(10, 5)]
        [TestCase(10, 9)]
        [TestCase(100, -1)]
        [TestCase(100, 0)]
        [TestCase(100, 13)]
        [TestCase(100, 66)]
        [TestCase(100, 99)]
        public void ExecuteAndRollback(int numberOfCommands, int falseExecutionIndex)
        {
            Fixture fixture = new Fixture();

            CommandChain commandChain = new CommandChain();
            Assert.IsFalse(commandChain.InputDataSet);
            Assert.IsFalse(commandChain.WasExecuted);
            Assert.IsFalse(commandChain.WasRolledBack);
            Assert.IsNotNull(commandChain.Name);

            List<string> names = new List<string>(numberOfCommands);
            List<TestCommand> commands = new List<TestCommand>(numberOfCommands);
            List<string> executionNames = new List<string>(numberOfCommands);
            List<string> rollbackNames = new List<string>(numberOfCommands);
            for (int i = 0; i < numberOfCommands; i++)
            {
                string name = fixture.Create<string>();
                names.Add(name);

                bool executionOk = i != falseExecutionIndex;

                TestCommand command = new TestCommand
                (
                    name,
                    () =>
                    {
                        executionNames.Add(name);
                        return executionOk;
                    },
                    () => { rollbackNames.Add(name); }
                );

                commandChain.Add(command);
                commands.Add(command);
            }

            Assert.IsTrue(commandChain.InputDataSet);
            Assert.IsFalse(commandChain.WasExecuted);
            Assert.IsFalse(commandChain.WasRolledBack);

            Assert.AreEqual(falseExecutionIndex == -1, commandChain.Execute());

            Assert.IsTrue(commandChain.WasExecuted);
            Assert.IsFalse(commandChain.WasRolledBack);
            Assert.AreEqual(falseExecutionIndex == -1 ? numberOfCommands : falseExecutionIndex + 1, executionNames.Count);
            Assert.IsTrue(executionNames.SequenceEqual(names.Take(executionNames.Count)));
            Assert.AreEqual(0, rollbackNames.Count);
            Assert.AreEqual(executionNames.Count, commands.Take(executionNames.Count).Count(p => p.WasExecuted));
            Assert.AreEqual(numberOfCommands - executionNames.Count, commands.Skip(executionNames.Count).Count(p => !p.WasExecuted));

            commandChain.Rollback();

            Assert.IsTrue(commandChain.WasExecuted);
            Assert.IsTrue(commandChain.WasRolledBack);
            Assert.AreEqual(executionNames.Count, rollbackNames.Count);
            Assert.IsTrue(executionNames.SequenceEqual(((IEnumerable<string>) rollbackNames).Reverse()));
            Assert.AreEqual(rollbackNames.Count, commands.Take(rollbackNames.Count).Count(p => p.WasRolledBack));
            Assert.AreEqual(numberOfCommands - rollbackNames.Count, commands.Skip(rollbackNames.Count).Count(p => !p.WasRolledBack));
        }
    }
}