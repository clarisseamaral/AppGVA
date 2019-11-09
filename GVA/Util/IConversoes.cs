using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace GVA.Util
{
    public static class IConversoes
    {
        //Fonte: https://www.c-sharpcorner.com/UploadFile/ee01e6/different-way-to-convert-datatable-to-list/
        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        //Fonte: https://www.c-sharpcorner.com/UploadFile/ee01e6/different-way-to-convert-datatable-to-list/
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }
    }
}