using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Telegram.Net.Tests.SchemaGenerator
{
    public static class CodeGen
    {
        public class SourceFile
        {
            public readonly List<string> usingStatements = new List<string>();
            public readonly List<Class> classes = new List<Class>();

            public void Write(TextWriter writer)
            {
                foreach (var usingLibrary in usingStatements)
                {
                    writer.WriteLine($"using {usingLibrary}");
                }

                writer.WriteLine("namespace Telegram.Net");
                using (var rootBlock = Block.CreateRootBlock(writer))
                {
                    foreach (var classItem in classes)
                    {
                        Write(classItem, rootBlock);
                    }
                }
            }

            private static void Write(Class classItem, Block container)
            {
                container.WriteLine($"{classItem.accessType}{(classItem.isStatic ? " static" : "")} class {classItem.name}{(classItem.baseTypes.Count > 0 ? " : " + string.Join(", ", classItem.baseTypes) : "")}");
                using (var classBlock = container.CreateChild())
                {
                    foreach (var field in classItem.fields)
                    {
                        classBlock.WriteLine($"{field.accessType} {(field.isReadonly ? "readonly" : "")} {field.type} {field.name};");
                    }

                    foreach (var method in classItem.methods)
                    {
                        Write(method, classBlock);
                    }
                }
            }

            private static void Write(Method methodItem, Block container)
            {
                // Signature
                container.WriteLine("");
                var signature = "";
                signature += methodItem.accessType.ToString();
                if (methodItem.isOverride)
                    signature += " override";
                else if (methodItem.isVirtual)
                    signature += " virtual";

                if (methodItem.isVoid)
                    signature += " void";
                else
                {
                    signature += " " + methodItem.returnType;
                }

                signature += " " + methodItem.name;
                signature += "(";
                signature += string.Join(", ", methodItem.parameters.Select(p => p.type + " " + p.name));
                signature += ")";

                container.WriteLine(signature);

                // Body
                using (var bodyContainer = container.CreateChild())
                {

                }
            }

            class Block : IDisposable
            {
                private readonly TextWriter writer;
                private readonly bool openBrace;
                private readonly int bodyIdentationLevel;

                private Block(TextWriter writer, int identation, bool openBrace)
                {
                    this.writer = writer;
                    this.openBrace = openBrace;
                    this.bodyIdentationLevel = identation + 1;
                    if (openBrace)
                        WriteLineImpl("{", bodyIdentationLevel - 1);
                }

                public static Block CreateRootBlock(TextWriter writer, bool openBrace = true)
                {
                    return new Block(writer, 0, openBrace);
                }

                public Block CreateChild(bool openBraceInChild = true)
                {
                    return new Block(writer, bodyIdentationLevel, openBraceInChild);
                }

                public void WriteAppend(string text)
                {
                    writer.Write(text);
                }

                public void WriteLine(string line)
                {
                    WriteLineImpl(line, bodyIdentationLevel);
                }

                private void WriteLineImpl(string line, int identationLevel)
                {
                    var identation = string.Concat(Enumerable.Repeat('\t', identationLevel));
                    writer.WriteLine(identation + line);
                }

                public void Dispose()
                {
                    if (openBrace)
                    {
                        WriteLineImpl("}", bodyIdentationLevel - 1);
                    }
                }
            }
        }

        public class Class
        {
            public readonly List<Field> fields = new List<Field>();
            public readonly List<Property> properties = new List<Property>();
            public readonly List<Method> methods = new List<Method>();

            public AccessType accessType = AccessType.Public();
            public readonly string name;
            public readonly List<string> baseTypes;

            public readonly bool isStatic;

            public Class(string name, bool isStatic = false, params string[] baseTypes)
            {
                this.name = name;
                this.isStatic = isStatic;
                this.baseTypes = baseTypes.ToList();
            }

            public override string ToString()
            {
                return $"{accessType} {name}{(baseTypes.Count > 0 ? " : " + string.Join(", ", baseTypes) : "")}";
            }
        }

        public class AccessType
        {
            private readonly string type;
            private AccessType(string type)
            {
                this.type = type;
            }

            public override string ToString()
            {
                return type;
            }

            public static AccessType Private() => new AccessType("private");
            public static AccessType Protected() => new AccessType("protected");
            public static AccessType Public() => new AccessType("public");
        }

        public class Statement
        {
        }

        public class Method
        {
            public readonly AccessType accessType;
            public readonly string returnType;
            public readonly string name;
            public readonly List<Parameter> parameters = new List<Parameter>();

            public Statement body;

            public bool isOverride;
            public bool isVirtual;

            public bool isVoid => returnType == null;

            public Method(AccessType accessType, string returnType, string name)
            {
                this.accessType = accessType;
                this.returnType = returnType;
                this.name = name;
            }

            public override string ToString()
            {
                return $"{accessType} {returnType} {name}";
            }
        }

        public class Parameter
        {
            public readonly string type;
            public readonly string name;

            public Parameter(string type, string name)
            {
                this.type = type;
                this.name = name;
            }

            public override string ToString()
            {
                return $"{type} {name}";
            }
        }

        public class Field
        {
            public AccessType accessType;
            public bool isReadonly;
            public string type;
            public string name;
            public string defaultValue;

            public Field(AccessType accessType, bool isReadonly, string type, string name, string defaultValue = null)
            {
                this.accessType = accessType;
                this.isReadonly = isReadonly;
                this.type = type;
                this.name = name;
                this.defaultValue = defaultValue;
            }

            public override string ToString()
            {
                return $"{accessType} {(isReadonly ? "readonly" : "")} {type} {name}";
            }
        }

        public class Property
        {
            public AccessType accessType;
            public Type type;
            public string name;

            public Property(AccessType accessType, Type type, string name)
            {
                this.accessType = accessType;
                this.type = type;
                this.name = name;
            }

            public override string ToString()
            {
                return $"{accessType} {type} {name}";
            }
        }

        public class Constructor
        {
        }

        public static string Capitalize(this string value)
        {
            if (value.Length == 0)
                return value;

            var chars = value.ToCharArray();
            chars[0] = char.ToUpperInvariant(chars[0]);

            return new string(chars);
        }
    }
}
