//Copyright (c) 2018 Yardi Technology Limited. Http://www.kooboo.com 
//All rights reserved.
using Kooboo.IndexedDB;
using Kooboo.IndexedDB.Query;
using System;
using System.Linq;
using System.Collections.Generic;
using Kooboo.Mail.Repositories;

namespace Kooboo.Mail.Repositories
{
    public interface IMessageRepository : IRepository<Message>
    {
        void Add(Message Message, string MessageBody);

        void Update(Message Message, string MessageBody);

        List<Message> ByFolder(int folderId, int addressId, bool? deleted = null, int? pageSize = null);

        List<Message> ByUidRange(int folderId, int addressId, int minId, int maxId, int? pageSize = null);

        List<Message> ByCreationTimeRange(string folderName, long minTick, long maxTick);

        Message GetBySeqNo(string FolderName, int LastMaxId, int MessageCount, int SeqNo);

        List<Message> GetBySeqNos(string FolderName, int LastMaxId, int MessageCount, int lower, int upper);

        int GetSeqNo(string FolderName, int LastMaxId, int MessagetCount, int MsgId);

        string GetContent(int id);

        Message GetStatus(int id);

        void MarkAsRead(int MsgId, bool read = true);

        void UpdateRecent(int Id);

        // used with select.... select command should update the recent... 
        void UpdateRecentByMaxId(int maxMsgId);

        void Move(Message message, Folder toFolder);

        Models.MessageStat GetStat(string FolderName);

        Models.MessageStat GetStat(int FolderId, int AddressId = 0);

        void AddFlags(int MsgId, string[] flags);

        void ReplaceFlags(int msgid, string[] flags);

        void RemoveFlags(int msgid, string[] flags);
    }
}

namespace Kooboo.Mail
{
    public static class IMessageRepositoryExtensions
    {
        public static List<Message> ByFolder(this IMessageRepository repo, string folderName, bool? deleted = null, int? pageSize = null)
        {
            var model = Kooboo.Mail.Utility.FolderUtility.ParseFolder(folderName);
            return repo.ByFolder(model.FolderId, model.AddressId, deleted, pageSize);
        }

        public static List<Message> ByUidRange(this IMessageRepository repo, string folderName, int minId, int maxId, int? pageSize = null)
        {
            var model = Kooboo.Mail.Utility.FolderUtility.ParseFolder(folderName);
            return repo.ByUidRange(model.FolderId, model.AddressId, minId, maxId, pageSize);
        }

        public static string[] GetFlags(this IMessageRepository repo, int MsgId)
        {
            var msg = repo.GetStatus(MsgId);

            var flags = new List<string>();
            if (msg.Recent)
            {
                flags.Add("Recent");
            }
            if (msg.Read)
            {
                flags.Add("Seen");
            }
            if (msg.Answered)
            {
                flags.Add("Answered");
            }
            if (msg.Flagged)
            {
                flags.Add("Flagged");
            }
            if (msg.Deleted)
            {
                flags.Add("Deleted");
            }
            return flags.ToArray();
        }
    }
}
