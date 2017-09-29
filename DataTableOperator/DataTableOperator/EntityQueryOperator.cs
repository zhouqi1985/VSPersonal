using CommonLibs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Data.Entity;

namespace DataTableOperator
{
    public class EntityQueryOperator
    {
        public static Expression<Func<T, bool>> CreateDydaminWhereAndExpression<T>(T querymodel) where T : class
        {
            Expression<Func<T, bool>> finalexpression = PredicateExtensionses.True<T>();
            if (querymodel == null)
            {
                return finalexpression;
            }
            Type t = querymodel.GetType();
            PropertyInfo[] properties = t.GetProperties();
            foreach (PropertyInfo item in properties)
            {
                object value = item.GetValue(querymodel, null);
                if (value != null)
                {
                    Type valuetype = value.GetType();
                    if (valuetype != typeof(Int32) && valuetype != typeof(Int64))
                    {
                        if (valuetype == typeof(Guid) && (Guid)value != Guid.Empty)
                        {
                            Expression<Func<T, bool>> currentexpression = ExpressionOperator.CreateCompareExpression<T>(item.Name, value);
                            finalexpression = finalexpression.And<T>(currentexpression);
                        }
                        if (valuetype == typeof(DateTime) && (DateTime)value != DateTime.Parse("1900/1/1 0:00:00") && (DateTime)value != default(DateTime))
                        {
                            DateTime startdate = DateTime.Parse(Convert.ToDateTime(value).ToShortDateString());
                            DateTime enddate = startdate.AddDays(1);
                            Expression<Func<T, bool>> startexpression = ExpressionOperator.CreateCompareExpression<T>(item.Name, startdate, "greaterthanorequal");
                            Expression<Func<T, bool>> endexpression = ExpressionOperator.CreateCompareExpression<T>(item.Name, enddate, "lessthan");
                            finalexpression = finalexpression.And<T>(startexpression).And<T>(endexpression);

                        }
                        if (valuetype != typeof(DateTime) && valuetype != typeof(Boolean) && valuetype != typeof(Guid))
                        {
                            Expression<Func<T, bool>> currentexpression = ExpressionOperator.CreateCompareExpression<T>(item.Name, value);
                            finalexpression = finalexpression.And<T>(currentexpression);
                        }
                    }
                    else
                    {
                        long convervalue = Convert.ToInt64(value);
                        if (convervalue != 0)
                        {
                            Expression<Func<T, bool>> currentexpression = ExpressionOperator.CreateCompareExpression<T>(item.Name, value);
                            finalexpression = finalexpression.And<T>(currentexpression);
                        }
                    }

                }

            }
            return finalexpression;
        }


    }
}

