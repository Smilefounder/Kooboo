//Copyright (c) 2018 Yardi Technology Limited. Http://www.kooboo.com 
//All rights reserved.
using System;
using Kooboo.IndexedDB;
using Kooboo.Data;
using Kooboo.Mail.Repositories;

namespace Kooboo.Mail.Repositories.KoobooDb
{
    public  class MailDbImpl : MailDb
    {
        private object _lock = new object();
   
        public MailDbImpl(Guid UserId, Guid OrganizationId)
            : base(UserId, OrganizationId)
        {
            string folder = AppSettings.GetMailDbName(OrganizationId);
            string dbname = System.IO.Path.Combine(folder, UserId.ToString());
            Db = DB.GetDatabase(dbname);
        }

        public Database Db { get; set; }

        private FolderRepository _folders;
        public override IFolderRepository Folders
        {
            get
            {
                if (_folders == null)
                {
                    lock(_lock)
                    {
                        if (_folders == null)
                        {
                            _folders = new FolderRepository(this); 
                        }
                    }
                }
                return _folders; 
            }
        }

        private MessageRepository _messages;
        public override IMessageRepository Messages
        {
            get
            {
                if (_messages == null)
                {
                    lock(_lock)
                    {
                        if (_messages == null)
                        {
                            _messages = new MessageRepository(this); 
                        }
                    }
                } 
                return _messages;  
            }
        }

        private TargetAddressRepository _targetAddresses;
        public override ITargetAddressRepository TargetAddresses
        {
            get
            {
                if (_targetAddresses == null)
                {
                    lock(_lock)
                    {
                        if (_targetAddresses == null)
                        {
                            _targetAddresses = new TargetAddressRepository(this); 
                        }
                    }
                }
                return _targetAddresses;  
            }
        }

        private Sequence<string> _msgbody; 
        public Kooboo.IndexedDB.Sequence<string> MsgBody
        {
            get
            {
                if (_msgbody == null)
                {
                    lock(_lock)
                    {
                        _msgbody = this.Db.GetSequenceOld<string>("MessageSource"); 
                    }
                }
                return _msgbody;
            }
        }

        public override void Dispose()
        {
            this.Db.Close(); 
        }
    }
}
