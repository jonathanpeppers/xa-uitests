CONFIGURATION = Release
MSBUILD       = msbuild
MSBUILD_FLAGS = /p:Configuration=$(CONFIGURATION) $(MSBUILD_ARGS)
NUNIT         = packages/NUnit.Runners.2.6.4/tools/nunit-console.exe
TEST_SLN      = xa-uitests.sln
XA_DIR        = xamarin-android
TEST_CLOUD    = packages/Xamarin.UITest.2.1.4/tools/test-cloud.exe
DEVICES       = bd59174b
APK           = $(XA_DIR)/bin/Test$(CONFIGURATION)/Xamarin.Android.Bcl_Tests.apk

RESULTS_ORIG  = TestResult-Original.xml
RESULTS_DEST  = TestResult-Final.xml

TEST_APK_PROJECTS = \
	$(XA_DIR)/src/Mono.Android/Test/Mono.Android-Tests.csproj \
	$(XA_DIR)/tests/CodeGen-Binding/Xamarin.Android.JcwGen-Tests/Xamarin.Android.JcwGen-Tests.csproj \
	$(XA_DIR)/tests/locales/Xamarin.Android.Locale-Tests/Xamarin.Android.Locale-Tests.csproj \
	$(XA_DIR)/tests/Xamarin.Android.Bcl-Tests/Xamarin.Android.Bcl-Tests.csproj \
	$(XA_DIR)/tests/Xamarin.Forms-Performance-Integration/Droid/Xamarin.Forms.Performance.Integration.Droid.csproj

all:: ui-tests xamarin-android apks

xamarin-android::
	$(MAKE) -C $(XA_DIR) prepare all all-tests MSBUILD="$(MSBUILD)" CONFIGURATION="$(CONFIGURATION)"

ui-tests::
	nuget restore $(TEST_SLN)
	$(MSBUILD) $(MSBUILD_FLAGS) $(TEST_SLN)

run-local::
	mono $(NUNIT) xa-uitests/bin/$(CONFIGURATION)/xa-uitests.dll --result=$(RESULTS_ORIG)
	dotnet TestCloudNunit/bin/$(CONFIGURATION)/netcoreapp2.0/TestCloudNunit.dll $(RESULTS_ORIG) $(RESULTS_DEST)

run-test-cloud::
	mono $(TEST_CLOUD) submit $(APK) $(TEST_CLOUD_KEY) \
		--devices $(DEVICES) --series master --locale en_US \
		--user $(TEST_CLOUD_USER) --assembly-dir xa-uitests/bin/$(CONFIGURATION) \
		--nunit-xml $(RESULTS_ORIG)
	dotnet TestCloudNunit/bin/$(CONFIGURATION)/netcoreapp2.0/TestCloudNunit.dll $(RESULTS_ORIG) $(RESULTS_DEST)

define BUILD_TEST_APK
	MSBUILD="$(MSBUILD)" $(XA_DIR)/tools/scripts/xabuild $(MSBUILD_FLAGS) /t:SignAndroidPackage $(1)
endef

apks::
	$(foreach p, $(TEST_APK_PROJECTS), $(call BUILD_TEST_APK, $(p)))