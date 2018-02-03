namespace MoreLinq.Test
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class ToDataReaderTest
    {
        class TestObject
        {
            public int KeyField;
            public Guid? ANullableGuidField;

            public string AString { get; }
            public decimal? ANullableDecimal { get; }

            public object this[int index]
            {
                get => new object();
                set { }
            }


            public TestObject(int key)
            {
                KeyField = key;
                ANullableGuidField = Guid.NewGuid();

                ANullableDecimal = key / 3;
                AString = "ABCDEFGHIKKLMNOPQRSTUVWXYSZ";
            }
        }

        readonly IReadOnlyCollection<TestObject> _testObjects;

        public ToDataReaderTest()
        {
            _testObjects = Enumerable.Range(0, 3)
                                     .Select(i => new TestObject(i))
                                     .ToArray();
        }

        [Test]
        public void ToDataReaderSchemaInDeclarationOrder()
        {
            var dr = _testObjects.ToDataReader();

            // Assert properties first, then fields, then in declaration order

            Assert.AreEqual("AString", dr.GetName(0));
            Assert.AreEqual(typeof(string), dr.GetFieldType(0));

            Assert.AreEqual("ANullableDecimal", dr.GetName(1));
            Assert.AreEqual(typeof(decimal?), dr.GetFieldType(1));

            Assert.AreEqual("KeyField", dr.GetName(2));
            Assert.AreEqual(typeof(int), dr.GetFieldType(2));

            Assert.AreEqual("ANullableGuidField", dr.GetName(3));
            Assert.AreEqual(typeof(Guid?), dr.GetFieldType(3));

            Assert.AreEqual(4, dr.FieldCount);
        }

        [Test]
        public void ToDataReaderHasValues()
        {
            var dr = _testObjects.ToDataReader();

            var count = 0;
            while (dr.Read())
            {
                var expectedItem = _testObjects.ElementAt(count);

                Assert.AreEqual(expectedItem.AString, dr.GetValue(0));
                Assert.AreEqual(expectedItem.ANullableDecimal, dr.GetValue(1));
                Assert.AreEqual(expectedItem.KeyField, dr.GetValue(2));
                Assert.AreEqual(expectedItem.ANullableGuidField, dr.GetValue(3));
                count++;
            }

            Assert.AreEqual(3, count);
        }

        struct Point
        {
            public static Point Empty = new Point();
            public bool IsEmpty => X == 0 && Y == 0;
            public int X { get; }
            public int Y { get; }
            public Point(int x, int y) : this() { X = x; Y = y; }
        }

        [Test]
        public void ToDataReaderIgnoresStaticMembers()
        {
            var points = new[] { new Point(12, 34) }.ToDataReader();

            Assert.AreEqual(3, points.FieldCount);
        }
    }
}
