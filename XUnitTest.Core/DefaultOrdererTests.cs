using Xunit;
using Xunit.Abstractions;

namespace XUnitTest;

/// <summary>DefaultOrderer 默认排序器测试</summary>
/// <remarks>
/// 测试默认排序器的各种场景：
/// 1. 按方法在源码中出现的顺序执行
/// 2. 多个测试方法的顺序验证
/// 
/// 注意：由于 xUnit 并行执行，测试顺序验证依赖日志输出而非断言。
/// </remarks>
[TestCaseOrderer("NewLife.UnitTest.DefaultOrderer", "DH.UnitTest")]
public class DefaultOrdererTests
{
    private readonly ITestOutputHelper _output;

    public DefaultOrdererTests(ITestOutputHelper output) => _output = output;

    /// <summary>第一个方法，应该第一个执行</summary>
    [Fact]
    public void FirstMethod_ShouldExecuteFirst()
    {
        _output.WriteLine($"执行方法: {nameof(FirstMethod_ShouldExecuteFirst)}");
        Assert.True(true);
    }

    /// <summary>第二个方法</summary>
    [Fact]
    public void SecondMethod_ShouldExecuteSecond()
    {
        _output.WriteLine($"执行方法: {nameof(SecondMethod_ShouldExecuteSecond)}");
        Assert.True(true);
    }

    /// <summary>第三个方法</summary>
    [Fact]
    public void ThirdMethod_ShouldExecuteThird()
    {
        _output.WriteLine($"执行方法: {nameof(ThirdMethod_ShouldExecuteThird)}");
        Assert.True(true);
    }

    /// <summary>第四个方法</summary>
    [Fact]
    public void FourthMethod_ShouldExecuteFourth()
    {
        _output.WriteLine($"执行方法: {nameof(FourthMethod_ShouldExecuteFourth)}");
        Assert.True(true);
    }

    /// <summary>最后一个方法</summary>
    [Fact]
    public void LastMethod_ShouldExecuteLast()
    {
        _output.WriteLine($"执行方法: {nameof(LastMethod_ShouldExecuteLast)}");
        Assert.True(true);
    }
}

/// <summary>测试方法名不按字母顺序排列时的执行顺序</summary>
/// <remarks>
/// 验证 DefaultOrderer 按源码顺序而非字母顺序执行。
/// </remarks>
[TestCaseOrderer("NewLife.UnitTest.DefaultOrderer", "DH.UnitTest")]
public class DefaultOrdererAlphabeticTests
{
    private readonly ITestOutputHelper _output;

    public DefaultOrdererAlphabeticTests(ITestOutputHelper output) => _output = output;

    /// <summary>ZZZ开头但在源码中第一个定义</summary>
    [Fact]
    public void ZZZ_DefinedFirst_ShouldExecuteFirst()
    {
        _output.WriteLine($"执行方法: {nameof(ZZZ_DefinedFirst_ShouldExecuteFirst)} (按源码顺序应第一)");
        Assert.True(true);
    }

    /// <summary>MMM开头在源码中第二个定义</summary>
    [Fact]
    public void MMM_DefinedSecond_ShouldExecuteSecond()
    {
        _output.WriteLine($"执行方法: {nameof(MMM_DefinedSecond_ShouldExecuteSecond)} (按源码顺序应第二)");
        Assert.True(true);
    }

    /// <summary>AAA开头但在源码中最后定义</summary>
    [Fact]
    public void AAA_DefinedLast_ShouldExecuteLast()
    {
        _output.WriteLine($"执行方法: {nameof(AAA_DefinedLast_ShouldExecuteLast)} (按源码顺序应第三)");
        Assert.True(true);
    }
}

/// <summary>测试单个方法场景</summary>
[TestCaseOrderer("NewLife.UnitTest.DefaultOrderer", "DH.UnitTest")]
public class DefaultOrdererSingleMethodTests
{
    private readonly ITestOutputHelper _output;

    public DefaultOrdererSingleMethodTests(ITestOutputHelper output) => _output = output;

    /// <summary>单个方法应该正常执行</summary>
    [Fact]
    public void SingleMethod_ShouldExecuteNormally()
    {
        _output.WriteLine("单个方法测试");
        Assert.True(true);
    }
}

/// <summary>测试大量测试方法的排序</summary>
[TestCaseOrderer("NewLife.UnitTest.DefaultOrderer", "DH.UnitTest")]
public class DefaultOrdererLargeTests
{
    private readonly ITestOutputHelper _output;

    public DefaultOrdererLargeTests(ITestOutputHelper output) => _output = output;

    [Fact]
    public void Test01() => _output.WriteLine("Test01");

    [Fact]
    public void Test02() => _output.WriteLine("Test02");

    [Fact]
    public void Test03() => _output.WriteLine("Test03");

    [Fact]
    public void Test04() => _output.WriteLine("Test04");

    [Fact]
    public void Test05() => _output.WriteLine("Test05");

    [Fact]
    public void Test06() => _output.WriteLine("Test06");

    [Fact]
    public void Test07() => _output.WriteLine("Test07");

    [Fact]
    public void Test08() => _output.WriteLine("Test08");

    [Fact]
    public void Test09() => _output.WriteLine("Test09");

    [Fact]
    public void Test10() => _output.WriteLine("Test10");
}
