using Microsoft.CodeAnalysis.CSharp;
using System.Data;
using System.Reflection;

namespace AenEnterprise.FrontEndMvc.HelperMethods
{
    public static class Helper
    {
        public static DataTable listDataTable<T>(List<T> list)
        {
            DataTable dt = new DataTable(typeof(T).Name);
            //Get all the properties by using reflection
            PropertyInfo[] propts = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo pi in propts)
            {
                dt.Columns.Add(pi.Name);
            }

            foreach (T item in list)
            {
                var values = new object[propts.Length];
                for (int i = 0; i < propts.Length; i++)
                {
                    values[i] = propts[i].GetValue(item, null);
                }
                dt.Rows.Add(values);
            }
            return dt;
        }
    }
}
