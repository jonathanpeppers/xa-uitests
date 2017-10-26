# xa-uitests
Prototype of running Xamarin.Android apk tests in Test Cloud

## To Build

    make

1. Builds xa-uitest.sln
1. Builds xamarin-android
1. Builds xamarin-android test APKs

## Running Tests Locally

    make run-local

If you have an emulator open/device connected, this will run the BCL tests locally.

You will get two resulting files:

1. `TestResult-Original.xml` - the original NUnit results from the xa-uitests project
1. `TestResult-Final*.xml` - the on-device NUnit test results

## Running on Test Cloud

    make run-test-cloud TEST_CLOUD_KEY=1234 TEST_CLOUD_USER=youruser@something.com

The resulting files should be the same as if you ran locally, except the final file will have the name of the device.

## Notes / TODO

1. `TestCloudNUnit` is .NET Core project, and run via `dotnet`. Perhaps an MSBuild task would be better.
1. `TestCloudNUnit` probably needs some cleanup. It prints `Hello World!`.
1. [API changes](https://github.com/xamarin/xamarin-android/compare/master...jonathanpeppers:uitest-support) in xamarin-android/Xamarin.Android.NUnitLite should be reviewed
1. I did not add the required `[Export]` methods to all APK tests, or setup a way for xa-uitests to run against different APKs.