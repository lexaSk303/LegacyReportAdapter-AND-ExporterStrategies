using System;
using System.Collections.Generic;
using System.IO;
using LegacyReportAdapter;

var failed = new List<string>();

void Check(string name, bool condition)
{
    Console.WriteLine($"{(condition ? "PASS" : "FAIL")} — {name}");
    if (!condition) failed.Add(name);
}

bool Throws<TException>(Action action) where TException : Exception
{
    try
    {
        action();
        return false;
    }
    catch (TException)
    {
        return true;
    }
    catch
    {
        return false; 
    }
}

var output = new StringWriter();
var original = Console.Out;
try
{
    Console.SetOut(output);
    IReportExporter exporter = new LegacyReportAdapter.LegacyReportAdapter(new LegacyReportPrinter());
    exporter.Export(new ReportDocument("Test title", "Test body"));
}
finally
{
    Console.SetOut(original);
}

var text = output.ToString();
Check("заголовок передан legacy-принтеру", text.Contains("=== Test title ==="));
Check("тело передано legacy-принтеру", text.Contains("Test body"));
Check("сохранён завершающий маркер", text.Contains("=== END OF REPORT ==="));

// TODO 5: Тест пустого заголовка и тела
var outputEmpty = new StringWriter();
var originalEmpty = Console.Out;
try
{
    Console.SetOut(outputEmpty);
    IReportExporter exporterEmpty = new LegacyReportAdapter.LegacyReportAdapter(new LegacyReportPrinter());
    exporterEmpty.Export(new ReportDocument("", "")); 
}
finally
{
    Console.SetOut(originalEmpty);
}
var textEmpty = outputEmpty.ToString();
Check("пустой заголовок обрабатывается без ошибок", textEmpty.Contains("===  ==="));
Check("пустое тело обрабатывается без ошибок", textEmpty.Contains("=== END OF REPORT ==="));

// TODO 6: Тест null-документа
IReportExporter exporterForNullTest = new LegacyReportAdapter.LegacyReportAdapter(new LegacyReportPrinter());
Check("выбрасывает ArgumentNullException при null-документе",
    Throws<ArgumentNullException>(() => exporterForNullTest.Export(null!)));

// TODO 7: Тест null-зависимости в конструкторе
Check("выбрасывает ArgumentNullException при null-принтере в конструкторе",
    Throws<ArgumentNullException>(() => new LegacyReportAdapter.LegacyReportAdapter(null!)));

Console.WriteLine($"\nПровалено проверок: {failed.Count} из 7.");
if (failed.Count == 0)
{
    Console.WriteLine("Все тесты успешно пройдены!");
}


