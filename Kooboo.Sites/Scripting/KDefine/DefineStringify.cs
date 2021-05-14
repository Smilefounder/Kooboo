using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.Scripting.KDefine
{
    public class DefineStringify
    {
        readonly Define[] _defines;
        readonly string _indentation = "  ";

        public DefineStringify(Define[] defines)
        {
            _defines = defines;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            foreach (var defines in _defines.GroupBy(global => global.Namespace))
            {
                if (!string.IsNullOrWhiteSpace(defines.Key))
                {
                    builder.AppendLine($"declare namespace {defines.Key} {{");
                }

                foreach (var define in defines)
                {
                    var declare = string.IsNullOrWhiteSpace(define.Namespace) ? "declare " : string.Empty;

                    if (define.Enums == null)
                    {
                        var extends = define.Extends.Any() ? $"extends {string.Join(",", define.Extends)} " : string.Empty;
                        builder.AppendLine($"{_indentation}{declare}interface {define.Name} {extends}{{");

                        if (define.ValueType != null)
                        {
                            builder.AppendLine($"{_indentation}{_indentation}[key:string]:{define.ValueType};");
                        }

                        if (define.Properties != null)
                        {
                            foreach (var item in define.Properties)
                            {
                                if (item.Discription != null)
                                {
                                    builder.AppendLine($"{_indentation}{_indentation}/** {item.Discription} */");
                                }

                                builder.AppendLine($"{_indentation}{_indentation}{item.Name}:{item.Type};");
                            }
                        }

                        if (define.Methods != null)
                        {
                            foreach (var item in define.Methods)
                            {

                                if (item.Discription != null)
                                {
                                    builder.AppendLine($"{_indentation}{_indentation}/** {item.Discription} */");
                                }
                                var @params = item.Params.Select(s => $"{s.Name}:{s.Type}");
                                builder.AppendLine($"{_indentation}{_indentation}{item.Name}({string.Join(",", @params)}):{item.ReturnType};");
                            }
                        }

                    }
                    else
                    {
                        var enums = define.Enums.Keys.Select(s => $"'{s}'");
                        builder.AppendLine($"{_indentation}{declare}type {define.Name} = {string.Join(" | ", enums)};");
                    }

                    builder.AppendLine($"{_indentation}}}");
                    builder.AppendLine();
                }

                if (!string.IsNullOrWhiteSpace(defines.Key))
                {
                    builder.AppendLine($"}}");
                }
            }

            return builder.ToString();
        }
    }
}
