.PHONY: dotnet/generate dotnet/test -dotnet/test

-include eng/Makefile

## Generate generated code
dotnet/generate:
	srgen -c Carbonfrost.Commons.Instrumentation.Resources.SR \
		-r Carbonfrost.Commons.Instrumentation.Automation.SR \
		--resx \
		dotnet/src/Carbonfrost.Commons.Instrumentation/Automation/SR.properties

	/bin/sh -c "t4 dotnet/src/Carbonfrost.Commons.Instrumentation/Automation/Preprocessor/Logger.tt -o dotnet/src/Carbonfrost.Commons.Instrumentation/Automation/Preprocessor/Logger.g.cs"

## Execute dotnet unit tests
dotnet/test: dotnet/publish -dotnet/test

-dotnet/test:
	fspec -i dotnet/test/Carbonfrost.UnitTests.Instrumentation/Content \
		dotnet/test/Carbonfrost.UnitTests.Instrumentation/bin/$(CONFIGURATION)/netcoreapp3.0/publish/Carbonfrost.UnitTests.Instrumentation.dll

