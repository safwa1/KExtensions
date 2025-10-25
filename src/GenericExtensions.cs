using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;
using KExtensions.Utils;

namespace KExtensions;

public static class GenericExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? To<T>(this object? value)
    {
        if (value is null) return default;

        Type targetType = typeof(T);

        // Handle Nullable<T>
        Type underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

        try
        {
            // If the value is already of the target type, return it directly
            if (underlyingType.IsInstanceOfType(value)) return (T)value;

            // Handle string input
            if (value is string str)
            {
                if (underlyingType == typeof(int) && int.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture,
                        out var intValue))
                    return (T)(object)intValue;

                if (underlyingType == typeof(long) && long.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture,
                        out var longValue))
                    return (T)(object)longValue;

                if (underlyingType == typeof(float) && float.TryParse(str, NumberStyles.Any,
                        CultureInfo.InvariantCulture, out var floatValue))
                    return (T)(object)floatValue;

                if (underlyingType == typeof(double) && double.TryParse(str, NumberStyles.Any,
                        CultureInfo.InvariantCulture, out var doubleValue))
                    return (T)(object)doubleValue;

                if (underlyingType == typeof(decimal) && decimal.TryParse(str, NumberStyles.Any,
                        CultureInfo.InvariantCulture, out var decimalValue))
                    return (T)(object)decimalValue;

                if (underlyingType == typeof(bool) && bool.TryParse(str, out var boolValue))
                    return (T)(object)boolValue;

                if (underlyingType.IsEnum)
                    return (T)Enum.Parse(underlyingType, str, ignoreCase: true);
            }

            // Handle numeric conversion from other primitives
            if (underlyingType.IsEnum)
                return (T)Enum.ToObject(underlyingType, value);

            return (T)Convert.ChangeType(value, underlyingType, CultureInfo.InvariantCulture);
        }
        catch
        {
            return default;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNumeric(this object? value)
    {
        return value is not null && NumericUtils.IsNumeric(value.GetType());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNumeric(this object? value, out TypeCode typeCode)
    {
        typeCode = TypeCode.Empty;
        return value is not null && NumericUtils.IsNumeric(value.GetType(), out typeCode);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Dump<T>(this T value) => Console.WriteLine(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Dump<T>(this T value, string label) => Console.WriteLine($"{label}: {value}");
    
    /// <summary>
    /// Checks if the object is of type T (including derived types or implemented interfaces)
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TypeIs<T>(this object? value)
    {
        return value is T;
    }

    /// <summary>
    /// Checks if the object is exactly of type T (no derived types)
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TypeIsExact<T>(this object? value)
    {
        return value?.GetType() == typeof(T);
    }

    /// <summary>
    /// Checks if the type of the object can be assigned to type T (like 'is assignable to')
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TypeIsAssignableTo<T>(this object? value)
    {
        return value != null && typeof(T).IsAssignableFrom(value.GetType());
    }

    /// <summary>
    /// Checks if the object is null or of type T
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TypeIsOrNull<T>(this object? value)
    {
        return value == null || value is T;
    }

    /// <summary>
    /// Casts the object to T if possible, otherwise returns null
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? AsType<T>(this object? value) where T : class
    {
        return value as T;
    }

    /// <summary>
    /// Casts the object to T if possible, otherwise returns default(T)
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T AsTypeOrDefault<T>(this object? value)
    {
        return value is T t ? t : default!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNull<T>(this T? args, decimal? tryDecimalInvariant) => args is null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotNull<T>(this T? args) => args is not null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsDefault<T>(this T obj) where T : struct, IEquatable<T>
    {
        return EqualityComparer<T>.Default.Equals(obj, default);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string Serialize<T>(this T obj)
    {
        var data = JsonSerializer.Serialize(obj);
        return data;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? Deserialize<T>(this string jsonData)
    {
        var copy = JsonSerializer.Deserialize<T>(jsonData);
        return copy;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? CloneObject<T>(this T obj)
    {
        if (obj is null) return default;

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles,
            IncludeFields = true
        };

        return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(obj, options), options);
    }

    /// <summary>
    /// Returns the value of a member attribute for any member in a class.
    /// (a member is a Field, Property, Method, etc...)
    /// <remarks>
    /// If there is more than one member of the same name in the class, it will return the first one (this applies to overloaded methods)
    /// </remarks>
    /// <example>
    /// Read System.ComponentModel Description Attribute from method 'MyMethodName' in clas 'MyClass':
    /// var Attribute = typeof(MyClass).GetAttribute("MyMethodName", (DescriptionAttribute d) => d.Description);
    /// </example>
    /// <param name="type">The class that contains the member as a type</param>
    /// <param name="memberName">Name of the member in the class</param>
    /// <param name="valueSelector">Attribute type and property to get (will return first instance if there are multiple attributes of the same type)</param>
    /// <param name="inherit">true to search this member's inheritance chain to find the attributes; otherwise, false. This parameter is ignored for properties and events</param>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TValue? GetAttribute<TAttribute, TValue>(this Type type, string memberName,
        Func<TAttribute, TValue> valueSelector, bool inherit = false) where TAttribute : Attribute
    {
        if (type.GetMember(memberName).FirstOrDefault()
                ?.GetCustomAttributes(typeof(TAttribute),
                    inherit).FirstOrDefault() is TAttribute att)
        {
            return valueSelector(att);
        }

        return default;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T ParseTo<T>(this string input, IFormatProvider? formatProvider = null) where T : IParsable<T>
    {
        return T.Parse(input, formatProvider);
    }
}