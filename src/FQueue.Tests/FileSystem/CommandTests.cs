using System;
using FQueue.FileSystem;
using NUnit.Framework;

namespace FQueue.Tests.FileSystem
{
    [TestFixture]
    public class CommandTests
    {
        private class CommandTester : Command
        {
            private readonly Func<bool> _executeAction;
            private readonly Action _rollbackAction;

            public CommandTester(Func<bool> executeAction, Action rollbackAction, bool inputDataSet)
                : base(Guid.NewGuid().ToString())
            {
                _executeAction = executeAction;
                _rollbackAction = rollbackAction;
                InputDataSet = inputDataSet;
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

        [TestCase(true)]
        [TestCase(false)]
        public void Execute(bool executeActionResult)
        {
            bool executeAction = false;
            bool rollbackAction = false;
            CommandTester tester = new CommandTester(() =>
            {
                {
                    executeAction = true;
                    return executeActionResult;
                }
            }, () => { rollbackAction = true; }, true);


            Assert.AreEqual(executeActionResult, tester.Execute());
            Assert.IsTrue(executeAction);
            Assert.IsFalse(rollbackAction);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Rollback(bool executeActionResult)
        {
            bool executeAction = false;
            bool rollbackAction = false;
            CommandTester tester = new CommandTester(() =>
            {
                {
                    executeAction = true;
                    return executeActionResult;
                }
            }, () => { rollbackAction = true; }, true);


            Assert.AreEqual(executeActionResult, tester.Execute());
            tester.Rollback();
            Assert.IsTrue(executeAction);
            Assert.IsTrue(rollbackAction);
        }

        [Test]
        public void CanNotExecuteTwice()
        {
            CommandTester tester = new CommandTester(() => true, () => { }, true);

            tester.Execute();

            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => tester.Execute());
            Assert.IsTrue(exception.Message.Contains(tester.Name));
        }

        [Test]
        public void CanNotExecuteRolledBack()
        {
            CommandTester tester = new CommandTester(() => true, () => { }, true);

            tester.Execute();
            tester.Rollback();

            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => tester.Execute());
            Assert.IsTrue(exception.Message.Contains(tester.Name));
        }

        [Test]
        public void CanNotExecuteWithoutInputData()
        {
            CommandTester tester = new CommandTester(() => true, () => { }, false);

            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => tester.Execute());
            Assert.IsTrue(exception.Message.Contains(tester.Name));
        }

        [Test]
        public void CanNotRollBackIfNotExecuted()
        {
            CommandTester tester = new CommandTester(() => true, () => { }, true);

            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => tester.Rollback());
            Assert.IsTrue(exception.Message.Contains(tester.Name));
        }
    }
}