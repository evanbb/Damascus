using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Damascus.Example.Domain.UnitTests
{
    public class MutableBookmarkCollectionShould
    {
        #region ctor

        [Fact]
        public void EmitCreatedWhenCreatingNew()
        {
            var collection = MutableBookmarksCollection.CreateNew();

            var events = collection.FlushEvents().ToList();

            events.Single().Should().BeAssignableTo<MutableBookmarksCollectionCreated>();
        }

        [Fact]
        public void NotEmitCreatedWhenRehydrating()
        {
            var collection = MutableBookmarksCollection.Rehydrate(Guid.NewGuid(), Enumerable.Empty<IFolderContent>());

            var events = collection.FlushEvents().ToList();

            events.Should().BeEmpty();
        }

        #endregion

        #region bookmarks

        [Fact]
        public void EmitBookmarkAddedWhenAddingBookmark()
        {
            var collection = MutableBookmarksCollection.Rehydrate(Guid.NewGuid(), Enumerable.Empty<IFolderContent>());

            collection.AddBookmark(new Uri("https://google.com"), "Google", Guid.Empty, new Position(1));

            var events = collection.FlushEvents().ToList();

            events.Single().Should().BeAssignableTo<BookmarkCreated>();
        }

        [Fact]
        public void ThrowWhenAddingBookmarkToNonexistentFolder()
        {
            var collection = MutableBookmarksCollection.CreateNew();

            Action action = () => collection.AddBookmark(new Uri("https://google.com"), "Google", Guid.NewGuid(), new Position(1));

            action.Should().ThrowExactly<InvalidOperationException>().WithMessage("*not found");
        }

        [Fact]
        public void EmitBookmarkDeletedWhenDeletingBookmark()
        {
            var bookmarkId = Guid.NewGuid();

            var collection = MutableBookmarksCollection.Rehydrate(
                Guid.NewGuid(),
                new[]
                {
                    MutableBookmark.Rehydrate(bookmarkId, new Uri("https://google.com"), "Google", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow)
                });

            collection.DeleteBookmark(bookmarkId);

            var events = collection.FlushEvents();

            events.Single().Should().BeAssignableTo<BookmarkDeleted>();
        }

        [Fact]
        public void NotEmitBookmarkDeletedWhenDeletingNonexistentBookmark()
        {
            var bookmarkId = Guid.NewGuid();

            var collection = MutableBookmarksCollection.Rehydrate(
                Guid.NewGuid(),
                new[]
                {
                    MutableBookmark.Rehydrate(bookmarkId, new Uri("https://google.com"), "Google", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow)
                });

            collection.DeleteBookmark(Guid.NewGuid());

            var events = collection.FlushEvents();

            events.Should().BeEmpty();
        }

        [Fact]
        public void EmitBookmarkMovedWhenMovingBookmark()
        {
            var destinationFolderId = Guid.NewGuid();
            var bookmarkId = Guid.NewGuid();

            var collection = MutableBookmarksCollection.Rehydrate(
                Guid.NewGuid(),
                new[]
                {
                    MutableFolder.Rehydrate(Guid.NewGuid(), "Source", new []{
                        MutableBookmark.Rehydrate(bookmarkId, new Uri("https://google.com"), "Google", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow)
                    }),
                    MutableFolder.Rehydrate(destinationFolderId, "Destination", Enumerable.Empty<IFolderContent>())
                });

            collection.MoveBookmark(bookmarkId, destinationFolderId, new Position(1));

            var events = collection.FlushEvents();

            events.Single().Should().BeAssignableTo<BookmarkMoved>();
        }

        [Fact]
        public void NotEmitWhenMovingBookmarkToSameDestination()
        {
            var destinationFolderId = Guid.NewGuid();
            var bookmarkId = Guid.NewGuid();

            var collection = MutableBookmarksCollection.Rehydrate(
                Guid.NewGuid(),
                new[]
                {
                    MutableFolder.Rehydrate(Guid.NewGuid(), "Source", Enumerable.Empty<IFolderContent>()),
                    MutableFolder.Rehydrate(destinationFolderId, "Destination", new []{
                        MutableBookmark.Rehydrate(bookmarkId, new Uri("https://google.com"), "Google", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow)
                    })
                });

            collection.MoveBookmark(bookmarkId, destinationFolderId, new Position(1));

            var events = collection.FlushEvents();

            events.Should().BeEmpty();
        }

        [Fact]
        public void ThrowWhenMovingNonexistentBookmark()
        {
            var destinationFolderId = Guid.NewGuid();
            var bookmarkId = Guid.NewGuid();

            var collection = MutableBookmarksCollection.Rehydrate(
                Guid.NewGuid(),
                new[]
                {
                    MutableFolder.Rehydrate(Guid.NewGuid(), "Source", new []{
                        MutableBookmark.Rehydrate(bookmarkId, new Uri("https://google.com"), "Google", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow)
                    }),
                    MutableFolder.Rehydrate(destinationFolderId, "Destination", Enumerable.Empty<IFolderContent>())
                });

            Action action = () => collection.MoveBookmark(Guid.NewGuid(), destinationFolderId, new Position(1));

            action.Should().ThrowExactly<InvalidOperationException>().WithMessage("Folder*not found");
        }

        [Fact]
        public void ThrowWhenMovingBookmarkToNonexistentDestination()
        {
            var destinationFolderId = Guid.NewGuid();
            var bookmarkId = Guid.NewGuid();

            var collection = MutableBookmarksCollection.Rehydrate(
                Guid.NewGuid(),
                new[]
                {
                    MutableFolder.Rehydrate(Guid.NewGuid(), "Source", new []{
                        MutableBookmark.Rehydrate(bookmarkId, new Uri("https://google.com"), "Google", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow)
                    }),
                    MutableFolder.Rehydrate(destinationFolderId, "Destination", Enumerable.Empty<IFolderContent>())
                });

            Action action = () => collection.MoveBookmark(bookmarkId, Guid.NewGuid(), new Position(1));

            action.Should().ThrowExactly<InvalidOperationException>().WithMessage("Folder*not found");
        }

        [Fact]
        public void EmitBookmarkReaddressedWhenReaddressingBookmark()
        {
            var bookmarkId = Guid.NewGuid();
            var collection = MutableBookmarksCollection.Rehydrate(Guid.NewGuid(), new[] {
                MutableBookmark.Rehydrate(bookmarkId, new Uri("https://google.com"), "Google", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow)
            });

            collection.ReaddressBookmark(bookmarkId, new Uri("https://bing.com"));

            var events = collection.FlushEvents();

            events.Single().Should().BeAssignableTo<BookmarkReaddressed>();
        }

        [Fact]
        public void ThrowWhenReaddressingNonexistentBookmark()
        {
            var collection = MutableBookmarksCollection.Rehydrate(Guid.NewGuid(), new[] {
                MutableBookmark.Rehydrate(Guid.NewGuid(), new Uri("https://google.com"), "Google", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow)
            });

            Action action = () => collection.ReaddressBookmark(Guid.NewGuid(), new Uri("https://bing.com"));

            action.Should().ThrowExactly<InvalidOperationException>().WithMessage("*not found");
        }

        [Fact]
        public void EmitBookmarkRenamedWhenRenamingBookmark()
        {
            var bookmarkId = Guid.NewGuid();
            var collection = MutableBookmarksCollection.Rehydrate(Guid.NewGuid(), new[] {
                MutableBookmark.Rehydrate(bookmarkId, new Uri("https://google.com"), "Google", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow)
            });

            collection.RenameBookmark(bookmarkId, "googz");

            var events = collection.FlushEvents();

            events.Single().Should().BeAssignableTo<BookmarkRenamed>();
        }

        [Fact]
        public void ThrowWhenRenamingNonexistentBookmark()
        {
            var collection = MutableBookmarksCollection.Rehydrate(Guid.NewGuid(), new[] {
                MutableBookmark.Rehydrate(Guid.NewGuid(), new Uri("https://google.com"), "Google", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow)
            });

            Action action = () => collection.RenameBookmark(Guid.NewGuid(), "googz");

            action.Should().ThrowExactly<InvalidOperationException>().WithMessage("*not found");
        }

        #endregion

        #region folders

        [Fact]
        public void EmitFolderAddedWhenAddingFolder()
        {
            var collection = MutableBookmarksCollection.Rehydrate(Guid.NewGuid(), Enumerable.Empty<IFolderContent>());

            collection.AddFolder(Guid.Empty, "Stuff", new Position(1));

            var events = collection.FlushEvents();

            events.Single().Should().BeAssignableTo<FolderCreated>();
        }

        [Fact]
        public void ThrowWhenAddingFolderToNonexistentParent()
        {
            var collection = MutableBookmarksCollection.Rehydrate(Guid.NewGuid(), Enumerable.Empty<IFolderContent>());

            Action action = () => collection.AddFolder(Guid.NewGuid(), "Stuff", new Position(1));

            action.Should().ThrowExactly<InvalidOperationException>().WithMessage("*not found");
        }

        [Fact]
        public void NotEmitWhenDeletingNonexistentFolder()
        {
            var collection = MutableBookmarksCollection.Rehydrate(Guid.NewGuid(), new[] {
                MutableFolder.Rehydrate(Guid.NewGuid(), "Stuff", Enumerable.Empty<IFolderContent>())
            });

            collection.DeleteFolder(Guid.NewGuid());

            var events = collection.FlushEvents();

            events.Should().BeEmpty();
        }

        [Fact]
        public void EmitSingleFolderDeletedWhenDeletingEmptyFolder()
        {
            var folderId = Guid.NewGuid();
            var collection = MutableBookmarksCollection.Rehydrate(Guid.NewGuid(), new[] {
                MutableFolder.Rehydrate(folderId, "Stuff", Enumerable.Empty<IFolderContent>())
            });

            collection.DeleteFolder(folderId);

            var events = collection.FlushEvents();

            events.Single().Should().BeAssignableTo<FolderDeleted>();
        }

        [Fact]
        public void EmitDeletedEventsForEachItemDeletedWhenDeletingFolderWithStuff()
        {
            var folderId = Guid.NewGuid();
            var collection = MutableBookmarksCollection.Rehydrate(Guid.NewGuid(), new[] {
                MutableFolder.Rehydrate(folderId, "Stuff", new IFolderContent[]{
                    MutableFolder.Rehydrate(Guid.NewGuid(), "insider info", Enumerable.Empty<IFolderContent>()),
                    MutableBookmark.Rehydrate(Guid.NewGuid(), new Uri("https://google.com"), "google", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow)
                })
            });

            collection.DeleteFolder(folderId);

            var events = collection.FlushEvents();

            events.Should().HaveCount(3);
        }

        [Fact]
        public void ThrowWhenDeletingRootFolder()
        {
            var collection = MutableBookmarksCollection.Rehydrate(Guid.NewGuid(), Enumerable.Empty<IFolderContent>());

            Action action = () => collection.DeleteFolder(Guid.Empty);

            action.Should().ThrowExactly<InvalidOperationException>().WithMessage("Cannot delete root*");
        }

        [Fact]
        public void EmitFolderMovedWhenMovingFolder()
        {
            var folderId = Guid.NewGuid();

            var collection = MutableBookmarksCollection.Rehydrate(Guid.NewGuid(), new[] {
                MutableFolder.Rehydrate(Guid.NewGuid(), "Stuff", new [] {
                    MutableFolder.Rehydrate(folderId, "inside info", Enumerable.Empty<IFolderContent>())
                })
            });

            collection.MoveFolder(folderId, Guid.Empty, new Position(1));

            var events = collection.FlushEvents();

            events.Single().Should().BeAssignableTo<FolderMoved>();
        }

        [Fact]
        public void NotEmitWhenMovingFolderToSameDestination()
        {
            var folderId = Guid.NewGuid();
            var destinationId = Guid.NewGuid();

            var collection = MutableBookmarksCollection.Rehydrate(Guid.NewGuid(), new[] {
                MutableFolder.Rehydrate(destinationId, "Stuff", new [] {
                    MutableFolder.Rehydrate(folderId, "inside info", Enumerable.Empty<IFolderContent>())
                })
            });

            collection.MoveFolder(folderId, destinationId, new Position(1));

            var events = collection.FlushEvents();

            events.Should().BeEmpty();
        }

        [Fact]
        public void ThrowWhenMovingRootFolder()
        {
            var collection = MutableBookmarksCollection.Rehydrate(Guid.NewGuid(), Enumerable.Empty<IFolderContent>());

            Action action = () => collection.MoveFolder(Guid.Empty, Guid.NewGuid(), new Position(1));

            action.Should().ThrowExactly<InvalidOperationException>().WithMessage("Cannot move root*");
        }

        [Fact]
        public void ThrowWhenMovingNonexistentFolder()
        {
            var destinationId = Guid.NewGuid();

            var collection = MutableBookmarksCollection.Rehydrate(Guid.NewGuid(), new[] {
                MutableFolder.Rehydrate(destinationId, "Destination", Enumerable.Empty<IFolderContent>())
            });

            Action action = () => collection.MoveFolder(Guid.NewGuid(), destinationId, new Position(1));

            action.Should().ThrowExactly<InvalidOperationException>().WithMessage("*not found");
        }

        [Fact]
        public void ThrowWhenMovingFolderToNonexistentDestination()
        {
            var sourceId = Guid.NewGuid();

            var collection = MutableBookmarksCollection.Rehydrate(Guid.NewGuid(), new[] {
                MutableFolder.Rehydrate(sourceId, "Source", Enumerable.Empty<IFolderContent>())
            });

            Action action = () => collection.MoveFolder(sourceId, Guid.NewGuid(), new Position(1));

            action.Should().ThrowExactly<InvalidOperationException>().WithMessage("*not found");
        }

        [Fact]
        public void EmitFolderRenamedWhenRenamingFolder()
        {
            var folderId = Guid.NewGuid();
            var collection = MutableBookmarksCollection.Rehydrate(Guid.NewGuid(), new[] {
                MutableFolder.Rehydrate(folderId, "Stuff", Enumerable.Empty<IFolderContent>())
            });

            collection.RenameFolder(folderId, "Stuff and things");

            var events = collection.FlushEvents();

            events.Single().Should().BeAssignableTo<FolderRenamed>();
        }

        [Fact]
        public void ThrowWhenRenamingNonexistentFolder()
        {
            var collection = MutableBookmarksCollection.Rehydrate(Guid.NewGuid(), Enumerable.Empty<IFolderContent>());

            Action action = () => collection.RenameFolder(Guid.NewGuid(), "Stuff and things");

            action.Should().ThrowExactly<InvalidOperationException>().WithMessage("*not found");
        }

        #endregion
    }
}
