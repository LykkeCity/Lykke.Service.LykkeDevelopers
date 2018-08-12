using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.LykkeDevelopers.Core
{
    public interface IEntity
    {
        string RowKey { get; set; }

        string ETag { get; set; }
    }
}
