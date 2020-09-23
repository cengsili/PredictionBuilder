using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;

namespace PredictionBuilder
{
    public class ExpressionBuilder<T, F> //where F : class
    {
        private Expression<Func<T, bool>> predicate = PredicateBuilder.True<T>();
        private F filterClass;
        public ExpressionBuilder(F filterClass)
        {
            this.filterClass = filterClass;
        }
        public Expression<Func<T, bool>> Build()
        {

            filterClass.GetType().GetProperties().ToList().ForEach(e =>
            {
                var attr = (PredicateAttribute)e.GetCustomAttribute(typeof(PredicateAttribute));
                var value = e.GetValue(filterClass);

                if (attr != null && !String.IsNullOrEmpty(attr.AttributeName) && value != null)
                {
                    var parameter = Expression.Parameter(typeof(T), "entity");
                    PropertyInfo leftProperty = parameter.Type.GetProperty(attr.AttributeName);
                    var left = Expression.Property(parameter, leftProperty);
                    var right = Expression.Constant(e.GetValue(filterClass), e.PropertyType);
                    Expression<Func<T, bool>> expression = null;
                    BinaryExpression exp = null;
                    if (attr.Option == PredicateOption.Equal)
                    {
                        exp = Expression.Equal(left, right);
                        expression = (Expression<Func<T, bool>>)Expression.Lambda(exp, parameter);
                    }
                    else if (attr.Option == PredicateOption.NotEqual)
                    {
                        exp = Expression.NotEqual(left, right);
                        expression = (Expression<Func<T, bool>>)Expression.Lambda(exp, parameter);
                    }
                    else if (attr.Option == PredicateOption.Contains && e.GetValue(filterClass) != null)
                    {
                        expression = ContainsPredicate<T>(e.GetValue(filterClass), attr.AttributeName);
                    }
                    else if (attr.Option == PredicateOption.GreaterThan)
                    {
                        exp = Expression.GreaterThan(left, right);
                        expression = (Expression<Func<T, bool>>)Expression.Lambda(exp, parameter);
                    }
                    else if (attr.Option == PredicateOption.GreaterThanOrEqual)
                    {
                        exp = Expression.GreaterThanOrEqual(left, right);
                        expression = (Expression<Func<T, bool>>)Expression.Lambda(exp, parameter);
                    }
                    else if (attr.Option == PredicateOption.LessThan)
                    {
                        exp = Expression.LessThan(left, right);
                        expression = (Expression<Func<T, bool>>)Expression.Lambda(exp, parameter);
                    }
                    else if (attr.Option == PredicateOption.LessThanOrEqual)
                    {
                        exp = Expression.LessThanOrEqual(left, right);
                        expression = (Expression<Func<T, bool>>)Expression.Lambda(exp, parameter);
                    }
                    if (expression != null)
                    {

                        predicate = predicate.And(expression);
                    }
                }
            });
            return predicate;
        }

        public Expression<Func<T, bool>> ContainsPredicate<T>(object arr, string fieldname) //where T : class
        {
            ParameterExpression entity = Expression.Parameter(typeof(T), "entity");
            MemberExpression member = Expression.Property(entity, fieldname);
            MethodInfo method = typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public)
               .FirstOrDefault(x => x.Name == "Contains" && x.GetParameters().Count() == 2);
            method = method.MakeGenericMethod(member.Type);
            var exprContains = Expression.Call(method, new Expression[] { Expression.Constant(arr), member });
            return Expression.Lambda<Func<T, bool>>(exprContains, entity);
        }
    }

}
