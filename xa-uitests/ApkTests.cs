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
