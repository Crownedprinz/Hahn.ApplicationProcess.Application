using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace  HAF.Domain
{
    /// <summary>This is helper class contains methods to make working with specific expressions simpler.</summary>
    public static class ExpressionsHelper
    {
        public static MethodCallExpression GetMethodCallExpression(this LambdaExpression expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            var methodCallExpression = expression.Body as MethodCallExpression;
            if (methodCallExpression == null)
            {
                throw new ArgumentException(
                    "The expression's body must be a MethodCallExpression. The code block supplied should invoke a method.\nExample: x => x.Foo().",
                    nameof(expression));
            }

            return methodCallExpression;
        }

        /// <summary>Gets the name of the parameter used in the expression.</summary>
        /// <typeparam name="T">The type of the parameter used in the expression </typeparam>
        /// <param name="parameterExpression">The parameter expression.</param>
        /// <returns>The name of the parameter used in the expression.</returns>
        [SuppressMessage(
            "Microsoft.Design",
            "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "A different design is not necessary as these generic parameters are infered.")]
        [SuppressMessage(
            "Microsoft.Design",
            "CA1011:ConsiderPassingBaseTypesAsParameters",
            Justification = "Using the base type would reduce discoverability of the API.")]
        public static string GetParameterName<T>(Expression<Func<T>> parameterExpression)
        {
            if (parameterExpression == null)
                throw new ArgumentNullException(nameof(parameterExpression));

            var body = parameterExpression.Body as MemberExpression;
            if (body == null)
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        "'parameterExpression' should be a member expression, but it is a {0}",
                        parameterExpression.Body.GetType()));
            }

            var propertyInfo = body.Member as PropertyInfo;
            if (propertyInfo != null)
                return propertyInfo.Name;

            var fieldInfo = body.Member as FieldInfo;
            if (fieldInfo != null)
                return fieldInfo.Name;

            throw new ArgumentException(
                string.Format(
                    CultureInfo.CurrentCulture,
                    "The member used in the expression should be a property or a field, but it is a {0}",
                    body.Member.GetType()));
        }

        /// <summary>Gets the <see cref="PropertyInfo" /> for the property used in the expression.</summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <returns>The <see cref="PropertyInfo" /> for the property used in the expression.</returns>
        public static PropertyInfo GetPropertyInfo(LambdaExpression propertyExpression)
        {
            if (propertyExpression == null)
                throw new ArgumentNullException(nameof(propertyExpression));

            var body = propertyExpression.Body as MemberExpression;
            if (body == null)
            {
                var convert = propertyExpression.Body as UnaryExpression;
                if (convert != null)
                    body = convert.Operand as MemberExpression;
            }

            if (body == null)
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        "'propertyExpression' should be a member expression, but it is a {0}",
                        propertyExpression.Body.GetType()));
            }

            var propertyInfo = body.Member as PropertyInfo;
            if (propertyInfo == null)
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        "The member used in the expression should be a property, but it is a {0}",
                        body.Member.GetType()));
            }

            return propertyInfo;
        }

        /// <summary>Gets the name of the property used in the expression.</summary>
        /// <typeparam name="TProperty"> The type of the property </typeparam>
        /// <param name="propertyExpression">The property expression.</param>
        /// <returns>The name of the property used in the expression.</returns>
        [SuppressMessage(
            "Microsoft.Design",
            "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "A different design is not necessary as these generic parameters are infered.")]
        [SuppressMessage(
            "Microsoft.Design",
            "CA1011:ConsiderPassingBaseTypesAsParameters",
            Justification = "Using the base type would reduce discoverability of the API.")]
        public static string GetPropertyName<TProperty>(Expression<Func<TProperty>> propertyExpression) =>
            GetPropertyInfo(propertyExpression).Name;

        /// <summary>Gets the name of the property used in the expression.</summary>
        /// <typeparam name="TClass"> The type of the class the property is declared in.</typeparam>
        /// <typeparam name="TProperty"> The type of the property </typeparam>
        /// <param name="propertyExpression">The property expression.</param>
        /// <returns>The name of the property used in the expression.</returns>
        [SuppressMessage(
            "Microsoft.Design",
            "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "A different design is not necessary as these generic parameters are infered.")]
        [SuppressMessage(
            "Microsoft.Design",
            "CA1011:ConsiderPassingBaseTypesAsParameters",
            Justification = "Using the base type would reduce discoverability of the API.")]
        public static string GetPropertyName<TClass, TProperty>(Expression<Func<TClass, TProperty>> propertyExpression) =>
            GetPropertyInfo(propertyExpression).Name;
    }
}