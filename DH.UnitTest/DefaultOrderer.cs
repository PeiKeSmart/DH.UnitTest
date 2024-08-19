using System;
using System.Collections.Generic;
using System.Linq;
using NewLife.Log;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace NewLife.UnitTest;

/// <summary>默认顺序测试排序器</summary>
/// <remarks>
/// 默认排序（按方法在类内出现的顺序）：
/// [TestCaseOrderer("NewLife.UnitTest.DefaultOrderer", "NewLife.UnitTest")]
/// </remarks>
public class DefaultOrderer : ITestCaseOrderer
{
    private readonly IMessageSink _messageSink;

    /// <summary>实例化默认顺序测试排序器</summary>
    /// <param name="messageSink"></param>
    public DefaultOrderer(IMessageSink messageSink) => _messageSink = messageSink;

    /// <summary>对测试用例进行排序</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="testCases"></param>
    /// <returns></returns>
    public IEnumerable<T> OrderTestCases<T>(IEnumerable<T> testCases) where T : ITestCase
    {
        // 所有测试用例
        var ts = testCases.ToList();

        // 借助反射，拿到方法列表，此时为方法在源码中出现顺序。
        // 需要注意，方法对象不唯一，需要拼接名称字符串
        var types = new List<String>();
        var methods = new List<String>();
        foreach (var item in ts)
        {
            var cls = item.TestMethod?.TestClass?.Class;
            if (cls != null && !types.Contains(cls.Name))
            {
                types.Add(cls.Name);
                foreach (var elm in cls.GetMethods(false))
                {
                    var key = $"{cls.Name}-{elm.Name}";
                    if (!methods.Contains(key)) methods.Add(key);
                }
            }
        }

        XTrace.WriteLine("使用[DefaultOrderer/默认方法顺序]测试: {0}, 用例：{1}", types.Join(), ts.Count);

        var dic = new SortedDictionary<Int32, List<T>>();

        // 从源码行获取优先级
        foreach (var testCase in ts)
        {
            var priority = testCase.SourceInformation?.LineNumber ?? 0;
            if (!dic.TryGetValue(priority, out var list)) list = dic[priority] = [];

            list.Add(testCase);
        }

        // 遇到无效源码信息时，从方法出现顺序为准
        if (dic.Count == 1 && dic.First().Key == 0)
        {
            // 按照方法顺序，逐个返回测试用例，并从列表中删除
            foreach (var method in methods)
            {
                for (var i = 0; i < ts.Count; i++)
                {
                    var item = ts[i];
                    var key = $"{item.TestMethod.TestClass.Class.Name}-{item.TestMethod.Method.Name}";
                    if (method == key)
                    {
                        XTrace.WriteLine(key);

                        yield return item;
                        ts.RemoveAt(i);
                        i--;

                        break;
                    }
                }
            }

            // 如果列表中还有测试用例，可能上述逻辑有缺陷，仍然返回
            foreach (var item in ts)
            {
                var key = $"{item.TestMethod.TestClass.Class.Name}-{item.TestMethod.Method.Name}";
                XTrace.WriteLine(key);

                yield return item;
            }

            yield break;
        }

        // 排序字典按优先级带有顺序，每个优先级内部按类名、方法名排序
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
}