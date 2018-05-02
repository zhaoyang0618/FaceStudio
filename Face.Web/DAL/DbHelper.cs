using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace Face.Web.DAL
{
    public class DbHelper<T>
    {
        /// <summary>
        /// 更新数据库数据的时候只更新部分列
        /// </summary>
        /// <param name="t"></param>
        /// <param name="cols"></param>
        public static void DbColumnsUpdate(DbContext db, T t, String[] cols)
        {
            System.Diagnostics.Debug.Assert(null != db);
            System.Diagnostics.Debug.Assert(null != t);
            System.Diagnostics.Debug.Assert(null != cols && cols.Length > 0);

            var stateEntry = ((IObjectContextAdapter)db).ObjectContext.ObjectStateManager.GetObjectStateEntry(t);
            foreach(var v in cols)
            {
                stateEntry.SetModifiedProperty(v);
            }
        }
    }
}