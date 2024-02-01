using BE.LocalAccountabilitySystem.Common.Util;
using ClosedXML.Excel;

namespace BE.LocalAccountabilitySystem.Business.Spreadsheets
{
    /// <summary>
    /// Encapsulate <see cref="IXLWorkbook"/> for unit testing purposes
    /// </summary>
    public interface IXLWorkbookAdapter : IDisposable
    {
        /// <summary>
        /// Get the first sheet of an excel file
        /// </summary>
        /// <returns></returns>
        IXLWorksheet GetFirstSheet();
    }

    public class XLWorkbookAdapter : IXLWorkbookAdapter
    {
        private readonly IXLWorkbook _workbook;

        public XLWorkbookAdapter(IXLWorkbook workbook) 
        {
            Util.Guard.ArgumentsAreNotNull(workbook);

            _workbook = workbook;
        }

        public IXLWorksheet GetFirstSheet() 
        {
            return _workbook.Worksheets.FirstOrDefault();
        }

        public void Dispose()
        {
            _workbook.Dispose();
        }
    }
}
