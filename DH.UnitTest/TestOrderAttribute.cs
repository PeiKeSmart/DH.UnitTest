using System;

namespace NewLife.UnitTest;

/// <summary>测试顺序。升序</summary>
/// <remarks>
/// 测试用例排序
/// 
/// 方法特性优先级：
/// [TestCaseOrderer("NewLife.UnitTest.PriorityOrderer", "NewLife.UnitTest")]
/// 
/// 默认排序（按方法在类内出现的顺序）：
/// [TestCaseOrderer("NewLife.UnitTest.DefaultOrderer", "NewLife.UnitTest")]
/// </remarks>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class TestOrderAttribute : Attribute
{
    /// <summary>测试顺序。升序</summary>
    public Int32 Priority { get; private set; }

    /// <summary>测试顺序。升序</summary>
    /// <param name="priority"></param>
    public TestOrderAttribute(Int32 priority) => Priority = priority;
}