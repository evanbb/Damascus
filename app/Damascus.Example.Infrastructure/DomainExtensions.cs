using Damascus.Domain.Abstractions;
using Damascus.Example.Contracts;
using Damascus.Example.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Damascus.Example.Infrastructure
{
    public static class DomainExtensions
    {
        public static BookmarksCollection ToContract(this MutableBookmarksCollection collection)
        {
            IEnumerable<IBookmarkItem> GetContractItems(IEnumerable<IFolderContent> domainItems)
            {
                foreach(var item in domainItems)
                {
                    if (item is MutableBookmark)
                    {
                        yield return ((MutableBookmark)item).ToContract();
                    }
                    if (item is MutableFolder)
                    {
                        yield return ((MutableFolder)item).ToContract();

                        foreach (var x in GetContractItems(((MutableFolder)item).Contents))
                        {
                            yield return x;
                        }
                    }
                }
            }

            return new BookmarksCollection(collection.Id, GetContractItems(collection.Contents));
        }

        public static Bookmark ToContract(this MutableBookmark bookmark)
        {
            return new Bookmark(bookmark.Id, bookmark.Name, bookmark.Uri.ToString());
        }

        public static Folder ToContract(this MutableFolder folder)
        {
            return new Folder(folder.Id, folder.Name, folder.Contents.Select(c => c.Id));
        }
    }
}
