using System;
using System.Collections.Generic;
using Damascus.Core;
using Damascus.Domain.Abstractions;

namespace Damascus.Example.Domain
{
    public class Folder : Entity<string>, IFolderContent
    {
        public Folder(string path, IEnumerable<IFolderContent> contents) : base(path)
        {
            contents.BetterNotBeNull("Contents");
            contents.BetterHaveNoNulls("Contents");
            path.BetterNotBeNull("Path");

            Contents = contents;
            Path = path;
        }

        public string Path { get; }
        public IEnumerable<IFolderContent> Contents { get; }

        public IEnumerable<IDomainEvent> Delete()
        {
            foreach (var item in Contents)
            {
                foreach (var e in item.Delete())
                {
                    yield return e;
                }
            }

            yield return new FolderDeleted(this.Path);
        }

        public Maybe<IFolderContent> Find(Predicate<IFolderContent> callback)
        {
            if (callback(this))
            {
                return this;
            }

            foreach(var item in Contents)
            {
                if (callback(item))
                {
                    return item.ToMaybe<IFolderContent>();
                }
            }

            return Maybe.Nothing;
        }
    }
}
