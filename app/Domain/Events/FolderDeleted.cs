using Damascus.Core;
using Damascus.Domain.Abstractions;

namespace Damascus.Example.Domain
{
    public class FolderDeleted : IDomainEvent
    {
        public FolderDeleted(string path)
        {
            path.BetterNotBeNull("Path");

            Path = path;
        }

        public string Path { get; }
    }
}
