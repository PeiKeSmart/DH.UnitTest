using System;

namespace NewLife.UnitTest;

/// <summary>测试顺序特性。升序排列</summary>
/// <remarks>
/// 用于指定测试用例的执行优先级，数值越小越先执行。
/// 
/// 配合 PriorityOrderer 使用：
/// [TestCaseOrderer("NewLife.UnitTest.PriorityOrderer", "NewLife.UnitTest")]
/// 
/// 默认排序（按方法在类内出现的顺序）：
/// [TestCaseOrderer("NewLife.UnitTest.DefaultOrderer", "NewLife.UnitTest")]
/// </remarks>
/// <remarks>实例化测试顺序特性</remarks>
/// <param name="priority">测试优先级。数值越小越先执行</param>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class TestOrderAttribute(Int32 priority) : Attribute
{
    #region 属性
    /// <summary>测试优先级。数值越小越先执行</summary>
    public Int32 Priority { get; } = priority;
    #endregion
}