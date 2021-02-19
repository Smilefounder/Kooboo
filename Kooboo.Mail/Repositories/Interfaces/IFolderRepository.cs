//Copyright (c) 2018 Yardi Technology Limited. Http://www.kooboo.com 
//All rights reserved.
using System;
using System.Linq;
using System.Collections.Generic;
using Kooboo.IndexedDB;

namespace Kooboo.Mail.Repositories
{
    public  interface IFolderRepository : IRepository<Folder>
    {
        Folder Get(string name);

        void Rename(Folder folder, string newName, bool Recursive = true);

        void Delete(Folder folder);

        void Add(string name);

        void Subscribe(Folder folder);

        void Unsubscribe(Folder folder);

        List<Folder> AllFolders();
    }
}
