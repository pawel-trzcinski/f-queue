using System;
using Newtonsoft.Json;

namespace FQueue.FileSystem.VersionFile
{
    [Serializable]
    public class VersionData
    {
#warning TODO - unit tests
        public Guid Id { get; }
        public System.DateTime UtcTime { get; }
        public string CurrentFile { get; }
        public long CurrentPointer { get; }
        public string LastFile { get; }
        public long LastPointer { get; }

        [JsonConstructor]
        public VersionData(Guid id, System.DateTime utcTime, string currentFile, long currentPointer, string lastFile, long lastPointer)
        {
            Id = id;
            UtcTime = utcTime;
            CurrentFile = currentFile;
            CurrentPointer = currentPointer;
            LastFile = lastFile;
            LastPointer = lastPointer;
        }

        public VersionData(string currentFile, long currentPointer, string lastFile, long lastPointer)
        {
            Id = Guid.NewGuid();
            UtcTime = System.DateTime.UtcNow;
            CurrentFile = currentFile;
            CurrentPointer = currentPointer;
            LastFile = lastFile;
            LastPointer = lastPointer;
        }
    }
}