using System;
using System.Text;
using ApiCaller;
using ApiCaller.Common;
using ApiCaller.ScriptableObjects;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Editor
{
	[TestFixture]
	public class RestfulTests
	{
		[Test]
		public void Should_Copy_ScriptableObject_Success()
		{
			var requestData = Resources.Load<ApiRequestData>("Test");
		}
	}
}