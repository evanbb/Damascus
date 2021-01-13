using Damascus.Core;
using Damascus.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Damascus.Example.Domain
{
    public class MutableBookmarksCollection : AggregateRootBase<Guid>
    {
        #region ctor

        private MutableBookmarksCollection(
            Guid id,
            IEnumerable<IFolderContent> contents
        ) : base(id)
        {
            id.BetterNotBe(Guid.Empty, "Cannot create bookmarks collection with an empty ID");

            contents.BetterNotBeNull("Bookmarks");
            contents.BetterNotHaveNulls("Bookmarks");

            RootFolder = MutableFolder.CreateRoot(contents);
        }

        private MutableFolder RootFolder { get; }

        public static MutableBookmarksCollection CreateNew()
        {
            var id = Guid.NewGuid();

            var result = new MutableBookmarksCollection(id, Enumerable.Empty<IFolderContent>());

            result.Emit(new MutableBookmarksCollectionCreated(id));

            return result;
        }

        public static MutableBookmarksCollection Rehydrate(Guid id, IEnumerable<IFolderContent> contents)
        {
            return new MutableBookmarksCollection(id, contents);
        }

        #endregion

        #region bookmark operations

        public void AddBookmark(Uri uri, string name, Guid destinationFolder, Position position)
        {
            var folder = FindFolder(destinationFolder);

            var bookmark = MutableBookmark.CreateNew(uri, name);

            Emit(folder.Add(bookmark, position));
        }

        public void DeleteBookmark(Guid bookmarkId)
        {
            var parentFolder = RootFolder.ParentOf(bookmarkId);

            if (!parentFolder.HasValue)
            {
                return;
            }

            Emit(parentFolder.Value.DeleteItem(bookmarkId));
        }

        public void MoveBookmark(Guid bookmarkId, Guid destinationFolderId, Position position)
        {
            var currentParent = RootFolder.ParentOf(bookmarkId);

            if (!currentParent.HasValue)
            {
                throw new InvalidOperationException($"Folder containing bookmark {bookmarkId} was not found");
            }

            var destination = FindFolder(destinationFolderId);

            Emit(currentParent.Value.MoveItem(bookmarkId, destination, position));
        }

        public void ReaddressBookmark(Guid bookmarkId, Uri uri)
        {
            var bookmark = FindBookmark(bookmarkId);

            Emit(bookmark.Readdress(uri));
        }

        public void RenameBookmark(Guid bookMarkId, string newName)
        {
            var bookmark = FindBookmark(bookMarkId);

            Emit(bookmark.Rename(newName));
        }

        #endregion

        #region folder

        public void AddFolder(Guid parentFolderId, string name, Position position)
        {
            var folder = FindFolder(parentFolderId);

            Emit(folder.Add(MutableFolder.CreateNew(name), position));
        }

        public void DeleteFolder(Guid folderId)
        {
            folderId.BetterNotBe(Guid.Empty, "Cannot delete root folder. Stop it.");

            var folder = RootFolder.ParentOf(folderId);

            if (!folder.HasValue)
            {
                return;
            }

            Emit(folder.Value.DeleteItem(folderId));
        }

        public void MoveFolder(Guid folderToMoveId, Guid destinationFolderId, Position position)
        {
            folderToMoveId.BetterNotBe(Guid.Empty, "Cannot move root folder. That's ridiculous.");

            var currentParent = RootFolder.ParentOf(folderToMoveId);

            if (!currentParent.HasValue)
            {
                throw new InvalidOperationException($"Folder containing folder {folderToMoveId} was not found");
            }

            var destination = FindFolder(destinationFolderId);

            Emit(currentParent.Value.MoveItem(folderToMoveId, destination, position));
        }

        public void RenameFolder(Guid folderId, string newName)
        {
            var folder = FindFolder(folderId);

            Emit(folder.Rename(newName));
        }

        #endregion

        #region private stuff

        private MutableFolder FindFolder(Guid folderId)
        {
            var result = RootFolder.Find<MutableFolder>(f => f.Id == folderId);

            if (!result.HasValue)
            {
                throw new InvalidOperationException($"Folder {folderId} was not found");
            }

            return result.Value;
        }

        private MutableBookmark FindBookmark(Guid bookmarkId)
        {
            var result = RootFolder.Find<MutableBookmark>(b => b.Id == bookmarkId);

            if (!result.HasValue)
            {
                throw new InvalidOperationException($"Bookmark {bookmarkId} was not found");
            }

            return result.Value;
        }

        #endregion
    }
}
