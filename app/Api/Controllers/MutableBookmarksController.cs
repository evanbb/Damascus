using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Damascus.Example.Contracts;
using Damascus.Example.Infrastructure;
using Damascus.Example.Domain;
using System.Linq;
using Damascus.Core;

namespace Damascus.Example.Api
{
    [ApiController]
    // hide the identity and other info from the url - get it from a signed token or similar
    [Route("[controller]/my")]
    public class MutableBookmarksController : ControllerBase
    {
        // we would normally use the real identity of the user, but meh for now
        private static Guid FAKE_USER_IDENTITY = Guid.NewGuid();

        private readonly IMutableBookmarksCommandRepo _commandRepo;
        private readonly IBookmarksQueryRepo _queryRepo;

        public MutableBookmarksController(
            IMutableBookmarksCommandRepo commandRepo,
            IBookmarksQueryRepo queryRepo
        )
        {
            _commandRepo = commandRepo;
            _queryRepo = queryRepo;
        }

        // hide the identity and other info from the url - get it from a signed token or similar
        [HttpGet]
        public async Task<IActionResult> GetCollection()
        {
            var collection = await _queryRepo.FindAsync(FAKE_USER_IDENTITY);

            if (!collection.HasValue)
            {
                /*
                 * lazily create a bookmark collection for the current user
                 * you probably don't want to do this in most actual apps, but maybe :D
                 * normally, you would just `return NotFound();` as illustrated in other routes below
                 */

                var newCollection = MutableBookmarksCollection
                    .Rehydrate(FAKE_USER_IDENTITY, Enumerable.Empty<IFolderContent>());

                await _commandRepo.CommitAsync(newCollection);

                return Ok(newCollection.ToContract());
            }

            return Ok(collection.Value);
        }

        #region bookmark operations

        [HttpPost]
        [Route("bookmarks")]
        public async Task<IActionResult> AddBookmark([FromBody] AddBookmark bookmark)
        {
            var collection = await _commandRepo.FindAsync(FAKE_USER_IDENTITY);

            if (!collection.HasValue)
            {
                return NotFound();
            }

            var (folderId, position) = bookmark.Location.ToDomain();

            var newBookmark = MutableBookmark.CreateNew(new Uri(bookmark.Url), bookmark.Label);

            collection.Value.AddBookmark(newBookmark, folderId, position);

            await _commandRepo.CommitAsync(collection);

            return CreatedAtAction(nameof(GetBookmark), new { bookmarkId = newBookmark.Id }, newBookmark.ToContract());
        }

        [HttpGet]
        [Route("bookmarks/{bookmarkId:Guid}")]
        public async Task<IActionResult> GetBookmark(Guid bookmarkId)
        {
            var collection = await _queryRepo.FindAsync(FAKE_USER_IDENTITY);

            if (!collection.HasValue)
            {
                return NotFound();
            }

            var bookmark = collection.Value.Items
                .Where(x => x is Bookmark)
                .Cast<Bookmark>()
                .SingleOrDefault(i => i.Id == bookmarkId);

            if (bookmark.IsNull())
            {
                return NotFound();
            }

            return Ok(bookmark);
        }

        [HttpDelete]
        [Route("bookmarks/{bookmarkId:Guid}")]
        public async Task<IActionResult> DeleteBookmark(Guid bookmarkId)
        {
            var collection = await _commandRepo.FindAsync(FAKE_USER_IDENTITY);

            if (!collection.HasValue)
            {
                return NotFound();
            }

            collection.Value.DeleteBookmark(bookmarkId);

            await _commandRepo.CommitAsync(collection);

            return Ok();
        }

        [HttpPost]
        [Route("bookmarks/{bookmarkId:Guid}/name")]
        public async Task<IActionResult> RenameBookmark(Guid bookmarkId, [FromBody] string name)
        {
            var collection = await _commandRepo.FindAsync(FAKE_USER_IDENTITY);

            if (!collection.HasValue)
            {
                return NotFound();
            }

            collection.Value.RenameBookmark(bookmarkId, name);

            await _commandRepo.CommitAsync(collection);

            return Ok();
        }

        [HttpPost]
        [Route("bookmarks/{bookmarkId:Guid}/url")]
        public async Task<IActionResult> ReaddressBookmark(Guid bookmarkId, [FromBody] string url)
        {
            var collection = await _commandRepo.FindAsync(FAKE_USER_IDENTITY);

            if (!collection.HasValue)
            {
                return NotFound();
            }

            collection.Value.ReaddressBookmark(bookmarkId, new Uri(url));

            await _commandRepo.CommitAsync(collection);

            return Ok();
        }

        [HttpPost]
        [Route("bookmarks/{bookmarkId:Guid}/location")]
        public async Task<IActionResult> MoveBookmark(Guid bookmarkId, [FromBody] FolderLocation newPosition)
        {
            var collection = await _commandRepo.FindAsync(FAKE_USER_IDENTITY);

            if (!collection.HasValue)
            {
                return NotFound();
            }

            var (folderId, position) = newPosition.ToDomain();

            collection.Value.MoveBookmark(bookmarkId, folderId, position);

            await _commandRepo.CommitAsync(collection);

            return Ok();
        }

        #endregion

        #region folder operations

        [HttpPost]
        [Route("folders")]
        public async Task<IActionResult> AddFolder([FromBody] AddFolder folder)
        {
            var collection = await _commandRepo.FindAsync(FAKE_USER_IDENTITY);

            if (!collection.HasValue)
            {
                return NotFound();
            }

            var (folderId, postion) = folder.Location.ToDomain();
            var newFolder = MutableFolder.CreateNew(folder.Label);

            collection.Value.AddFolder(folderId, newFolder, postion);

            await _commandRepo.CommitAsync(collection);

            return CreatedAtAction(nameof(GetFolder), new { folderId = newFolder.Id }, newFolder.ToContract());
        }

        [HttpGet]
        [Route("folders/{folderId:Guid}")]
        public async Task<IActionResult> GetFolder(Guid folderId)
        {
            var collection = await _queryRepo.FindAsync(FAKE_USER_IDENTITY);

            if (!collection.HasValue)
            {
                return NotFound();
            }

            var folder = collection.Value.Items
                .Where(i => i is Folder)
                .Cast<Folder>()
                .SingleOrDefault(f => f.Id == folderId);

            if (folder.IsNull())
            {
                return NotFound();
            }

            return Ok(folder);
        }

        [HttpPost]
        [Route("folders/{folderId:Guid}/location")]
        public async Task<IActionResult> MoveFolder(Guid folderId, [FromBody] AddFolder folder)
        {
            var collection = await _commandRepo.FindAsync(FAKE_USER_IDENTITY);

            if (!collection.HasValue)
            {
                return NotFound();
            }

            var (destinationFolderId, postion) = folder.Location.ToDomain();

            collection.Value.MoveFolder(folderId, destinationFolderId, postion);

            await _commandRepo.CommitAsync(collection);

            return Ok();
        }

        [HttpPost]
        [Route("folders/{folderId:Guid}/name")]
        public async Task<IActionResult> RenameFolder(Guid folderId, [FromBody] string label)
        {
            var collection = await _commandRepo.FindAsync(FAKE_USER_IDENTITY);

            if (!collection.HasValue)
            {
                return NotFound();
            }

            collection.Value.RenameFolder(folderId, label);

            await _commandRepo.CommitAsync(collection);

            return Ok();
        }

        [HttpDelete]
        [Route("folders/{folderId:Guid}")]
        public async Task<IActionResult> DeleteFolder(Guid folderId)
        {
            var collection = await _commandRepo.FindAsync(FAKE_USER_IDENTITY);

            if (!collection.HasValue)
            {
                return NotFound();
            }

            collection.Value.DeleteFolder(folderId);

            await _commandRepo.CommitAsync(collection);

            return Ok();
        }

        #endregion
    }
}
