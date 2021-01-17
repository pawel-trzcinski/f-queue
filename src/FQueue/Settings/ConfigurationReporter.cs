using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FQueue.Settings
{
    public abstract class ConfigurationReporter
    {
#warning TODO - unit tests
        private readonly string _className;
        private readonly string _indent;

        protected const string INDENT = "  ";
        public const string TIME_SPAN_FORMAT = "c";

        protected ConfigurationReporter(string className, string indent)
        {
            _className = className;
            _indent = indent;
        }

        protected void Report(StringBuilder sb, string name, ushort value)
        {
            Report(sb, name, value.ToString(CultureInfo.InvariantCulture));
        }

        protected void Report(StringBuilder sb, string name, int value)
        {
            Report(sb, name, value.ToString(CultureInfo.InvariantCulture));
        }

        protected void Report(StringBuilder sb, string name, uint value)
        {
            Report(sb, name, value.ToString(CultureInfo.InvariantCulture));
        }

        protected void Report(StringBuilder sb, string name, long value)
        {
            Report(sb, name, value.ToString(CultureInfo.InvariantCulture));
        }

        protected void Report(StringBuilder sb, string name, TimeSpan value)
        {
            Report(sb, name, value.ToString(TIME_SPAN_FORMAT, CultureInfo.InvariantCulture));
        }

        protected void Report(StringBuilder sb, string name, string value)
        {
            sb.AppendLine($"{_indent}{_className}.{name} = {value}");
        }

        protected void Report(StringBuilder sb, string name, IEnumerable<string> value)
        {
            sb.AppendLine($"{_indent}{_className}.{name} = {Environment.NewLine}{String.Join(Environment.NewLine, new[] {$"{_indent}["}.Concat(value.Select(p => $"{_indent}{INDENT}{p}")).Concat(new[] {$"{_indent}]"}))}");
        }
    }
}