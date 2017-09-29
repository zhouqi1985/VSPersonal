using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DataTableOperator
{
    public class ExpressionOperator
    {
        public static Expression<Func<T, Boolean>> CreateCompareExpression<T, U, V>(Expression<Func<T, U>> exp, V conval)
        {
            MemberExpression expname = (MemberExpression)exp.Body;
            return CreateCompareExpression<T>(expname.Member.Name, conval);
        }

        public static Expression<Func<T, bool>> CreateCompareExpression<T>(string membername, object membervalue, string operat = "equal")
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
            MemberExpression member1 = Expression.PropertyOrField(parameter, membername);
            ConstantExpression constant = Expression.Constant(membervalue);
            BinaryExpression bin;
            operat = operat.ToLower();
            switch (operat)
            {
                case "lessthan":
                    bin = Expression.LessThan(member1, constant);
                    break;
                case "lessthanorequal":
                    bin = Expression.LessThanOrEqual(member1, constant);
                    break;
                case "greaterthan":
                    bin = Expression.GreaterThan(member1, constant);
                    break;
                case "greaterthanorequal":
                    bin = Expression.GreaterThanOrEqual(member1, constant);
                    break;
                default:
                    bin = Expression.Equal(member1, constant);
                    break;
            }
            Expression<Func<T, Boolean>> lamda = Expression.Lambda<Func<T, Boolean>>(bin, parameter);
            return lamda;
        }

    }
}
