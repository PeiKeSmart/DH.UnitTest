using System.Threading;
using Xunit;
using Xunit.Abstractions;

namespace XUnitTest;

[TestCaseOrderer("NewLife.UnitTest.DefaultOrderer", "NewLife.UnitTest")]
public class MethodOrderTests
{
    private readonly ITestOutputHelper _output;

    public MethodOrderTests(ITestOutputHelper output) => _output = output;

    [Fact]
    public void Test3()
    {
        _output.WriteLine("Test3");

        Thread.Sleep(1000);
    }

    [Fact]
    public void Test2() => Thread.Sleep(1000);

    [Fact]
    public void Test1() => Thread.Sleep(1000);

    [Fact]
    public void Test5() => Thread.Sleep(1000);

    [Fact]
    public void Test4() => Thread.Sleep(1000);
}