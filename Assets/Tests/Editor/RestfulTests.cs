using System;
using System.Text;
using ApiCaller.Domain;
using ApiCaller.ScriptableObjects;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Editor
{
	[TestFixture]
	public class RestfulTests
	{
		[Test]
		public void Should_Builder_Success()
		{
			var body = new TestRequestData("1", "2");

			var request = new ApiRequest()
			              .SetUrl("https://example.com/api/test")
			              .SetMethod(Method.POST)
			              .SetTimeout(60)
			              .AddHeader("Content-Type", "application/json")
			              .AddHeader("Authorization", "123456")
			              .AddQuery("test1", "1")
			              .AddQuery("test2", "2")
			              .SetBody(body)
			              .Build();

			Assert.AreEqual("https://example.com/api/test?test1=1&test2=2", request.url);
			Assert.AreEqual(60, request.timeout);
			Assert.AreEqual("POST", request.method);
			Assert.AreEqual("application/json", request.GetRequestHeader("Content-Type"));
			Assert.AreEqual("123456", request.GetRequestHeader("Authorization"));
			Assert.AreEqual(Encoding.UTF8.GetBytes(JsonUtility.ToJson(body)), request.uploadHandler.data);
		}

		[Test]
		public void Should_Copy_ScriptableObject_Success()
		{
			var request = new ApiRequest()
			              .UseApiRequestData(Resources.Load<ApiRequestData>("Test"))
			              .Build();

			Assert.AreEqual("https://example.com/api/test", request.url);
			Assert.AreEqual(60, request.timeout);
			Assert.AreEqual("POST", request.method);
			Assert.AreEqual("application/json", request.GetRequestHeader("Content-Type"));
			Assert.AreEqual("123456", request.GetRequestHeader("Authorization"));
		}
	}

	[Serializable]
	public struct TestRequestData
	{
		public string test1;
		public string test2;

		public TestRequestData(string test1, string test2)
		{
			this.test1 = test1;
			this.test2 = test2;
		}
	}
}