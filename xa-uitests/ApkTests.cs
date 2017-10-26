using System;
using System.IO;
using System.Threading;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Android;
using Xamarin.UITest.Utils;


[TestFixture]
public class TestCloud
{
	AndroidApp app;

	[SetUp]
	public void SetUp()
	{
		var apk = Path.Combine (Path.GetDirectoryName (GetType ().Assembly.Location), "..", "..", "..", "xamarin-android", "bin", "TestRelease", "Xamarin.Android.Bcl_Tests.apk");

		app = ConfigureApp.Android
						  .ApkFile (apk)
						  .WaitTimes (new WaitTime ())
						  .StartApp ();
	}

	[Test]
	public void OnDeviceTests()
	{
		Console.WriteLine ("Starting tests...");
		app.Invoke("StartTests");

		while (bool.Parse(app.Invoke ("IsRunning").ToString()))
		{
			Console.WriteLine("Waiting on test completion...");
			Thread.Sleep (1000);
		}

		Console.WriteLine("Test run complete....");

		var testResults = app.Invoke ("EncodedTestResults").ToString ();
		if (string.IsNullOrEmpty (testResults) || testResults == "null") {
			var testFailure = app.Invoke ("TestRunFailure").ToString ();
			Assert.Fail (testFailure);
		} else {
			Assert.Pass (testResults);
		}
	}

	class WaitTime : IWaitTimes
	{
		public TimeSpan WaitForTimeout => TimeSpan.FromMinutes (5);

		public TimeSpan GestureWaitTimeout => TimeSpan.FromMinutes (5);

		public TimeSpan GestureCompletionTimeout => TimeSpan.FromMinutes (5);
	}
}
