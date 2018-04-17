using System;
using System.Collections.Generic;
using System.Linq;

namespace Telegram.Net.SchemaGen.Generator
{
    public class TextTemplate
    {
        private readonly List<string> linedTemplate;

        public TextTemplate(string text)
        {
            linedTemplate = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
        }

        private string ReplaceTemplate(string cursorName) => $"##{cursorName}##";

        public void Apply(string cursorName, string value)
        {
            for (var i = 0; i < linedTemplate.Count; i++)
            {
                linedTemplate[i] = linedTemplate[i].Replace(ReplaceTemplate(cursorName), value);
            }
        }

        public void Apply(string cursorName, List<string> multilineValue)
        {
            if (multilineValue.Count == 0)
            {
                return;
            }

            var (lineIndex, position) = (-1, -1);
            for (int i = 0; i < linedTemplate.Count; i++)
            {
                position = linedTemplate[i].IndexOf(ReplaceTemplate(cursorName), StringComparison.InvariantCulture);
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
            linedTemplate[lineIndex] = linedTemplate[lineIndex].Replace(ReplaceTemplate(cursorName), multilineValue.First());

            // Insert rest of the lines with correct spacing
            foreach (var lineToInsert in multilineValue.Skip(1))
            {
                var newLine = $"{spacing}{lineToInsert}";
                linedTemplate.Insert(lineIndex, newLine);
            }
        }

        public override string ToString()
        {
            return string.Concat(linedTemplate);
        }
    }
}