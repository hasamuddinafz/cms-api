namespace NexusCms.Application.Common.Interfaces;

public interface IExportService
{
    byte[] ExportCsv<T>(IEnumerable<T> records);
    byte[] ExportExcel<T>(IEnumerable<T> records, string sheetName = "Sheet1");
}
