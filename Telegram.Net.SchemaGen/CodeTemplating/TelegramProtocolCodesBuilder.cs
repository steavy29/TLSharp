using System.Collections.Generic;

namespace Telegram.Net.SchemaGen.CodeTemplating
{
    public class TelegramProtocolCodesBuilder : CodeBuilder
    {
        private readonly string CodeDeclarationsCursor = "CodeDeclarationsCursor";

        private readonly List<(string, string)> types = new List<(string, string)>();

        public TelegramProtocolCodesBuilder(CodeTemplateInstance codeTemplateInstance)
            : base(codeTemplateInstance)
        {
        }

        public override void Build()
        {
            var codeDeclarationsLines = new List<string>();
            for (var i = 0; i < types.Count; i++)
            {
                var declLine = $"{{{types[i].Item1}, typeof ({types[i].Item2})}},";
                if (i == types.Count - 1)
                {
                    declLine = declLine.TrimEnd(',');
                }

                codeDeclarationsLines.Add(declLine);
            }

            CodeTemplate.Replace(CodeDeclarationsCursor, codeDeclarationsLines);
        }

        public void RegisterType(string code, string typeName)
        {
            types.Add((code, typeName));
        }
    }
}