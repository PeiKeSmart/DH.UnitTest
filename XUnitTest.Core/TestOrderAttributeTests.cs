using System;
using System.Linq;

using NewLife.UnitTest;

using Xunit;

namespace XUnitTest;

/// <summary>TestOrderAttribute 特性测试</summary>
public class TestOrderAttributeTests
{
    /// <summary>测试正数优先级</summary>
    [Fact]
    public void Constructor_WithPositivePriority_ShouldSetPriority()
    {
        var attr = new TestOrderAttribute(10);

        Assert.Equal(10, attr.Priority);
    }

    /// <summary>测试零优先级</summary>
    [Fact]
    public void Constructor_WithZeroPriority_ShouldSetPriority()
    {
        var attr = new TestOrderAttribute(0);

        Assert.Equal(0, attr.Priority);
    }

    /// <summary>测试负数优先级</summary>
    [Fact]
    public void Constructor_WithNegativePriority_ShouldSetPriority()
    {
        var attr = new TestOrderAttribute(-5);

        Assert.Equal(-5, attr.Priority);
    }

    /// <summary>测试最大整数优先级</summary>
    [Fact]
    public void Constructor_WithMaxIntPriority_ShouldSetPriority()
    {
        var attr = new TestOrderAttribute(Int32.MaxValue);

        Assert.Equal(Int32.MaxValue, attr.Priority);
    }

    /// <summary>测试最小整数优先级</summary>
    [Fact]
    public void Constructor_WithMinIntPriority_ShouldSetPriority()
    {
        var attr = new TestOrderAttribute(Int32.MinValue);

        Assert.Equal(Int32.MinValue, attr.Priority);
    }

    /// <summary>测试特性只能应用于方法</summary>
    [Fact]
    public void Attribute_ShouldOnlyApplyToMethods()
    {
        var attrUsage = typeof(TestOrderAttribute)
            .GetCustomAttributes(typeof(AttributeUsageAttribute), false)
            .Cast<AttributeUsageAttribute>()
            .FirstOrDefault();

        Assert.NotNull(attrUsage);
        Assert.Equal(AttributeTargets.Method, attrUsage.ValidOn);
        Assert.False(attrUsage.AllowMultiple);
    }

    /// <summary>测试优先级属性只读</summary>
    [Fact]
    public void Priority_ShouldBeReadOnly()
    {
        var property = typeof(TestOrderAttribute).GetProperty(nameof(TestOrderAttribute.Priority));

        Assert.NotNull(property);
        Assert.True(property.CanRead);
        Assert.False(property.CanWrite);
    }
}
