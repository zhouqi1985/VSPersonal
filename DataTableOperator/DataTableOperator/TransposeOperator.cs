using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DataTableOperator
{
    public class TransposeOperator
    {
        public static DataTable ListTransposeToDT<T, U, V, W>(List<T> lists, Expression<Func<T, U>> colconfig, Expression<Func<T, V>> rowconfig, Expression<Func<T, W>> calconfig, string FirstColName)
        {
            DataTable dt = new DataTable();
            IEnumerable<U> ColumnNames = lists.OrderBy(colconfig.Compile()).Select(colconfig.Compile()).Distinct();
            IEnumerable<V> FirstColumns = lists.OrderBy(rowconfig.Compile()).Select(rowconfig.Compile()).Distinct();
            dt.Columns.Add(FirstColName, typeof(U));
            foreach (V fc in FirstColumns)
            {
                dt.Rows.Add(fc);
            }
            foreach (U columnname in ColumnNames)
            {
                dt.Columns.Add(columnname.ToString(), typeof(U));
            }
            foreach (DataColumn dc in dt.Columns)
            {
                U name = (U)Convert.ChangeType(dc.ColumnName, typeof(U));
                if (!name.ToString().Equals(FirstColName))
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        V querycontent = (V)Convert.ChangeType(dt.Rows[i][FirstColName], typeof(V));
                        Expression<Func<T, bool>> checkcol = ExpressionOperator.CreateCompareExpression(colconfig, name);
                        Expression<Func<T, bool>> checkrow = ExpressionOperator.CreateCompareExpression(rowconfig, querycontent);
                        dt.Rows[i][name.ToString()] = lists.Where(checkcol.Compile()).Where(checkrow.Compile()).Select(calconfig.Compile()).FirstOrDefault();
                    }
                }
            }
            return dt;
        }
    }
}
