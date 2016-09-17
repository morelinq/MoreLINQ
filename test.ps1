Remove-Item -Recurse .\tools\NUnit.Runners.*
.\tools\nuget install NUnit.Runners -Version 2.6.4 -OutputDirectory tools

.\tools\NUnit.Runners.*\tools\nunit-console.exe .\MoreLinq.Test\bin\Debug\net40\MoreLinq.Test.dll
.\tools\NUnit.Runners.*\tools\nunit-console.exe .\MoreLinq.Test\bin\Release\net40\MoreLinq.Test.dll
.\tools\NUnit.Runners.*\tools\nunit-console.exe .\MoreLinq.Test\bin\Debug\net40-client\MoreLinq.Test.dll
.\tools\NUnit.Runners.*\tools\nunit-console.exe .\MoreLinq.Test\bin\Release\net40-client\MoreLinq.Test.dll