using BE.LocalAccountabilitySystem.Common.Util;
using CsvHelper;

namespace BE.LocalAccountabilitySystem.Business.Spreadsheets
{
    /// <summary>
    /// Excapsulate <see cref="CsvReader"/> for unit testing purposes
    /// </summary>
    public interface ICsvReaderAdapter : IDisposable
    {
        /// <summary>
        /// Get records and parse them into some type of record
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> GetRecords<T>();
    }

    public class CsvReaderAdapter : ICsvReaderAdapter
    {
        private readonly CsvReader _reader;
        private readonly StreamReader _stream;

        public CsvReaderAdapter(CsvReader reader, StreamReader stream) 
        {
            Util.Guard.ArgumentsAreNotNull(reader, stream);

            _reader = reader;
            _stream = stream;
        }

        public IEnumerable<T> GetRecords<T>() 
        {
            return _reader.GetRecords<T>();
        }

        public void Dispose() 
        {
            _stream.Dispose();
            _reader.Dispose();
        }
    }
}
