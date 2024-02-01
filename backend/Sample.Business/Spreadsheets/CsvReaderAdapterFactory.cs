using CsvHelper;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace BE.LocalAccountabilitySystem.Business.Spreadsheets
{
    /// <summary>
    /// Excapsulate the creation of <see cref="CsvReader"/> for unit testing purposes
    /// </summary>
    public interface ICsvReaderAdapterFactory
    {
        ICsvReaderAdapter Build(IFormFile file);
    }

    public class CsvReaderAdapterFactory : ICsvReaderAdapterFactory
    {
        public CsvReaderAdapterFactory() { }

        public ICsvReaderAdapter Build(IFormFile file) 
        {
            var reader = new StreamReader(file.OpenReadStream());
            return new CsvReaderAdapter(new CsvReader(reader, CultureInfo.InvariantCulture), reader);
        }
    }
}
