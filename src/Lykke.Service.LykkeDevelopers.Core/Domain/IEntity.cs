﻿namespace Lykke.Service.LykkeDevelopers.Core.Domain
{
    public interface IEntity
    {
        string RowKey { get; set; }

        string ETag { get; set; }
    }
}
