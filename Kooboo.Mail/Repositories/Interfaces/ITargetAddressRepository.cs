//Copyright (c) 2018 Yardi Technology Limited. Http://www.kooboo.com 
//All rights reserved.
using System;
using Kooboo.Extensions;
using Kooboo.IndexedDB;

namespace Kooboo.Mail.Repositories
{
    public interface ITargetAddressRepository : IRepository<TargetAddress>
    {
        TargetAddress Get(string email);
    }
}
