//Copyright (c) 2018 Yardi Technology Limited. Http://www.kooboo.com 
//All rights reserved.
using System;
using System.Collections.Generic;
using Kooboo.IndexedDB; 
using System.Linq;

namespace Kooboo.Mail.Repositories
{
    public interface IEmailAddressRepository : IRepository<EmailAddress>
    {
        EmailAddress Get(string email);

        List<EmailAddress> ByUser(Guid userId);

        List<string> GetMembers(int addressId);

        void AddMember(int addressId, string memberAddress);

        void DeleteMember(int addressId, string memberAddress);

        EmailAddress Find(string emailaddress);
    }
}