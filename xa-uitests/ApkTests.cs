using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NUnit.Framework;
using Xamarin.UITest;

[TestFixture]
public class TestCloud
{
	static IEnumerable Results
	{
		get
		{
			var app = ConfigureApp.Android
								  .ApkFile("/Users/jonathanpeppers/Desktop/Git/xamarin-android/bin/TestDebug/Xamarin.Android.JcwGen_Tests.apk")
								  .StartApp();

			string result = app.Invoke("RunTests").ToString();
			Console.WriteLine(result);

			var xml = new XmlDocument();
			xml.LoadXml(result);

			var results = new List<TestCaseData>();
			foreach (XmlElement node in xml.SelectNodes("//test-case"))
			{
				string fullName = node.Attributes["name"].Value;
				string name = fullName.Split(',').Last().Trim();
				string testResult = node.Attributes["result"].Value;
				var data = new TestCaseData(testResult)
					.SetName(name)
					.SetDescription(fullName);
				foreach (XmlAttribute attribute in node.Attributes)
				{
					data.SetProperty(attribute.Name, attribute.Value);
				}
				results.Add(data);
			}
			return results;
		}
	}

	[Test, TestCaseSource("Results")]
	public void Test(string result)
	{
		switch (result)
		{
			case "Success":
				break;
			case "Failure":
				Assert.Fail();
				break;
			default:
				Assert.Inconclusive("Unknown result: " + result);
				break;
		}
	}
}
