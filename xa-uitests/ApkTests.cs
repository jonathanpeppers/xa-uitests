using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Android;
using System.IO;
using Xamarin.UITest.Utils;
using System.Threading.Tasks;
using System.IO.Compression;


[TestFixture]
public class TestCloud
{
	//static IEnumerable Results
	//{
	//	get
	//	{
	//		var app = ConfigureApp.Android
	//							  .ApkFile("/Users/jonathanpeppers/Desktop/Git/xamarin-android/bin/TestDebug/Xamarin.Android.JcwGen_Tests.apk")
	//							  .StartApp();

	//		string result = app.Invoke("RunTests").ToString();
	//		//Console.WriteLine(result);

	//		var xml = new XmlDocument();
	//		xml.LoadXml(result);

	//		var results = new List<TestCaseData>();
	//		foreach (XmlElement node in xml.SelectNodes("//test-case"))
	//		{
	//			string fullName = node.Attributes["name"].Value;
	//			string name = fullName.Split(',').Last().Trim();
	//			string testResult = node.Attributes["result"].Value;
	//			var data = new TestCaseData(testResult)
	//				.SetName(name)
	//				.SetDescription(fullName);
	//			foreach (XmlAttribute attribute in node.Attributes)
	//			{
	//				data.SetProperty(attribute.Name, attribute.Value);
	//			}
	//			results.Add(data);
	//		}
	//		return results;
	//	}
	//}
	//[Test, TestCaseSource("Results")]
	//public void Test(string result)
	//{
	//	switch (result)
	//	{
	//		case "Success":
	//			break;
	//		case "Failure":
	//			Assert.Fail();
	//			break;
	//		default:
	//			Assert.Inconclusive("Unknown result: " + result);
	//			break;
	//	}
	//}

	AndroidApp app;
	[SetUp]
	public void BeforeEachTest()
	{
		app = ConfigureApp.Android
						  .ApkFile("/Users/clancey/Projects/xamarin-android/bin/TestDebug/Xamarin.Android.Bcl_Tests.apk").WaitTimes(new WaitTime())
						  .StartApp();
	}
	[Test]
		public void DeviceTest()
		{

			var started = bool.Parse(app.Invoke("RunTests").ToString());
			bool isRunning = started;
			while (bool.Parse(app.Invoke("IsRunning").ToString()))
			{
				Task.Delay(1000).Wait();
			}

			var result = app.Invoke("TestResults").ToString();
			//var bytes = System.Convert.FromBase64String(r);
			//string result = null;
			//using (MemoryStream ms = new MemoryStream(bytes))
			//{
			//	using (var decompressedMs = new MemoryStream())
			//	{
			//		using (var gzs = new GZipStream(ms,
			//		 CompressionMode.Decompress))
			//		{
			//			gzs.CopyTo(decompressedMs);
			//		}

			//		result = System.Text.Encoding.UTF8.GetString(decompressedMs.ToArray());
			//	}
			//}
			bool success = bool.Parse(app.Invoke("TestRunSuccess").ToString());
			if (success)
				Assert.Pass(result);
			else
				Assert.Fail(result);
		}

	class WaitTime : IWaitTimes
	{
		public TimeSpan WaitForTimeout => TimeSpan.FromMinutes(5);

		public TimeSpan GestureWaitTimeout => TimeSpan.FromMinutes(5);

		public TimeSpan GestureCompletionTimeout => TimeSpan.FromMinutes(5);
	}
}
