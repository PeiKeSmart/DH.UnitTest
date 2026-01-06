using System;
using System.Collections.Generic;
using System.Linq;
using NewLife.Log;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace NewLife.UnitTest;

/// <summary>默认顺序测试排序器</summary>
/// <remarks>
/// 按方法在源码中出现的顺序执行测试用例。
/// 
/// 使用方式：
/// [TestCaseOrderer("NewLife.UnitTest.DefaultOrderer", "NewLife.UnitTest")]
/// </remarks>
public class DefaultOrderer : ITestCaseOrderer
{
    #region 属性
    private readonly IMessageSink _messageSink;
    #endregion

    #region 构造
    /// <summary>实例化默认顺序测试排序器</summary>
    /// <param name="messageSink">消息接收器</param>
    public DefaultOrderer(IMessageSink messageSink) => _messageSink = messageSink;
    #endregion

    #region 方法
    /// <summary>对测试用例进行排序</summary>
    /// <typeparam name="T">测试用例类型</typeparam>
    /// <param name="testCases">测试用例集合</param>
    /// <returns>排序后的测试用例</returns>
    public IEnumerable<T> OrderTestCases<T>(IEnumerable<T> testCases) where T : ITestCase
    {
        var cases = testCases.ToList();

        // 收集类型和方法信息
        var (types, methods) = CollectTypeAndMethodInfo(cases);

        XTrace.WriteLine("使用[DefaultOrderer/默认方法顺序]测试: {0}, 用例：{1}", types.Join(), cases.Count);

        // 按源码行号分组
        var dic = GroupBySourceLine(cases);

        // 无有效源码信息时，按方法出现顺序返回
        if (dic.Count == 1 && dic.First().Key == 0)
        {
            foreach (var item in OrderByMethodSequence(cases, methods))
            {
                yield return item;
            }
            yield break;
        }

        // 按源码行号顺序返回
        foreach (var item in OrderByLineNumber(dic))
        {
            yield return item;
        }
    }
    #endregion

    #region 辅助
    /// <summary>收集类型和方法信息</summary>
    /// <typeparam name="T">测试用例类型</typeparam>
    /// <param name="cases">测试用例集合</param>
    /// <returns>类型列表和方法列表</returns>
    private static (List<String> types, List<String> methods) CollectTypeAndMethodInfo<T>(List<T> cases) where T : ITestCase
    {
        var types = new List<String>();
        var methods = new List<String>();

        foreach (var item in cases)
        {
            var cls = item.TestMethod?.TestClass?.Class;
            if (cls == null || types.Contains(cls.Name)) continue;

            types.Add(cls.Name);
            foreach (var method in cls.GetMethods(false))
            {
                var key = $"{cls.Name}-{method.Name}";
                if (!methods.Contains(key)) methods.Add(key);
            }
        }

        return (types, methods);
    }

    /// <summary>按源码行号分组</summary>
    /// <typeparam name="T">测试用例类型</typeparam>
    /// <param name="cases">测试用例集合</param>
    /// <returns>按行号分组的字典</returns>
    private static SortedDictionary<Int32, List<T>> GroupBySourceLine<T>(List<T> cases) where T : ITestCase
    {
        var dic = new SortedDictionary<Int32, List<T>>();

        foreach (var testCase in cases)
        {
            var priority = testCase.SourceInformation?.LineNumber ?? 0;
            if (!dic.TryGetValue(priority, out var list)) list = dic[priority] = [];

            list.Add(testCase);
        }

        return dic;
    }

    /// <summary>按方法出现顺序返回测试用例</summary>
    /// <typeparam name="T">测试用例类型</typeparam>
    /// <param name="cases">测试用例集合</param>
    /// <param name="methods">方法顺序列表</param>
    /// <returns>排序后的测试用例</returns>
    private static IEnumerable<T> OrderByMethodSequence<T>(List<T> cases, List<String> methods) where T : ITestCase
    {
        var remaining = new List<T>(cases);

        // 按方法出现顺序逐个返回
        foreach (var method in methods)
        {
            for (var i = 0; i < remaining.Count; i++)
            {
                var item = remaining[i];
                var key = $"{item.TestMethod.TestClass.Class.Name}-{item.TestMethod.Method.Name}";
                if (method == key)
                {
                    XTrace.WriteLine(key);
                    yield return item;
                    remaining.RemoveAt(i);
                    break;
                }
            }
        }

        // 返回剩余的测试用例
        foreach (var item in remaining)
        {
            var key = $"{item.TestMethod.TestClass.Class.Name}-{item.TestMethod.Method.Name}";
            XTrace.WriteLine(key);
            yield return item;
        }
    }

    /// <summary>按行号顺序返回测试用例</summary>
    /// <typeparam name="T">测试用例类型</typeparam>
    /// <param name="dic">按行号分组的字典</param>
    /// <returns>排序后的测试用例</returns>
    private static IEnumerable<T> OrderByLineNumber<T>(SortedDictionary<Int32, List<T>> dic) where T : ITestCase
    {
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
}