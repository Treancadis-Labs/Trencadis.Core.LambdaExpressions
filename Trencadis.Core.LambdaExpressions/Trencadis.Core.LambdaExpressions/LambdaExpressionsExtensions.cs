// ---------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="LambdaExpressionsExtensions.cs" company="Trencadis">
// Copyright (c) 2016, Trencadis, All rights reserved
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------------------------

namespace Trencadis.Core.LambdaExpressions
{
	using System;
	using System.Linq.Expressions;

	/// <summary>
	/// Lambda expressions extensions
	/// </summary>
	public static class LambdaExpressionsExtensions
	{
		/// <summary>
		/// Gets the property name from a property name lambda expression
		/// </summary>
		/// <param name="propertyExpression">The expression which returns the property name</param>
		/// <returns>The name of the property</returns>
		public static string GetPropertyName(this LambdaExpression propertyExpression)
		{
			if (propertyExpression == null)
			{
				throw new ArgumentNullException("propertyExpression");
			}

			var body = propertyExpression.Body as System.Linq.Expressions.MemberExpression;
			if (body == null)
			{
				UnaryExpression castExpression = propertyExpression.Body as System.Linq.Expressions.UnaryExpression;
				if ((castExpression != null) && ((castExpression.NodeType == ExpressionType.Convert) || (castExpression.NodeType == ExpressionType.ConvertChecked)))
				{
					body = castExpression.Operand as System.Linq.Expressions.MemberExpression;
				}

				if (body == null)
				{
					throw new ArgumentException("Expression is not a MemberExpression");
				}
			}

			if (body.Member != null)
			{
				return body.Member.Name;
			}
			else
			{
				throw new MemberAccessException("Unable to get MemberInfo instance for the specified MemberExpression");
			}
		}

		/// <summary>
		/// Gets the name of a parameter at the specified index from the lambda expression
		/// </summary>
		/// <param name="lambdaExpression">The lambda expression</param>
		/// <param name="parameterIndex">The parameter index; If not specified, will try to pick the first parameter</param>
		/// <returns>The parameter name if found, empty string otherwise</returns>
		public static string GetParameterName(this LambdaExpression lambdaExpression, int parameterIndex = 0)
		{
			if (lambdaExpression == null)
			{
				throw new ArgumentNullException("lambdaExpression");
			}

			int parametersCount = (lambdaExpression.Parameters != null) ? lambdaExpression.Parameters.Count : 0;
			bool hasParameters = parametersCount > 0;

			bool zeroParametersButSpecifiedIndex = (!hasParameters) && (parameterIndex != 0);
			bool hasParametersButIndexOutOfRange = hasParameters && ((parameterIndex < 0) || (parameterIndex >= parametersCount));

			if (zeroParametersButSpecifiedIndex || hasParametersButIndexOutOfRange)
			{
				if (zeroParametersButSpecifiedIndex)
				{
					throw new IndexOutOfRangeException(string.Format("Parameter index out of range, it should be {0} because expression has no parameters", 0));
				}
				else if (hasParametersButIndexOutOfRange)
				{
					throw new IndexOutOfRangeException(string.Format("Parameter index out of range, it should be between {0} and {1}", 0, parametersCount - 1));
				}
			}

			if (hasParameters)
			{
				return lambdaExpression.Parameters[parameterIndex].Name;
			}

			return string.Empty;
		}
	}
}
