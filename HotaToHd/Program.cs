var rangesToRemove = new List<Range>()
{
    new Range(0, 14),
    new Range(18, 27),
    new Range(57, 58),
    new Range(82, 83),
    new Range(96, 97),
    new Range(107, 123),
    new Range(129, 132),
};

if (args.Length != 2)
{
    Console.WriteLine("Usage: HotaToHd.exe <source> <target>");
    return;
}

var columnsToRemove = CombineRanges(rangesToRemove);

var sourceDirectory = args[0];
var targetDirectory = args[1];

var templates = Directory.EnumerateFiles(sourceDirectory, "*.h3t");

foreach (var template in templates)
{
    var lines = File.ReadAllLines(template);
    var trimmedLines = RemoveHotaColumns(columnsToRemove, lines);

    var templateName = Path.GetFileNameWithoutExtension(template);
    var targetTemplateDir = Path.Combine(targetDirectory, templateName);
    var targetFile = Path.Combine(targetTemplateDir, "rmg.txt");

    if (!Directory.Exists(targetTemplateDir))
    {
        Directory.CreateDirectory(targetTemplateDir);
    }

    File.WriteAllLines(targetFile, trimmedLines);
}

static IEnumerable<string> RemoveHotaColumns(ISet<int> columnsToRemove, string[] lines)
{
    var convertedLines = new List<string>();
    foreach (var line in lines)
    {
        var columns = line.Split('\t').ToList();
        for (int i = columns.Count - 1; i >= 0; i--)
        {
            if (columnsToRemove.Contains(i))
            {
                columns.RemoveAt(i);
            }
        }

        var result = string.Join('\t', columns);

        convertedLines.Add(result);
    }

    return convertedLines;
}

ISet<int> CombineRanges(IEnumerable<Range> ranges)
{
    var results = new HashSet<int>();
    foreach (var range in ranges)
    {
        var count = range.Max - range.Min + 1;
        var numbers = Enumerable.Range(range.Min, count);

        foreach (var number in numbers)
        {
            results.Add(number);
        }
    }
    return results;
}

public record struct Range(int Min, int Max);