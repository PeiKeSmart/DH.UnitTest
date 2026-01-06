using System;
using System.Collections.Generic;
using System.Linq;
using NewLife.Log;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace NewLife.UnitTest;

/// <summary>优先级顺序测试排序器</summary>
/// <remarks>
/// 根据 TestOrderAttribute 特性指定的优先级排序，数值越小越先执行。
/// 
/// 使用方式：
/// [TestCaseOrderer("NewLife.UnitTest.PriorityOrderer", "NewLife.UnitTest")]
/// </remarks>
public class PriorityOrderer : ITestCaseOrderer
{
    #region 方法
    /// <summary>对测试用例进行排序</summary>
    /// <typeparam name="T">测试用例类型</typeparam>
    /// <param name="testCases">测试用例集合</param>
    /// <returns>排序后的测试用例</returns>
    public IEnumerable<T> OrderTestCases<T>(IEnumerable<T> testCases) where T : ITestCase
    {
        var cases = testCases.ToList();
        var types = CollectTypeNames(cases);

        XTrace.WriteLine("使用[PriorityOrderer/优先级顺序]测试: {0}, 用例：{1}", types.Join(), cases.Count);

        // 按优先级分组
        var dic = GroupByPriority(cases);

        // 按优先级顺序返回
        foreach (var item in dic)
        {
            foreach (var testCase in item.Value.OrderBy(x => x.TestMethod.Method.Name))
            {
                var key = $"{item.Key}-{testCase.TestMethod.TestClass.Class.Name}-{testCase.TestMethod.Method.Name}";
                XTrace.WriteLine(key);
                yield return testCase;
            }
        }
    }
    #endregion

    #region 辅助
    /// <summary>收集测试类型名称</summary>
    /// <typeparam name="T">测试用例类型</typeparam>
    /// <param name="cases">测试用例集合</param>
    /// <returns>类型名称列表</returns>
    private static List<String> CollectTypeNames<T>(List<T> cases) where T : ITestCase
    {
        var types = new List<String>();

        foreach (var item in cases)
        {
            var cls = item.TestMethod?.TestClass?.Class;
            if (cls != null && !types.Contains(cls.Name))
            {
                types.Add(cls.Name);
            }
        }

        return types;
    }

    /// <summary>按优先级分组</summary>
    /// <typeparam name="T">测试用例类型</typeparam>
    /// <param name="cases">测试用例集合</param>
    /// <returns>按优先级分组的字典</returns>
    private static SortedDictionary<Int32, List<T>> GroupByPriority<T>(List<T> cases) where T : ITestCase
    {
        var dic = new SortedDictionary<Int32, List<T>>();
        var assemblyName = typeof(TestOrderAttribute).AssemblyQualifiedName!;

        foreach (var testCase in cases)
        {
            var priority = GetPriority(testCase, assemblyName);
            if (!dic.TryGetValue(priority, out var list)) list = dic[priority] = [];

            list.Add(testCase);
        }

        return dic;
    }

    /// <summary>获取测试用例的优先级</summary>
    /// <typeparam name="T">测试用例类型</typeparam>
    /// <param name="testCase">测试用例</param>
    /// <param name="assemblyName">特性程序集名称</param>
    /// <returns>优先级数值</returns>
    private static Int32 GetPriority<T>(T testCase, String assemblyName) where T : ITestCase
    {
        var atts = testCase.TestMethod.Method.GetCustomAttributes(assemblyName);
        foreach (var att in atts)
        {
            var priority = att.GetNamedArgument<Int32>(nameof(TestOrderAttribute.Priority));
            if (priority != 0) return priority;
        }

        return 0;
    }
    #endregion
}