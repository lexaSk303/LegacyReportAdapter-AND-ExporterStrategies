namespace LegacyReportAdapter;

public sealed class LegacyReportAdapter : IReportExporter
{
    private readonly LegacyReportPrinter _printer;

    public LegacyReportAdapter(LegacyReportPrinter printer)
    {
        // TODO 1: Защищаем обязательную зависимость от null
        _printer = printer ?? throw new ArgumentNullException(nameof(printer), "Принтер не может быть null");
    }

    public void Export(ReportDocument document)
    {
        // TODO 2: Проверяем входной документ на null
        if (document is null)
        {
            throw new ArgumentNullException(nameof(document), "Документ не может быть null");
        }

        // TODO 3 и 4: Преобразуем документ
        string safeTitle = document.Title ?? string.Empty;
        string safeBody = document.Body ?? string.Empty;

        _printer.PrintReport(safeTitle, safeBody);
    }
}