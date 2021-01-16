using System;
using Damascus.Example.Contracts;
using Damascus.Example.Domain;

namespace Damascus.Example.Api
{
    public static class ContractsExtensions
    {
        public static (Guid, Position) ToDomain(this FolderLocation position)
        {
            return (position.FolderId, new Position(position.NonZeroIndex));
        }
    }
}
