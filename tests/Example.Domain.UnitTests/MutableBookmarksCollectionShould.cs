using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Xunit;

namespace Damascus.Example.Domain.UnitTests
{
    public class MutableBookmarkCollectionShould
    {
        private readonly Randomizer _randomizer = new Randomizer();
        private readonly Bogus.DataSets.Internet _randomizerNet = new Bogus.DataSets.Internet();
        private readonly Bogus.DataSets.Date _randomizerDate = new Bogus.DataSets.Date();

        private MutableBookmark _randomBookmark() => MutableBookmark.Rehydrate(
            Guid.NewGuid(),
            new Uri(_randomizerNet.Url()),
            _randomizer.String2(_randomizer.Int(1, 20)),
            _randomizerDate.RecentOffset(10),
            _randomizerDate.RecentOffset(1)
        );

        private MutableFolder _randomFolder(IEnumerable<IFolderContent> contents = null) =>
            MutableFolder.Rehydrate(
                Guid.NewGuid(),
                _randomizer.String2(_randomizer.Int(1, 10)),
                contents ?? Enumerable.Empty<IFolderContent>()
            );

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

            var bookmark = _randomBookmark();

            collection.AddBookmark(bookmark.Uri, bookmark.Name, Guid.Empty, new Position(1));

            var events = collection.FlushEvents().ToList();

            events.Single().Should().BeAssignableTo<BookmarkCreated>();

            collection.Contents.Cast<MutableBookmark>().Single().Name.Should().Be(bookmark.Name);
            collection.Contents.Cast<MutableBookmark>().Single().Uri.Should().Be(bookmark.Uri);
        }

        [Fact]
        public void ThrowWhenAddingBookmarkToNonexistentFolder()
        {
            var collection = MutableBookmarksCollection.CreateNew();

            var bookmark = _randomBookmark();

            Action action = () => collection.AddBookmark(bookmark.Uri, bookmark.Name, Guid.NewGuid(), new Position(1));

            action.Should().ThrowExactly<InvalidOperationException>().WithMessage("*not found");

            collection.Contents.Should().BeEmpty();
        }

        [Fact]
        public void EmitBookmarkDeletedWhenDeletingBookmark()
        {
            var bookmark = _randomBookmark();

            var collection = MutableBookmarksCollection.Rehydrate(
                Guid.NewGuid(),
                new[] { bookmark }
            );

            collection.DeleteBookmark(bookmark.Id);

            var events = collection.FlushEvents();

            events.Single().Should().BeAssignableTo<BookmarkDeleted>();

            collection.Contents.Should().BeEmpty();
        }

        [Fact]
        public void NotEmitBookmarkDeletedWhenDeletingNonexistentBookmark()
        {
            var bookmark = _randomBookmark();

            var collection = MutableBookmarksCollection.Rehydrate(
                Guid.NewGuid(),
                new[] { bookmark }
            );

            collection.DeleteBookmark(Guid.NewGuid());

            var events = collection.FlushEvents();

            events.Should().BeEmpty();

            collection.Contents.Cast<MutableBookmark>().Single().Should().BeEquivalentTo(bookmark);
        }

        [Fact]
        public void EmitBookmarkMovedWhenMovingBookmark()
        {
            var bookmark = _randomBookmark();
            var sourceFolder = _randomFolder(new[] { bookmark });
            var destinationFolder = _randomFolder();

            var collection = MutableBookmarksCollection.Rehydrate(
                Guid.NewGuid(),
                new[] { sourceFolder, destinationFolder }
            );

            collection.MoveBookmark(bookmark.Id, destinationFolder.Id, new Position(1));

            var events = collection.FlushEvents();

            events.Single().Should().BeAssignableTo<BookmarkMoved>();

            collection.Contents.Skip(1).Take(1).Cast<MutableFolder>().Single()
                .Contents.Cast<MutableBookmark>().Single()
                    .Should().BeEquivalentTo(bookmark);
        }

        [Fact]
        public void ThrowWhenMovingNonexistentBookmark()
        {
            var bookmark = _randomBookmark();
            var sourceFolder = _randomFolder(new[] { bookmark });
            var destinationFolder = _randomFolder();

            var collection = MutableBookmarksCollection.Rehydrate(
                Guid.NewGuid(),
                new[] { sourceFolder, destinationFolder }
            );

            Action action = () => collection.MoveBookmark(Guid.NewGuid(), destinationFolder.Id, new Position(1));

            action.Should().ThrowExactly<InvalidOperationException>().WithMessage("Folder*not found");

            collection.Contents.Take(1).Cast<MutableFolder>().Single()
                .Contents.Cast<MutableBookmark>().Single()
                    .Should().BeEquivalentTo(bookmark);

            collection.Contents.Skip(1).Take(1).Cast<MutableFolder>().Single()
                .Contents.Should().BeEmpty();
        }

        [Fact]
        public void ThrowWhenMovingBookmarkToNonexistentDestination()
        {
            var bookmark = _randomBookmark();
            var sourceFolder = _randomFolder(new[] { bookmark });
            var destinationFolder = _randomFolder();

            var collection = MutableBookmarksCollection.Rehydrate(
                Guid.NewGuid(),
                new[] { sourceFolder, destinationFolder }
            );

            Action action = () => collection.MoveBookmark(bookmark.Id, Guid.NewGuid(), new Position(1));

            action.Should().ThrowExactly<InvalidOperationException>().WithMessage("Folder*not found");

            collection.Contents.Take(1).Cast<MutableFolder>().Single()
                .Contents.Cast<MutableBookmark>().Single()
                    .Should().BeEquivalentTo(bookmark);

            collection.Contents.Skip(1).Take(1).Cast<MutableFolder>().Single()
                .Contents.Should().BeEmpty();
        }

        [Fact]
        public void EmitBookmarkReaddressedWhenReaddressingBookmark()
        {
            var bookmark = _randomBookmark();
            var newUri = new Uri("https://bing.com");

            var collection = MutableBookmarksCollection.Rehydrate(Guid.NewGuid(), new[] { bookmark });

            collection.ReaddressBookmark(bookmark.Id, newUri);

            var events = collection.FlushEvents();

            events.Single().Should().BeAssignableTo<BookmarkReaddressed>();

            collection.Contents.Cast<MutableBookmark>().Single().Uri.Should().BeEquivalentTo(newUri);
        }

        [Fact]
        public void ThrowWhenReaddressingNonexistentBookmark()
        {
            var bookmark = _randomBookmark();
            var collection = MutableBookmarksCollection.Rehydrate(Guid.NewGuid(), new[] { bookmark });
            var newUri = new Uri("https://bing.com");

            Action action = () => collection.ReaddressBookmark(Guid.NewGuid(), newUri);

            action.Should().ThrowExactly<InvalidOperationException>().WithMessage("*not found");

            collection.Contents.Cast<MutableBookmark>().Single().Should().BeEquivalentTo(bookmark);
        }

        [Fact]
        public void EmitBookmarkRenamedWhenRenamingBookmark()
        {
            var bookmark = _randomBookmark();
            var collection = MutableBookmarksCollection.Rehydrate(Guid.NewGuid(), new[] { bookmark });

            collection.RenameBookmark(bookmark.Id, "brand new");

            var events = collection.FlushEvents();

            events.Single().Should().BeAssignableTo<BookmarkRenamed>();

            collection.Contents.Cast<MutableBookmark>().Single().Name.Should().Be("brand new");
        }

        [Fact]
        public void ThrowWhenRenamingNonexistentBookmark()
        {
            var bookmark = _randomBookmark();
            var collection = MutableBookmarksCollection.Rehydrate(Guid.NewGuid(), new[] { bookmark });

            Action action = () => collection.RenameBookmark(Guid.NewGuid(), "brand new");

            action.Should().ThrowExactly<InvalidOperationException>().WithMessage("*not found");

            collection.Contents.Cast<MutableBookmark>().Single().Should().BeEquivalentTo(bookmark);
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

            collection.Contents.Cast<MutableFolder>().Single().Name.Should().Be("Stuff");
        }

        [Fact]
        public void ThrowWhenAddingFolderToNonexistentParent()
        {
            var collection = MutableBookmarksCollection.Rehydrate(Guid.NewGuid(), Enumerable.Empty<IFolderContent>());

            Action action = () => collection.AddFolder(Guid.NewGuid(), "Stuff", new Position(1));

            action.Should().ThrowExactly<InvalidOperationException>().WithMessage("*not found");

            collection.Contents.Should().BeEmpty();
        }

        [Fact]
        public void NotEmitWhenDeletingNonexistentFolder()
        {
            var folder = _randomFolder();
            var collection = MutableBookmarksCollection.Rehydrate(Guid.NewGuid(), new[] { folder });

            collection.DeleteFolder(Guid.NewGuid());

            var events = collection.FlushEvents();

            events.Should().BeEmpty();

            collection.Contents.Cast<MutableFolder>().Single().Should().BeEquivalentTo(folder);
        }

        [Fact]
        public void EmitSingleFolderDeletedWhenDeletingEmptyFolder()
        {
            var folder = _randomFolder();
            var collection = MutableBookmarksCollection.Rehydrate(Guid.NewGuid(), new[] { folder });

            collection.DeleteFolder(folder.Id);

            var events = collection.FlushEvents();

            events.Single().Should().BeAssignableTo<FolderDeleted>();

            collection.Contents.Should().BeEmpty();
        }

        [Fact]
        public void EmitDeletedEventsForEachItemDeletedWhenDeletingFolderWithStuff()
        {
            var emptyFolder = _randomFolder();
            var bookmark = _randomBookmark();
            var folder = _randomFolder(new IFolderContent[] { emptyFolder, bookmark });

            var collection = MutableBookmarksCollection.Rehydrate(Guid.NewGuid(), new[] { folder });

            collection.DeleteFolder(folder.Id);

            var events = collection.FlushEvents();

            events.Should().HaveCount(3);

            collection.Contents.Should().BeEmpty();
        }

        [Fact]
        public void ThrowWhenDeletingRootFolder()
        {
            var collection = MutableBookmarksCollection.Rehydrate(Guid.NewGuid(), Enumerable.Empty<IFolderContent>());

            Action action = () => collection.DeleteFolder(Guid.Empty);

            action.Should().ThrowExactly<InvalidOperationException>().WithMessage("Cannot delete root*");

            collection.Contents.Should().BeEmpty();
        }

        [Fact]
        public void EmitFolderMovedWhenMovingFolder()
        {
            var innerFolder = _randomFolder();
            var folder = _randomFolder(new[] { innerFolder });

            var collection = MutableBookmarksCollection.Rehydrate(Guid.NewGuid(), new[] { folder });

            collection.MoveFolder(innerFolder.Id, Guid.Empty, new Position(1));

            var events = collection.FlushEvents();

            events.Single().Should().BeAssignableTo<FolderMoved>();

            collection.Contents.Should().HaveCount(2);

            collection.Contents.Take(1).Cast<MutableFolder>().Single().Contents
                .Should().BeEmpty();

            collection.Contents.Skip(1).Take(1).Cast<MutableFolder>().Single().Contents
                .Should().BeEmpty();
        }

        [Fact]
        public void ThrowWhenMovingRootFolder()
        {
            var collection = MutableBookmarksCollection.Rehydrate(Guid.NewGuid(), Enumerable.Empty<IFolderContent>());

            Action action = () => collection.MoveFolder(Guid.Empty, Guid.NewGuid(), new Position(1));

            action.Should().ThrowExactly<InvalidOperationException>().WithMessage("Cannot move root*");

            collection.Contents.Should().BeEmpty();
        }

        [Fact]
        public void ThrowWhenMovingNonexistentFolder()
        {
            var innerFolder = _randomFolder();
            var folder = _randomFolder(new[] { innerFolder });

            var collection = MutableBookmarksCollection.Rehydrate(Guid.NewGuid(), new[] { folder });

            Action action = () => collection.MoveFolder(Guid.NewGuid(), innerFolder.Id, new Position(1));

            action.Should().ThrowExactly<InvalidOperationException>().WithMessage("*not found");

            collection.Contents.Cast<MutableFolder>().Single().Should().BeEquivalentTo(folder);
        }

        [Fact]
        public void ThrowWhenMovingFolderToNonexistentDestination()
        {
            var innerFolder = _randomFolder();
            var folder = _randomFolder(new[] { innerFolder });

            var collection = MutableBookmarksCollection.Rehydrate(Guid.NewGuid(), new[] { folder });

            Action action = () => collection.MoveFolder(innerFolder.Id, Guid.NewGuid(), new Position(1));

            action.Should().ThrowExactly<InvalidOperationException>().WithMessage("*not found");

            collection.Contents.Single().Should().BeEquivalentTo(folder);
        }

        [Fact]
        public void EmitFolderRenamedWhenRenamingFolder()
        {
            var folder = _randomFolder();
            var collection = MutableBookmarksCollection.Rehydrate(Guid.NewGuid(), new[] { folder });

            collection.RenameFolder(folder.Id, "Stuff and things");

            var events = collection.FlushEvents();

            events.Single().Should().BeAssignableTo<FolderRenamed>();

            collection.Contents.Cast<MutableFolder>().Single().Name.Should().Be("Stuff and things");
        }

        [Fact]
        public void ThrowWhenRenamingNonexistentFolder()
        {
            var folder = _randomFolder();

            var collection = MutableBookmarksCollection.Rehydrate(Guid.NewGuid(), new[] { folder });

            Action action = () => collection.RenameFolder(Guid.NewGuid(), "Stuff and things");

            action.Should().ThrowExactly<InvalidOperationException>().WithMessage("*not found");

            collection.Contents.Single().Should().BeEquivalentTo(folder);
        }

        #endregion
    }
}
