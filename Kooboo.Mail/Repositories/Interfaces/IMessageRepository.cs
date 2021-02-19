//Copyright (c) 2018 Yardi Technology Limited. Http://www.kooboo.com 
//All rights reserved.
using Kooboo.IndexedDB;
using Kooboo.IndexedDB.Query;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Kooboo.Mail.Repositories
{
    public interface IMessageRepository : IRepository<Message>
    {
        void Add(Message Message, string MessageBody);

        void Update(Message Message, string MessageBody);

        bool AddOrUpdate(Message value);

        List<Message> ByFolder(string FolderName);

        List<Message> ByUidRange(string folderName, int minId, int maxId);

        Message GetBySeqNo(string FolderName, int LastMaxId, int MessageCount, int SeqNo);

        List<Message> GetBySeqNos(string FolderName, int LastMaxId, int MessageCount, int lower, int upper);

        int GetSeqNo(string FolderName, int LastMaxId, int MessagetCount, int MsgId);

        string GetContent(int id);

        void MarkAsRead(int MsgId, bool read = true);

        void UpdateRecent(int Id);

        // used with select.... select command should update the recent... 
        void UpdateRecentByMaxId(int maxMsgId);

        void Move(Message message, Folder toFolder);

        Models.MessageStat GetStat(string FolderName);

        Models.MessageStat GetStat(int FolderId, int AddressId = 0);

        string[] GetFlags(int MsgId);

        void AddFlags(int MsgId, string[] flags);

        void ReplaceFlags(int msgid, string[] flags);

        void RemoveFlags(int msgid, string[] flags);
    }
}
