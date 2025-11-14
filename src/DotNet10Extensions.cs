using System.Numerics;
using KExtensions.Types;
using KExtensions.Utils;

namespace KExtensions;

// #if NET10_0_OR_GREATER
public static class DotNet10Extensions
{
    extension(DirectoryPath)
    {
        public static string operator /(DirectoryPath left, DirectoryPath right)
            => left.Combine(right);
        
        public static string operator /(DirectoryPath left, FilePath right)
            => left.Combine(right.GetFileName());
    }

    extension(string)
    {
        public static string operator /(string left, string right)
            => Path.Combine(left, right);

        public static string operator /(DirectoryPath left, string right)
            => Path.Combine(left, right);

        public static string operator /(string left, DirectoryPath right)
            => Path.Combine(left, right);

        public static string operator /(string left, FilePath right)
            => Path.Combine(left, right);

        public static string operator /(string left, FileInfo right)
            => Path.Combine(left, right.Name);

        public static DirectoryInfo operator /(DirectoryInfo left, string right)
            => new(Path.Combine(left.FullName, right));
    }

    extension(string)
    {
        public static string operator *(string s, int n)
            => string.Concat(Enumerable.Repeat(s, n));
    }

    extension<T>(T) where T : INumber<T>
    {
        public static TimeSpan operator *(T value, string unit)
            => unit switch
            {
                "s" => TimeSpan.FromSeconds(double.CreateChecked(value)),
                "m" => TimeSpan.FromMinutes(double.CreateChecked(value)),
                "h" => TimeSpan.FromHours(double.CreateChecked(value)),
                "d" => TimeSpan.FromDays(double.CreateChecked(value)),
                _ => throw new ArgumentException($"Invalid unit {unit}.", nameof(unit))
            };
    }

    extension(string)
    {
        public static string operator %(string template, object data)
            => TemplateEngine.Evaluate(template, data);
    }

    extension<T>(T) where T : notnull
    {
        public static T? operator &(T? value, Action<T> action)
        {
            if (value is not null)
                action(value);

            return value;
        }
    }

    extension<T, TResult>(T) where T : notnull
    {
        public static TResult? operator &(T? value, Func<T, TResult> func)
        {
            return value is null ? default : func(value);
        }
    }

    extension<T, TResult>(T)
    {
        public static TResult operator |(
            T value,
            Func<T, TResult> mapper)
            => mapper(value);
    }

    extension<T, TResult>(IEnumerable<T>)
    {
        public static IEnumerable<TResult> operator |(
            IEnumerable<T> source,
            Func<T, TResult> mapper)
            => source.Select(mapper);
    }
    
    extension<T>(T val) where T : INumber<T>
    {
        public TimeSpan Second => TimeSpan.FromSeconds(double.CreateChecked(val));

        public TimeSpan Minute => TimeSpan.FromMinutes(double.CreateChecked(val));

        public TimeSpan Hour => TimeSpan.FromHours(double.CreateChecked(val));

        public TimeSpan Day => TimeSpan.FromDays(double.CreateChecked(val));

        public TimeSpan Millis => TimeSpan.FromMilliseconds(double.CreateChecked(val));

        public TimeSpan Micros => TimeSpan.FromMicroseconds(double.CreateChecked(val));
    }
}
// #endif