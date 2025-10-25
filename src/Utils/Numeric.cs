namespace KExtensions.Utils;

public static class NumericUtils
{
    /// <summary>
    /// Determines if a type is numeric. Nullable numeric types are considered numeric.
    /// </summary>
    /// <remarks>
    /// Boolean is not considered numeric.
    /// </remarks>
    public static bool IsNumeric(Type? type)
    {
        if (type == null) return false;

        switch (Type.GetTypeCode(type))
        {
            case TypeCode.Byte:
            case TypeCode.Decimal:
            case TypeCode.Double:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
            case TypeCode.SByte:
            case TypeCode.Single:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
                return true;
            case TypeCode.Object:
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    return IsNumeric(Nullable.GetUnderlyingType(type));
                }
                return false;
        }
        return false;
    }

    public static bool IsNumeric(Type? type, out TypeCode typeCode)
    {
        if (type == null)
        {
            typeCode = TypeCode.Empty;
            return false;
        }

        // Handle Nullable types
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            type = Nullable.GetUnderlyingType(type);
        }

        // Check for numeric TypeCode
        switch (Type.GetTypeCode(type))
        {
            case TypeCode.Byte:
            case TypeCode.Decimal:
            case TypeCode.Double:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
            case TypeCode.SByte:
            case TypeCode.Single:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
                typeCode = Type.GetTypeCode(type);
                return true;
            default:
                typeCode = TypeCode.Empty;
                return false;
        }
    }

    public static bool Is<T>()
    {
        return IsNumeric(typeof(T));
    }
}