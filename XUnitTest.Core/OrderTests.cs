using System.Threading;
using NewLife.UnitTest;
using Xunit;
using Xunit.Abstractions;

namespace XUnitTest;

[TestCaseOrderer("NewLife.UnitTest.PriorityOrderer", "NewLife.UnitTest")]
public class OrderTests
{
    private readonly ITestOutputHelper _output;

    public OrderTests(ITestOutputHelper output) => _output = output;

    [TestOrder(3)]
    [Fact]
    public void Test1()
    {
        Thread.Sleep(1000);

        _output.WriteLine("Test1");
    }

    [TestOrder(1)]
    [Fact]
    public void Test2() => Thread.Sleep(1000);

    [TestOrder(2)]
    [Fact]
    public void Test3() => Thread.Sleep(1000);

    [TestOrder(5)]
    [Fact]
    public void Test4() => Thread.Sleep(1000);

    [TestOrder(4)]
    [Fact]
    public void Test5() => Thread.Sleep(1000);
}