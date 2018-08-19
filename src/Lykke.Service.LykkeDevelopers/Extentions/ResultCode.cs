using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.LykkeDevelopers.Extentions
{
    public enum ResultCode
    {
        Ok = 0,
        EmailAlreadyExists = 1,
        TelegramAlreadyExists = 2,
        GitAlreadyExists = 3,
        TeamAlreadyExists = 4,
        HasDuplicated = 5,
        InvalidInput = 7,
        InvalidRequest = 8
    }
}
