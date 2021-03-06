using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using WonderTools.JsonSectionReader;
using NUnit.Framework;

namespace WonderTools.JsonSectionReaderTests
{
    [TestFixture]
    public class TableDataTests
    {
        [TestCaseSource(nameof(GetTestCaseData))]
        public void Test(string searchPath , Type[] tableTypes, List<List<object>> expected)
        {
            var reader = new JSectionReader();
            var actual = reader.Read("TableDataTests.json").GetSection(searchPath)
                .GetTable(tableTypes);
            actual.Should().BeEquivalentTo(expected);
        }

        public static List<object> List(params object[] objs)
        {
            return objs.ToList();
        }

        public static IEnumerable<TestCaseData> GetTestCaseData()
        {
            yield return GetTestCase(BasicData);
            yield return GetTestCase(NullableValueInData);
            yield return GetTestCase(DateInTable);
            yield return GetTestCase(EnumInTable);
            yield return GetTestCase(ListInTable);
            yield return GetTestCase(ObjectInTable);
            yield return GetTestCase(ObjectListInTable);
        }

        private static TestCaseData GetTestCase(Func<IEnumerable<object>> method)
        {
            var objects = method.Invoke().ToList();
            var path = (string)objects[0];
            var types = (Type[]) objects[1];
            var expected = (List<List<object>>) objects[2];
            return new TestCaseData(path, types, expected);
        }

        private static IEnumerable<object> BasicData()
        {
            yield return "basicData";
            yield return new[] {typeof(int), typeof(string), typeof(int)};
            yield return new List<List<object>>
            {
                List(1, "string1", 322),
                List(2, "string2", 32433),
                List(3, "string3", 32433),
                List(4, "string4", 32433),
                List(5, "string5", 32433),
                List(6, "string6", 32433),
                List(7, "string7", 32433)
            };
        }

        private static IEnumerable<object> NullableValueInData()
        {
            yield return "nullableValueInData";
            yield return new[] { typeof(int), typeof(string), typeof(int?) };
            yield return new List<List<object>>
            {
                List(1, "string1", 322),
                List(2, "string2", null),
                List(3, "string3", 32433)
            };
        }

        private static IEnumerable<object> DateInTable()
        {
            yield return "dateValueInTable";
            yield return new[] { typeof(DateTime), typeof(string), typeof(int) };
            yield return new List<List<object>>
            {
                List(new DateTime(2017, 3, 13), "string1", 322),
                List(new DateTime(2017, 3, 15), "string2", 834),
                List(new DateTime(2017, 3, 14), "string3", 32433)
            };
        }


        private static IEnumerable<object> EnumInTable()
        {
            yield return "enumValueInTable";
            yield return new[] { typeof(DayOfTheWeek), typeof(string), typeof(int) };
            yield return new List<List<object>>
            {
                List(DayOfTheWeek.Monday, "string1", 322),
                List(DayOfTheWeek.Thursday, "string2", 834),
                List(DayOfTheWeek.Sunday, "string3", 32433)
            };
        }

        private static IEnumerable<object> ListInTable()
        {
            yield return "listInTable";
            yield return new[] { typeof(List<int>), typeof(string), typeof(int) };
            yield return new List<List<object>>
            {
                List(List(12,3,4,5), "string1", 322),
                List(List(), "string2", 834),
                List(List(4,5), "string3", 32433)
            };
        }

        public enum DayOfTheWeek
        {
            Monday,
            Tuesday,
            Wednesday,
            Thursday,
            Friday,
            Saturday,
            Sunday
        }

        private static IEnumerable<object> ObjectInTable()
        {
            yield return "objectInTable";
            yield return new[] { typeof(Person), typeof(string), typeof(int) };
            yield return new List<List<object>>
            {
                List(Per("John",39), "string1", 322),
                List(Per("Eric",22), "string2", 834),
                List(Per("Mark",37), "string3", 32433)
            };
        }

        private static IEnumerable<object> ObjectListInTable()
        {
            yield return "objectListInTable";
            yield return new[] { typeof(List<Person>), typeof(string), typeof(int) };
            yield return new List<List<object>>
            {
                List(List(Per("John",39), Per("Eric",22)), "string1", 322),
                List(List(),                               "string2", 834),
                List(List(Per("Mark",37)),                 "string3", 32433)
            };
        }

        public class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        public static Person Per(string name, int age)
        {
            return new Person() {Name = name, Age = age};
        }
    }
}