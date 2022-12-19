
using CakesByRAndRCore.Data;
using CakesByRAndRCore.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Reflection.PortableExecutable;

namespace CakesByRAndRCore.Repositories
{
    public class UnitOfWork : IDisposable
    {

        private CakesByRAndRCoreContext myContext;

        public UnitOfWork(CakesByRAndRCoreContext context)
        {
            myContext = context;
        }


        public List<CakeCategory> GetCakeCategories(string sqlSyntex, SqlParameter[] param = null)
        {
            //if (param == null)
                return myContext.CakeCategories.FromSqlRaw(sqlSyntex).ToList();
            //else
            //    return context.CakeCategories.FromSqlRaw<T>(sqlSyntex, param).ToList();


            //using (var context = new CakesByRAndRCoreContext())
            //{
            //List<CakeCategory> cakeCategories = context.CakeCategories.FromSqlRaw(sqlSyntex).ToList();
            //return cakeCategories.ToList();
            //}

        }

        public List<T> GetRecordSet<T>(string sqlSyntex, SqlParameter[] param = null)
        {

            using (var command = myContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sqlSyntex;
                command.CommandType = CommandType.Text;

                if (param != null)
                {
                    command.Parameters.Add(param);
                }
                myContext.Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {

                    return DataReaderMapToList<T>(result);

                }
            }


            //myContext.Database.GetDbConnection().CreateCommand())


            //return myContext.TaskItems.FromSqlRaw(sqlSyntex).ToList();
            //else
            //    return context.CakeCategories.FromSqlRaw<T>(sqlSyntex, param).ToList();


            //using (var context = new CakesByRAndRCoreContext())
            //{
            //List<CakeCategory> cakeCategories = context.CakeCategories.FromSqlRaw(sqlSyntex).ToList();
            //return cakeCategories.ToList();
            //}

        }

        private List<T> DataReaderMapToList<T>(IDataReader dr)
        {
            List<T> list = new List<T>();
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (HasColumn(dr, prop.Name))
                    {
                        prop.SetValue(obj, dr[prop.Name]);
                    }
                    //obj.SetPropertyValue(prop.Name, dr[prop.Name]);

                }
                list.Add(obj);
            }
            return list;
        }


        //public string GetSingleValue(string sql)
        //{
        //    var res = context.Database.SqlQuery<int?>(sql).FirstOrDefault();
        //    res = res ?? 0;
        //    return res + "";
        //}

        //public string GetSingleStringValue(string sql)
        //{
        //    var res = context.Database.SqlQuery<string>(sql).FirstOrDefault();
        //    return res;
        //}


        private bool HasColumn(IDataReader dr, string columnName)
        {
            try
            {
                return dr.GetOrdinal(columnName) >= 0;
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
        }


        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    myContext.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

}