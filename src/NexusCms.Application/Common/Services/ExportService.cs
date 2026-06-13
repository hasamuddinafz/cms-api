using System.Globalization;
using ClosedXML.Excel;
using CsvHelper;
using NexusCms.Application.Common.Interfaces;

namespace NexusCms.Application.Common.Services;

public class ExportService : IExportService
{
    public byte[] ExportCsv<T>(IEnumerable<T> records)
    {
        using var ms = new MemoryStream();
        using var writer = new StreamWriter(ms, leaveOpen: true);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.WriteRecords(records);
        writer.Flush();
        return ms.ToArray();
    }

    public byte[] ExportExcel<T>(IEnumerable<T> records, string sheetName = "Sheet1")
    {
        using var workbook = new XLWorkbook();
        var ws = workbook.Worksheets.Add(sheetName);

        var list = records.ToList();
        if (list.Count == 0)
        {
            using var empty = new MemoryStream();
            workbook.SaveAs(empty);
            return empty.ToArray();
        }

        var properties = typeof(T).GetProperties();

        // Header row
        for (var i = 0; i < properties.Length; i++)
            ws.Cell(1, i + 1).Value = properties[i].Name;

        // Data rows
        for (var row = 0; row < list.Count; row++)
        {
            for (var col = 0; col < properties.Length; col++)
            {
                var value = properties[col].GetValue(list[row]);
                ws.Cell(row + 2, col + 1).Value = value switch
                {
                    null           => XLCellValue.FromObject(string.Empty),
                    bool b         => b,
                    int n          => n,
                    long l         => (double)l,
                    double d       => d,
                    decimal dec    => (double)dec,
                    DateTime dt    => dt,
                    _              => value.ToString() ?? string.Empty
                };
            }
        }

        ws.RangeUsed()?.SetAutoFilter();
        ws.Columns().AdjustToContents();

        using var ms = new MemoryStream();
        workbook.SaveAs(ms);
        return ms.ToArray();
    }
}
