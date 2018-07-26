using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Telegram.Net.SchemaGen.CodeTemplating
{
    public class CodeTemplateInstance
    {
        private readonly List<string> linedTemplate;

        public CodeTemplateInstance(List<string> linedTemplate)
        {
            this.linedTemplate = linedTemplate;
        }

        private string FullCursorStr(string cursorName) => $"##{cursorName}##";

        private bool ExactMatchIgnoreSpaces(string line, string value) => Regex.IsMatch(line, $"^\\s*{value}\\s*$");

        public void RemoveLine(int index, bool checkIndex = false)
        {
            if (checkIndex)
            {
                if (index < 0 || index >= linedTemplate.Count)
                {
                    return;
                }
            }

            linedTemplate.RemoveAt(index);
        }

        public int FindCursorFullLine(string cursorName)
        {
            var fullCursorStr = FullCursorStr(cursorName);
            return linedTemplate.FindIndex(l => ExactMatchIgnoreSpaces(l, fullCursorStr));
        }

        public void RemoveCursorFullLine(string cursorName, bool removeLineAfter = false)
        {
            var lineIndex = FindCursorFullLine(cursorName);
            if (lineIndex < 0)
                return;

            RemoveLine(lineIndex);

            if (removeLineAfter)
            {
                var nextLineIndex = lineIndex;
                RemoveLine(nextLineIndex, true);
            }
        }

        public void Replace(string cursorName, string value)
        {
            for (var i = 0; i < linedTemplate.Count; i++)
            {
                linedTemplate[i] = linedTemplate[i].Replace(FullCursorStr(cursorName), value);
            }
        }

        public void Replace(string cursorName, List<string> multilineValue, EmptyMultilineReplace emptyBehaviour = EmptyMultilineReplace.None)
        {
            if (multilineValue.Count == 0)
            {
                switch (emptyBehaviour)
                {
                    case EmptyMultilineReplace.RemoveCursorLine:
                        RemoveCursorFullLine(cursorName);
                        break;
                    case EmptyMultilineReplace.RemoveCursorLineAndLineOneAfter:
                        RemoveCursorFullLine(cursorName, true);
                        break;
                    case EmptyMultilineReplace.RemoveCursorOnly:
                        Replace(cursorName, string.Empty);
                        break;
                }

                return;
            }

            var (lineIndex, position) = (-1, -1);
            for (int i = 0; i < linedTemplate.Count; i++)
            {
                position = linedTemplate[i].IndexOf(FullCursorStr(cursorName), StringComparison.InvariantCulture);
                if (position > 0)
                {
                    lineIndex = i;
                    break;
                }
            }

            if (lineIndex < 0)
                return;

            var spacing = new string(' ', position);

            // Replace first line
            linedTemplate[lineIndex] = linedTemplate[lineIndex].Replace(FullCursorStr(cursorName), multilineValue.First());

            // Insert rest of the lines with correct spacing
            foreach (var lineToInsert in multilineValue.Skip(1))
            {
                var insertionPoint = ++lineIndex;
                linedTemplate.Insert(insertionPoint, $"{spacing}{lineToInsert}");
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder(linedTemplate.Sum(l => l.Length) + linedTemplate.Count * 2);
            linedTemplate.ForEach(l => builder.AppendLine(l));

            return builder.ToString();
        }
    }
}