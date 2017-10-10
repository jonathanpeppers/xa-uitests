using System;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Android;

namespace Xamarin.Android.UITests
{
	[TestFixture]
	public class ApkTests
	{
		AndroidApp app;

		[SetUp]
		public void SetUp()
		{
			app = ConfigureApp.Android
				.ApkFile("/Users/jonathanpeppers/Desktop/Git/xamarin-android/bin/TestDebug/Xamarin.Android.JcwGen_Tests.apk")
				.StartApp();
		}

		[Test]
		public void RunAllTests()
		{
			app.Tap(q => q.Marked("Run Tests"));

			app.WaitForElement(q => q.Marked("Passed"));
		}
	}
}
