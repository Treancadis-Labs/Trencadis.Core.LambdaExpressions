// ---------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="LambdaExpressionsExtensionsTest.cs" company="Trencadis">
// Copyright (c) 2016, Trencadis, All rights reserved
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------------------------

namespace Trencadis.Core.LambdaExpressions.Tests
{
	using System;
	using System.Linq.Expressions;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	/// <summary>
	/// Test class for LambdaExpressionsExtensions
	/// </summary>
	[TestClass]
	public class LambdaExpressionsExtensionsTest
	{
		/// <summary>
		/// Tests the happy case of GetPropertyName, when the lambda expression doesn't use any parameter(s)
		/// </summary>
		[TestMethod]
		public void GetPropertyName_Test_HappyCase_NoParameters()
		{
			var inst = new TestClass();

			Expression<Func<string>> expression1 = () => inst.StringField;
			Expression<Func<int>> expression2 = () => inst.IntField;

			string fieldName1 = expression1.GetPropertyName();
			string fieldName2 = expression2.GetPropertyName();

			Assert.AreEqual("StringField", fieldName1);
			Assert.AreEqual("IntField", fieldName2);
		}

		/// <summary>
		/// Tests the happy case of GetPropertyName, when the lambda expression uses parameter(s)
		/// </summary>
		[TestMethod]
		public void GetPropertyName_Test_HappyCase_WithParameters()
		{
			Expression<Func<TestClass, string>> expression1 = (cls) => cls.StringField;
			Expression<Func<TestClass, int>> expression2 = (cls) => cls.IntField;

			string fieldName1 = expression1.GetPropertyName();
			string fieldName2 = expression2.GetPropertyName();

			Assert.AreEqual("StringField", fieldName1);
			Assert.AreEqual("IntField", fieldName2);
		}

		/// <summary>
		/// Tests the GetPropertyName, when the lambda expression performs an implicit conversion
		/// </summary>
		[TestMethod]
		public void GetPropertyName_Test_WithImplicitConversion_NoParameters()
		{
			var inst = new TestClass();

			Expression<Func<long>> expression = () => inst.IntField;

			string fieldName = expression.GetPropertyName();

			Assert.AreEqual("IntField", fieldName);
		}

		/// <summary>
		/// Tests the GetPropertyName, when the lambda expression requires an explicit cast
		/// </summary>
		[TestMethod]
		public void GetPropertyName_Test_WithExplicitConversion_NoParameters()
		{
			var inst = new TestClass();

			Expression<Func<short>> expression = () => (short)inst.IntField;

			string fieldName = expression.GetPropertyName();

			Assert.AreEqual("IntField", fieldName);
		}

		/// <summary>
		/// Tests the GetPropertyName, when the lambda expression is null
		/// </summary>
		[TestMethod]
		public void GetPropertyName_Test_ThrowsForNullExpressions()
		{
			var inst = new TestClass();

			Expression<Func<int>> expression = null;
			Exception expectedException = null;

			try
			{
				string fieldName = expression.GetPropertyName();
			}
			catch (Exception ex)
			{
				expectedException = ex;
			}

			Assert.IsNotNull(expectedException);
			Assert.IsInstanceOfType(expectedException, typeof(ArgumentNullException));
		}

		/// <summary>
		/// Tests the GetPropertyName, when the lambda expression is null
		/// </summary>
		[TestMethod]
		public void GetPropertyName_Test_ThrowsForNonMemberExpressions()
		{
			var inst = new TestClass();

			Expression<Func<string, bool>> expression = (s) => string.IsNullOrEmpty(s);
			Exception expectedException = null;

			try
			{
				string fieldName = expression.GetPropertyName();
			}
			catch (Exception ex)
			{
				expectedException = ex;
			}

			Assert.IsNotNull(expectedException);
			Assert.IsInstanceOfType(expectedException, typeof(ArgumentException));
			Assert.AreEqual("Expression is not a MemberExpression", expectedException.Message);
		}

		/// <summary>
		/// Tests the happy case of GetParameterName, when the lambda expression has one parameter
		/// </summary>
		[TestMethod]
		public void GetParameterName_Test_HappyCase_OneParameter()
		{
			Expression<Func<TestClass, string>> expression = (inst) => inst.StringField;

			string parameterName = expression.GetParameterName();

			Assert.AreEqual("inst", parameterName);
		}

		/// <summary>
		/// Tests the happy case of GetParameterName, when the lambda expression has one parameter and passes explicitly the parameter index
		/// </summary>
		[TestMethod]
		public void GetParameterName_Test_HappyCase_OneParameter_ExplicitParameterIndex()
		{
			Expression<Func<TestClass, string>> expression = (inst) => inst.StringField;

			string parameterName = expression.GetParameterName(0);

			Assert.AreEqual("inst", parameterName);
		}

		/// <summary>
		/// Tests the GetPropertyName, when the lambda expression is null
		/// </summary>
		[TestMethod]
		public void GetParameterName_Test_ThrowsForNullExpressions()
		{
			Expression<Func<TestClass, int>> expression = null;
			Exception expectedException = null;

			try
			{
				string parameterName = expression.GetParameterName();
			}
			catch (Exception ex)
			{
				expectedException = ex;
			}

			Assert.IsNotNull(expectedException);
			Assert.IsInstanceOfType(expectedException, typeof(ArgumentNullException));
		}

		/// <summary>
		/// Tests the GetPropertyName, when the lambda expression has zero parameters
		/// </summary>
		[TestMethod]
		public void GetParameterName_Test_ReturnsEmptyForZeroParameters()
		{
			var inst = new TestClass();

			Expression<Func<string>> expression = () => inst.StringField;
			Exception expectedException = null;
			string parameterName = null;

			try
			{
				parameterName = expression.GetParameterName();
			}
			catch (Exception ex)
			{
				expectedException = ex;
			}

			Assert.IsNull(expectedException);
			Assert.AreEqual(string.Empty, parameterName);
		}

		/// <summary>
		/// Tests the GetPropertyName, when the lambda expression has zero parameters but the caller has specified a non-zero parameter index
		/// </summary>
		[TestMethod]
		public void GetParameterName_Test_ThrowsForZeroParametersButExplicitNonzeroParameterIndex()
		{
			var inst = new TestClass();

			Expression<Func<string>> expression = () => inst.StringField;
			Exception expectedException = null;
			string parameterName = null;

			try
			{
				parameterName = expression.GetParameterName(1);
			}
			catch (Exception ex)
			{
				expectedException = ex;
			}

			Assert.IsNotNull(expectedException);
			Assert.IsInstanceOfType(expectedException, typeof(IndexOutOfRangeException));
		}

		/// <summary>
		/// Tests the GetPropertyName, when the lambda expression has parameters but the specified index is out of range (less than zero)
		/// </summary>
		[TestMethod]
		public void GetParameterName_Test_ThrowsParameterIndexOutOfRange_Lower()
		{
			Expression<Func<TestClass, string>> expression = (inst) => inst.StringField;
			Exception expectedException = null;
			string parameterName = null;

			try
			{
				parameterName = expression.GetParameterName(-1);
			}
			catch (Exception ex)
			{
				expectedException = ex;
			}

			Assert.IsNotNull(expectedException);
			Assert.IsInstanceOfType(expectedException, typeof(IndexOutOfRangeException));
		}

		/// <summary>
		/// Tests the GetPropertyName, when the lambda expression has parameters but the specified index is out of range (more than max-index)
		/// </summary>
		[TestMethod]
		public void GetParameterName_Test_ThrowsParameterIndexOutOfRange_Higher()
		{
			Expression<Func<TestClass, string>> expression = (inst) => inst.StringField;
			Exception expectedException = null;
			string parameterName = null;

			try
			{
				parameterName = expression.GetParameterName(1);
			}
			catch (Exception ex)
			{
				expectedException = ex;
			}

			Assert.IsNotNull(expectedException);
			Assert.IsInstanceOfType(expectedException, typeof(IndexOutOfRangeException));
		}
	}
}
