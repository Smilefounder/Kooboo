//Copyright (c) 2018 Yardi Technology Limited. Http://www.kooboo.com 
//All rights reserved.
using Kooboo.IndexedDB.Btree;
using Kooboo.IndexedDB.Indexs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.IndexedDB.Dynamic
{  
    public interface ITableIndex:IIndex
    {  
     

        bool IsIncremental { get; set; }

        long Seed { get; set; }

        bool IsUnique { get; set; }

        long Increment { get; set; }

        long NextIncrement();  

      

        bool IsPrimaryKey { get; set; }

        bool IsSystem { get; set; }

        bool Add(object key, Int64 blockPosition);
         
        void Update(object  oldKey,   Int64 oldBlockPosition, Int64 newBlockPosition);

        void Update(object oldKey, object newkey, long oldBlockPosition, long newBlockPosition); 
       
        long Get(object key);

        List<long> List(object key); 

        bool Del(object  key, Int64 blockPosition);

        List<Int64> Del(object key);  
    }
     
}
