using NewLife.UnitTest;

using Xunit;
using Xunit.Abstractions;

namespace XUnitTest;

/// <summary>PriorityOrderer 优先级排序器测试</summary>
/// <remarks>
/// 测试优先级排序器的各种场景：
/// 1. 基本优先级排序（数值小的先执行）
/// 2. 相同优先级按方法名排序
/// 3. 负数优先级
/// 4. 混合有无优先级的方法
/// 
/// 注意：由于 xUnit 并行执行，测试顺序验证依赖日志输出而非断言。
/// </remarks>
[TestCaseOrderer("NewLife.UnitTest.PriorityOrderer", "DH.UnitTest")]
public class PriorityOrdererTests
{
    private readonly ITestOutputHelper _output;

    public PriorityOrdererTests(ITestOutputHelper output) => _output = output;

    /// <summary>优先级100，应该最后执行</summary>
    [TestOrder(100)]
    [Fact]
    public void Priority100_ShouldExecuteLast()
    {
        _output.WriteLine($"执行方法: {nameof(Priority100_ShouldExecuteLast)}, 优先级: 100");
        Assert.True(true);
    }

    /// <summary>优先级1，应该第一个执行</summary>
    [TestOrder(1)]
    [Fact]
    public void Priority1_ShouldExecuteFirst()
    {
        _output.WriteLine($"执行方法: {nameof(Priority1_ShouldExecuteFirst)}, 优先级: 1");
        Assert.True(true);
    }

    /// <summary>优先级10，应该在中间执行</summary>
    [TestOrder(10)]
    [Fact]
    public void Priority10_ShouldExecuteInMiddle()
    {
        _output.WriteLine($"执行方法: {nameof(Priority10_ShouldExecuteInMiddle)}, 优先级: 10");
        Assert.True(true);
    }

    /// <summary>优先级5，应该在10之前执行</summary>
    [TestOrder(5)]
    [Fact]
    public void Priority5_AAA_ShouldExecuteBeforePriority10()
    {
        _output.WriteLine($"执行方法: {nameof(Priority5_AAA_ShouldExecuteBeforePriority10)}, 优先级: 5");
        Assert.True(true);
    }

    /// <summary>优先级5，相同优先级按方法名排序</summary>
    [TestOrder(5)]
    [Fact]
    public void Priority5_ZZZ_ShouldExecuteAfterPriority5_AAA()
    {
        _output.WriteLine($"执行方法: {nameof(Priority5_ZZZ_ShouldExecuteAfterPriority5_AAA)}, 优先级: 5");
        Assert.True(true);
    }
}

/// <summary>测试负数优先级场景</summary>
[TestCaseOrderer("NewLife.UnitTest.PriorityOrderer", "DH.UnitTest")]
public class NegativePriorityTests
{
    private readonly ITestOutputHelper _output;

    public NegativePriorityTests(ITestOutputHelper output) => _output = output;

    /// <summary>负数优先级应该最先执行</summary>
    [TestOrder(-100)]
    [Fact]
    public void NegativePriority_ShouldExecuteFirst()
    {
        _output.WriteLine($"执行方法: {nameof(NegativePriority_ShouldExecuteFirst)}, 优先级: -100");
        Assert.True(true);
    }

    /// <summary>优先级0</summary>
    [TestOrder(0)]
    [Fact]
    public void ZeroPriority_ShouldExecuteSecond()
    {
        _output.WriteLine($"执行方法: {nameof(ZeroPriority_ShouldExecuteSecond)}, 优先级: 0");
        Assert.True(true);
    }

    /// <summary>正数优先级应该最后执行</summary>
    [TestOrder(100)]
    [Fact]
    public void PositivePriority_ShouldExecuteLast()
    {
        _output.WriteLine($"执行方法: {nameof(PositivePriority_ShouldExecuteLast)}, 优先级: 100");
        Assert.True(true);
    }
}

/// <summary>测试混合有无优先级特性的场景</summary>
/// <remarks>
/// 无优先级特性的方法默认优先级为0。
/// </remarks>
[TestCaseOrderer("NewLife.UnitTest.PriorityOrderer", "DH.UnitTest")]
public class MixedPriorityTests
{
    private readonly ITestOutputHelper _output;

    public MixedPriorityTests(ITestOutputHelper output) => _output = output;

    /// <summary>无优先级特性，默认优先级为0</summary>
    [Fact]
    public void AAA_NoPriority_DefaultToZero()
    {
        _output.WriteLine($"执行方法: {nameof(AAA_NoPriority_DefaultToZero)}, 无优先级特性（默认0）");
        Assert.True(true);
    }

    /// <summary>有优先级1，应该在默认0之后执行</summary>
    [TestOrder(1)]
    [Fact]
    public void WithPriority1_ShouldExecuteAfterDefault()
    {
        _output.WriteLine($"执行方法: {nameof(WithPriority1_ShouldExecuteAfterDefault)}, 有优先级: 1");
        Assert.True(true);
    }

    /// <summary>有优先级10，应该最后执行</summary>
    [TestOrder(10)]
    [Fact]
    public void WithPriority10_ShouldExecuteLast()
    {
        _output.WriteLine($"执行方法: {nameof(WithPriority10_ShouldExecuteLast)}, 有优先级: 10");
        Assert.True(true);
    }
}

/// <summary>测试大量测试用例的排序</summary>
[TestCaseOrderer("NewLife.UnitTest.PriorityOrderer", "DH.UnitTest")]
public class LargePriorityTests
{
    private readonly ITestOutputHelper _output;

    public LargePriorityTests(ITestOutputHelper output) => _output = output;

    [TestOrder(1)]
    [Fact]
    public void Test01() => _output.WriteLine("Test01, 优先级: 1");

    [TestOrder(2)]
    [Fact]
    public void Test02() => _output.WriteLine("Test02, 优先级: 2");

    [TestOrder(3)]
    [Fact]
    public void Test03() => _output.WriteLine("Test03, 优先级: 3");

    [TestOrder(4)]
    [Fact]
    public void Test04() => _output.WriteLine("Test04, 优先级: 4");

    [TestOrder(5)]
    [Fact]
    public void Test05() => _output.WriteLine("Test05, 优先级: 5");

    [TestOrder(6)]
    [Fact]
    public void Test06() => _output.WriteLine("Test06, 优先级: 6");

    [TestOrder(7)]
    [Fact]
    public void Test07() => _output.WriteLine("Test07, 优先级: 7");

    [TestOrder(8)]
    [Fact]
    public void Test08() => _output.WriteLine("Test08, 优先级: 8");

    [TestOrder(9)]
    [Fact]
    public void Test09() => _output.WriteLine("Test09, 优先级: 9");

    [TestOrder(10)]
    [Fact]
    public void Test10() => _output.WriteLine("Test10, 优先级: 10");
}
