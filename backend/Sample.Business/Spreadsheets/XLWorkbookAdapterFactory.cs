using ClosedXML.Excel;

namespace BE.LocalAccountabilitySystem.Business.Spreadsheets
{
    /// <summary>
    /// Encapsulate the creation of <see cref="IXLWorkbook"/> for unit testing purposes.
    /// 
    /// Since loading is tied to the constructor, we have to utilize a factory.
    /// </summary>
    public interface IXLWorkbookAdapterFactory
    {
        /// <summary>
        /// Initialize a new excel workbook given some file stream
        /// </summary>
        /// <param name="fileContent"></param>
        /// <returns></returns>
        IXLWorkbookAdapter Init(Stream fileContent);
    }

    public class XLWorkbookAdapterFactory : IXLWorkbookAdapterFactory
    {
        public XLWorkbookAdapterFactory() { }

        public IXLWorkbookAdapter Init(Stream fileContent)
        {
            return new XLWorkbookAdapter(new XLWorkbook(fileContent));
        }
    }
}
