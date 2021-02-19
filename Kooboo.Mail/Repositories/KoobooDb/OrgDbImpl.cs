//Copyright (c) 2018 Yardi Technology Limited. Http://www.kooboo.com 
//All rights reserved.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Kooboo.Data;
using Kooboo.IndexedDB;
using Kooboo.Mail.Repositories;

namespace Kooboo.Mail.Repositories.KoobooDb
{
    public class OrgDbImpl : OrgDb
    {
        public OrgDbImpl(Guid organizationId)
            : base(organizationId)
        {
            var dbName = Kooboo.Data.AppSettings.GetMailDbName(OrganizationId);
            Db = DB.GetDatabase(dbName);
            EmailAddress = new EmailAddressRepository(this);
        }

        public Database Db { get; set; }  
 
        public override IEmailAddressRepository EmailAddress
        {
            get; 
        }

        public override void Dispose()
        {
            this.Db.Close();
        }
    }
}
