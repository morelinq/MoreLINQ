namespace MoreLinq
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;

    static partial class MoreEnumerable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IDataReader ToDataReader<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return new ReflectedDataReader<TSource>(source);
        }

        private class ReflectedDataReader<TSource> : IDataReader
        {
            private readonly IEnumerator<TSource> _enumerator;
            private readonly PropertyInfo[] _properties;
            private readonly FieldInfo[] _fields;
            private readonly string[] _names;
            private readonly Dictionary<string, int> _ordinals;
            private readonly Type[] _types;

            public ReflectedDataReader(IEnumerable<TSource> source)
            {
                _enumerator = source.GetEnumerator();

                _properties = typeof(TSource).GetProperties().
                    Where(x => !x.GetIndexParameters().Any()).
                    ToArray();
                _fields = typeof(TSource).GetFields().
                    Where(x => !x.IsStatic).
                    ToArray();

                _names = _properties.Select(x => x.Name).
                    Concat(_fields.Select(x => x.Name)).
                    ToArray();
                _ordinals = _names
                    .Select((name, index) => new { name, index })
                    .ToDictionary(x => x.name, x => x.index);

                _types = _properties.Select(x => x.PropertyType).
                    Concat(_fields.Select(x => x.FieldType)).
                    ToArray();
            }

            public int FieldCount => _properties.Length + _fields.Length;

            public string GetName(int i) => _names[i];

            public int GetOrdinal(string name) => _ordinals[name];

            public object GetValue(int i)
            {
                if (i < 0 || i >= FieldCount)
                    throw new ArgumentOutOfRangeException(nameof(i));

                var current = _enumerator.Current;
                if (i < _properties.Length)
                {
                    return _properties[i].GetValue(current, null);
                }
                return _fields[i - _properties.Length].GetValue(current);
            }

            public Type GetFieldType(int i) => _types[i];

            public bool Read() => _enumerator.MoveNext();

            #region Unimplemented

            public object this[int i] => throw new NotImplementedException();

            public object this[string name] => throw new NotImplementedException();

            public int Depth => throw new NotImplementedException();

            public bool IsClosed => throw new NotImplementedException();

            public int RecordsAffected => throw new NotImplementedException();

            public void Close()
            {
                throw new NotImplementedException();
            }

            public bool GetBoolean(int i)
            {
                throw new NotImplementedException();
            }

            public byte GetByte(int i)
            {
                throw new NotImplementedException();
            }

            public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
            {
                throw new NotImplementedException();
            }

            public char GetChar(int i)
            {
                throw new NotImplementedException();
            }

            public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
            {
                throw new NotImplementedException();
            }

            public IDataReader GetData(int i)
            {
                throw new NotImplementedException();
            }

            public string GetDataTypeName(int i)
            {
                throw new NotImplementedException();
            }

            public DateTime GetDateTime(int i)
            {
                throw new NotImplementedException();
            }

            public decimal GetDecimal(int i)
            {
                throw new NotImplementedException();
            }

            public double GetDouble(int i)
            {
                throw new NotImplementedException();
            }

            public float GetFloat(int i)
            {
                throw new NotImplementedException();
            }

            public Guid GetGuid(int i)
            {
                throw new NotImplementedException();
            }

            public short GetInt16(int i)
            {
                throw new NotImplementedException();
            }

            public int GetInt32(int i)
            {
                throw new NotImplementedException();
            }

            public long GetInt64(int i)
            {
                throw new NotImplementedException();
            }

            public DataTable GetSchemaTable()
            {
                throw new NotImplementedException();
            }

            public string GetString(int i)
            {
                throw new NotImplementedException();
            }

            public int GetValues(object[] values)
            {
                throw new NotImplementedException();
            }

            public bool IsDBNull(int i)
            {
                throw new NotImplementedException();
            }

            public bool NextResult()
            {
                throw new NotImplementedException();
            }

            #endregion

            #region IDisposable Support
            private bool disposedValue = false; // To detect redundant calls

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        // TODO: dispose managed state (managed objects).
                        _enumerator.Dispose();
                    }

                    // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                    // TODO: set large fields to null.

                    disposedValue = true;
                }
            }

            // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
            // ~ReflectedDataReader() {
            //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            //   Dispose(false);
            // }

            // This code added to correctly implement the disposable pattern.
            public void Dispose()
            {
                // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
                Dispose(true);
                // TODO: uncomment the following line if the finalizer is overridden above.
                // GC.SuppressFinalize(this);
            }
            #endregion
        }
    }
}
