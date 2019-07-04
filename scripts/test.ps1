dotnet publish ..\tests\Ordering.App.Tests
dotnet test ..\tests\Ordering.Domain.Tests

dotnet publish ..\tests\Ordering.Domain.Tests
dotnet test ..\tests\Ordering.App.Tests