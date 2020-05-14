using System;
using System.Collections.Generic;
using System.IO;

public static class BlaiseStringExt
{
    public static string Unindent(this string source, bool trimBlanks)
    {
        var lines = source.ToList();
        var minIndent = int.MaxValue;
        for (var i = 0; i < lines.Count; i++)
        {
            var line = lines[i];
            if (line.Trim().Length == 0)
            {
                lines[i] = string.Empty;
                continue;
            }
            var indent = line.Length - line.TrimStart().Length;
            minIndent = Math.Min(minIndent, indent);
        }
        for (var i = 0; i < lines.Count; i++)
        {
            if (lines[i].Length == 0)
                continue;
            lines[i] = lines[i].Substring(minIndent);
        }
        if (trimBlanks)
        {
            while (lines.Count > 0 && lines[0].Length == 0)
            {
                lines.RemoveAt(0);
            }
            while (lines.Count > 0 && lines[lines.Count - 1].Length == 0)
            {
                lines.RemoveAt(lines.Count - 1);
            }
        }
        return string.Join(Environment.NewLine, lines);
    }

    public static List<string> ToList(this string source)
    {
        var lines = new List<string>();
        using (var reader = new StringReader(source))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                lines.Add(line);
            }
        }
        return lines;
    }
}