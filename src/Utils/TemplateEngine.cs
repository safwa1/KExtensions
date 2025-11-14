using System.Reflection;
using System.Text;

namespace KExtensions.Utils;

internal static class TemplateEngine
{
    public static string Evaluate(string? template, object data)
    {
        if (template is null) return string.Empty;
        var sb = new StringBuilder();
        int idx = 0;
        while (idx < template.Length)
        {
            int open = template.IndexOf('{', idx);
            if (open == -1)
            {
                sb.Append(template[idx..]);
                break;
            }

            sb.Append(template[idx..open]);
            int close = template.IndexOf('}', open + 1);
            if (close == -1)
            {
                sb.Append(template[open]);
                idx = open + 1;
                continue;
            }

            var expr = template[(open + 1)..close].Trim();
            var val = EvaluateExpression(expr, data);
            sb.Append(val);
            idx = close + 1;
        }

        return sb.ToString();
    }

    // Very small expression evaluator: supports:
    // - single identifiers (property lookup)
    // - numeric literals
    // - binary ops: + - * /
    // - string literals in single quotes 'text'
    private static object EvaluateExpression(string expr, object data)
    {
        // If it's a quoted string
        if (expr is ['\'', _, ..] && expr[^1] == '\'')
            return expr[1..^1];

        // Tokenize by space for simplicity: supports patterns like "qty * price"
        var tokens = expr.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (tokens.Length == 1)
        {
            if (double.TryParse(tokens[0], out var d)) return d;
            return LookupProperty(data, tokens[0]) ?? string.Empty;
        }

        // Simple left-to-right evaluation (no operator precedence) for demo purposes
        object? acc = LookupOrNumber(tokens[0], data);
        for (int i = 1; i < tokens.Length - 1; i += 2)
        {
            var op = tokens[i];
            var right = LookupOrNumber(tokens[i + 1], data);
            acc = ApplyOp(acc, op, right);
        }

        return acc ?? string.Empty;
    }

    private static object? LookupOrNumber(string token, object data)
    {
        if (double.TryParse(token, out var d)) return d;
        return LookupProperty(data, token);
    }

    private static object ApplyOp(object left, string op, object right)
    {
        if (left is string || right is string)
        {
            if (op == "+") return left.ToString() + right.ToString();
        }

        var l = Convert.ToDouble(left);
        var r = Convert.ToDouble(right);
        return op switch
        {
            "+" => l + r,
            "-" => l - r,
            "*" => l * r,
            "/" => r == 0 ? 0 : l / r,
            _ => 0
        };
    }

    private static object? LookupProperty(object? data, string name)
    {
        if (data == null) return null;
        // If anonymous object or Expando
        if (data is IDictionary<string, object> dict && dict.TryGetValue(name, out var v)) return v;

        var t = data.GetType();
        var prop = t.GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        if (prop != null) return prop.GetValue(data);

        // Try field
        var fld = t.GetField(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        if (fld != null) return fld.GetValue(data);

        return null;
    }
}