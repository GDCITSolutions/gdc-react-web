using BE.LocalAccountabilitySystem.Common;
using BE.LocalAccountabilitySystem.Common.Enum;
using BE.LocalAccountabilitySystem.Common.Util;
using BE.LocalAccountabilitySystem.Entities.Database;
using BE.LocalAccountabilitySystem.Entities.Request;
using BE.LocalAccountabilitySystem.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BE.LocalAccountabilitySystem.Business.Spreadsheets
{
    /// <summary>
    /// Handle parsing of user csv/excel for bulk insertion
    /// </summary>
    public interface IUserSpreadsheetParser
    {
        /// <summary>
        /// Process an in-memory .xlsx file to turn it into a collection of user requests
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<IList<UserRequest>> ParseUserImport(IFormFile request);
    }

    public class UserSpreadsheetParser : IUserSpreadsheetParser
    {
        private readonly IXLWorkbookAdapterFactory _xlsxFactory;
        private readonly ICsvReaderAdapterFactory _csvFactory;
        private readonly SampleContext _context;

        private readonly IDictionary<string, string> EXCEL_CELL_POSITIONS = new Dictionary<string, string>()
        {
            { "School", "A" },
            { "FirstName", "B" },
            { "LastName", "C" },
            { "Email", "D" },
            { "Status", "E" },
            { "Roles", "F" },
            { "StudentIds", "G" }
        };

        public UserSpreadsheetParser(IXLWorkbookAdapterFactory xlsxFactory, SampleContext context, ICsvReaderAdapterFactory csvFactory) 
        {
            Util.Guard.ArgumentsAreNotNull(xlsxFactory, context);

            _xlsxFactory = xlsxFactory;
            _csvFactory = csvFactory;
            _context = context;
        }

        /// <summary>
        /// Validate that some parsed user is well formed and has its required fields
        /// </summary>
        /// <param name="user"></param>
        /// <param name="rowId"></param>
        /// <exception cref="BulkImportException"></exception>
        private string ValidateUser(UserRequest user, int rowId) 
        {
            if (string.IsNullOrEmpty(user.EmailAddress) ||
                string.IsNullOrEmpty(user.FirstName) ||
                string.IsNullOrEmpty(user.LastName) ||
                user.SystemStatusId == 0 ||
                user.RoleIds.Count <= 0)
                return $"User at row {rowId} is not properly formed or is missing required values";

            return null;
        }

        /// <summary>
        /// Given an .xlsx file, parse and map it to a collection of <see cref="UserRequest"/>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="roles"></param>
        /// <param name="statuses"></param>
        /// <param name="schools"></param>
        /// <returns></returns>
        /// <exception cref="BulkImportException"></exception>
        private IList<UserRequest> ParseExcel(IFormFile request, IList<Role> roles, IList<SystemStatus> statuses) 
        {
            var users = new List<UserRequest>();
            var badlyFormedRows = new List<string>();
            using var workbook = _xlsxFactory.Init(request.OpenReadStream());

            try
            {
                var worksheet = workbook.GetFirstSheet();

                int i = 2;
                while (!worksheet.Row(i).IsEmpty())
                {
                    var row = worksheet.Row(i);

                    var schoolField = row.Cell(EXCEL_CELL_POSITIONS["School"]).GetString();
                    var firstNameField = row.Cell(EXCEL_CELL_POSITIONS["FirstName"]).GetString();
                    var lastNameField = row.Cell(EXCEL_CELL_POSITIONS["LastName"]).GetString();
                    var emailField = row.Cell(EXCEL_CELL_POSITIONS["Email"]).GetString();
                    var statusField = row.Cell(EXCEL_CELL_POSITIONS["Status"]).GetString();
                    var rolesField = row.Cell(EXCEL_CELL_POSITIONS["Roles"]).GetString();

                    var splitRoles = rolesField.Split(",").Select(x => x.Trim()).ToList();
  
                    var roleIds = roles
                        .Where(x => splitRoles.Contains(x.Value))
                        .Select(x => x.Id)
                        .ToList();
                    var statusId = statuses.FirstOrDefault(x => x.Value == statusField)?.Id ?? 0;

                    var user = new UserRequest()
                    {
                        EmailAddress = emailField,
                        FirstName = firstNameField,
                        LastName = lastNameField,
                        RoleIds = roleIds,
                        SystemStatusId = statusId
                    };

                    var errorResponse = ValidateUser(user, i);

                    if (errorResponse != null)
                        badlyFormedRows.Add(errorResponse);

                    users.Add(user);

                    i++;
                }
            }
            catch (Exception)
            {
                throw new BulkImportException("Failed to parse bulk import users");
            }

            if (badlyFormedRows.Count > 0)
                throw new BulkImportException("Bulk import failed for multiple reasons", badlyFormedRows);

            return users;
        }

        /// <summary>
        /// Given an .csv file, parse and map it to a collection of <see cref="UserRequest"/>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="roles"></param>
        /// <param name="statuses"></param>
        /// <param name="schools"></param>
        /// <returns></returns>
        /// <exception cref="BulkImportException"></exception>
        private IList<UserRequest> ParseCsv(IFormFile request, IList<Role> roles, IList<SystemStatus> statuses)
        {
            var users = new List<UserRequest>();
            var badlyFormedRows = new List<string>();

            using var csv = _csvFactory.Build(request);
            var records = csv.GetRecords<UserCsvRow>().ToList();

            try
            {
                for (int i = 0; i < records.Count; i++) 
                {
                    var csvRow = records[i];
                    var splitRoles = csvRow.Roles.Split(",").Select(x => x.Trim()).ToList();
                    var splitStudentIds = csvRow.StudentIds.Split(",").Select(x => x.Trim()).ToList();

                    var roleIds = roles
                        .Where(x => splitRoles.Contains(x.Value))
                        .Select(x => x.Id)
                        .ToList();
                    var statusId = statuses.FirstOrDefault(x => x.Value == csvRow.Status).Id;

                    var user = new UserRequest()
                    {
                        EmailAddress = csvRow.EmailAddress,
                        FirstName = csvRow.FirstName,
                        LastName = csvRow.LastName,
                        RoleIds = roleIds,
                        SystemStatusId = statusId
                    };

                    var errorResponse = ValidateUser(user, i);

                    if (errorResponse != null)
                        badlyFormedRows.Add(errorResponse);

                    users.Add(user);
                }
            }
            catch (Exception ex)
            {
                throw new BulkImportException("Failed to parse bulk import users", ex);
            }

            if (badlyFormedRows.Count > 0)
                throw new BulkImportException("Bulk import failed for multiple reasons", badlyFormedRows);

            return users;
        }


        public async Task<IList<UserRequest>> ParseUserImport(IFormFile request) 
        {
            bool isXlsx = request.FileName.Contains(".xlsx");
            bool isCsv = request.FileName.Contains(".csv");

            if (!isXlsx && !isCsv)
                throw new BulkImportException("Invalid filetype provided");

            var roles = await _context.Role
                .Where(x => x.SystemStatusId == SystemStatusEnum.Active.AsInt())
                .ToListAsync();

            var statuses = await _context.SystemStatus
                .ToListAsync();

            if (isXlsx)
                return ParseExcel(request, roles, statuses);
            else
                return ParseCsv(request, roles, statuses);
        }
    }

    internal class UserCsvRow 
    {
        [CsvHelper.Configuration.Attributes.Name("School")]
        public string School { get; set; }

        [CsvHelper.Configuration.Attributes.Name("First Name")]
        public string FirstName { get; set; }

        [CsvHelper.Configuration.Attributes.Name("Last Name")]
        public string LastName { get; set; }

        [CsvHelper.Configuration.Attributes.Name("Email")]
        public string EmailAddress { get; set; }

        [CsvHelper.Configuration.Attributes.Name("Status")]
        public string Status { get; set; }

        [CsvHelper.Configuration.Attributes.Name("System Role(s)")]
        public string Roles { get; set; }

        [CsvHelper.Configuration.Attributes.Name("Student ID")]
        public string StudentIds { get; set; }
    }
}
