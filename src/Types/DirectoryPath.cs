namespace KExtensions.Types;

public struct DirectoryPath
{
    private readonly ReadOnlyMemory<char> _value;
    private string? _cachedValue;

    public DirectoryPath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("Path cannot be null or empty.", nameof(path));

        // Normalize separators
        path = path.Replace('/', Path.DirectorySeparatorChar)
            .Replace('\\', Path.DirectorySeparatorChar)
            .TrimEnd(Path.DirectorySeparatorChar);

        _value = path.AsMemory();
        _cachedValue = null;
    }

    public DirectoryPath(ReadOnlyMemory<char> path)
    {
        _value = path;
        _cachedValue = null;
    }

    public static implicit operator DirectoryPath(string path) => new(path);
    public static implicit operator string(DirectoryPath dir) => dir.ToString();
    public static implicit operator ReadOnlySpan<char>(DirectoryPath dir) => dir._value.Span;

    public ReadOnlySpan<char> AsSpan() => _value.Span;

    public override string ToString() => _cachedValue ??= _value.Span.ToString();

    public bool Exists() => Directory.Exists(ToString());

    public bool Delete(bool recursive = true)
    {
        if (Exists())
        {
            Directory.Delete(ToString(), recursive);
        }
        return !Exists();
    }

    public DirectoryInfo GetInfo()
    {
        if (!Exists())
            throw new DirectoryNotFoundException($"Directory does not exist: {ToString()}");
        return new DirectoryInfo(ToString());
    }

    public DirectoryPath Combine(DirectoryPath subDir)
    {
        ReadOnlySpan<char> dirSpan = _value.Span;
        ReadOnlySpan<char> subSpan = subDir._value.Span;

        bool needSeparator = dirSpan.Length > 0 && dirSpan[^1] != Path.DirectorySeparatorChar;
        int combinedLength = dirSpan.Length + subSpan.Length + (needSeparator ? 1 : 0);

        Span<char> buffer = combinedLength <= 512 ? stackalloc char[combinedLength] : new char[combinedLength];

        int pos = 0;
        dirSpan.CopyTo(buffer);
        pos += dirSpan.Length;

        if (needSeparator)
            buffer[pos++] = Path.DirectorySeparatorChar;

        subSpan.CopyTo(buffer.Slice(pos));

        return new DirectoryPath(buffer.ToArray());
    }
        
    public DirectoryPath Combine(params DirectoryPath[]? parts)
    {
        if (parts == null || parts.Length == 0)
            return this;
            
        int totalLength = _value.Length;
        for (int i = 0; i < parts.Length; i++)
        {
            totalLength += parts[i]._value.Length;
            totalLength += 1;
        }

        Span<char> buffer = totalLength <= 512 ? stackalloc char[totalLength] : new char[totalLength];

        int pos = 0;

        _value.Span.CopyTo(buffer);
        pos += _value.Length;
            
        foreach (var part in parts)
        {
            if (pos > 0 && buffer[pos - 1] != Path.DirectorySeparatorChar)
            {
                buffer[pos++] = Path.DirectorySeparatorChar;
            }

            part._value.Span.CopyTo(buffer.Slice(pos));
            pos += part._value.Length;
        }
            
        return new DirectoryPath(buffer.Slice(0, pos).ToArray());
    }

    public DirectoryPath Concat(DirectoryPath subDir) => Combine(subDir);

    public ReadOnlySpan<char> GetDrive()
    {
        int colonIndex = _value.Span.IndexOf(':');
        return colonIndex >= 0 ? _value.Span[..(colonIndex + 1)] : ReadOnlySpan<char>.Empty;
    }

    public IEnumerable<DirectoryPath> GetSubDirectories()
    {
        if (!Exists())
            throw new DirectoryNotFoundException($"Directory does not exist: {ToString()}");

        foreach (var sub in Directory.GetDirectories(ToString()))
            yield return new DirectoryPath(sub);
    }

    public IEnumerable<DirectoryPath> GetSubDirectoriesRecursive()
    {
        foreach (var sub in GetSubDirectories())
        {
            yield return sub;
            foreach (var child in sub.GetSubDirectoriesRecursive())
                yield return child;
        }
    }

    public IEnumerable<string> GetFiles(string searchPattern = "*", bool recursive = false)
    {
        if (!Exists())
            throw new DirectoryNotFoundException($"Directory does not exist: {ToString()}");

        if (recursive)
            return Directory.GetFiles(ToString(), searchPattern, SearchOption.AllDirectories);
        else
            return Directory.GetFiles(ToString(), searchPattern, SearchOption.TopDirectoryOnly);
    }
}