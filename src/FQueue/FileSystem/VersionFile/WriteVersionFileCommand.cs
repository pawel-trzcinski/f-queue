using System;
using Newtonsoft.Json;

namespace FQueue.FileSystem.VersionFile
{
    public class WriteVersionFileCommand : Command, IWriteVersionFileCommand
    {
#warning TODO - unit tests
        private readonly IFileAbstraction _fileAbstraction;

        private string _filename;
        private VersionData _versionData;

        private string _previousState;

        public WriteVersionFileCommand(IFileAbstraction fileAbstraction)
            : base(nameof(WriteVersionFileCommand))
        {
            _fileAbstraction = fileAbstraction;
        }

        public void SetInputData(string filename, VersionData versionData)
        {
            if (String.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentNullException(nameof(filename));
            }

            if (versionData == null)
            {
                throw new ArgumentNullException(nameof(versionData));
            }

            _filename = filename;
            _versionData = versionData;

            InputDataSet = true;
        }

        protected override bool ExecuteWithNoGuard()
        {
            _previousState = _fileAbstraction.ReadAllText(_filename);
            _fileAbstraction.WriteAllText(_filename, JsonConvert.SerializeObject(_versionData));
            return true;
        }

        protected override void RollbackWithNoGuard()
        {
            _fileAbstraction.WriteAllText(_filename, _previousState);
        }
    }
}