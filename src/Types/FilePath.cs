namespace KExtensions.Types;

public struct FilePath
{
    private readonly ReadOnlyMemory<char> _value;
    private string? _cachedValue;

    public FilePath(ReadOnlyMemory<char> location)
    {
        _value = location;
        _cachedValue = null;
    }

    public FilePath(string location)
    {
        _value = location.AsMemory();
        _cachedValue = location;
    }

    public static implicit operator FilePath(string location) => new FilePath(location);
    public static implicit operator string(FilePath file) => file.ToString();
    public static implicit operator ReadOnlySpan<char>(FilePath file) => file._value.Span;

    public ReadOnlySpan<char> AsSpan() => _value.Span;

    public override string ToString() => _cachedValue ??= _value.Span.ToString();

    // ------------------- Combine -------------------
    public static FilePath Combine(FilePath file1, FilePath file2)
    {
        return CombinePaths(file1, file2.AsSpan());
    }

    public static FilePath Combine(DirectoryPath dir, FilePath file)
    {
        return CombinePaths(dir.AsSpan(), file.AsSpan());
    }

    private static FilePath CombinePaths(ReadOnlySpan<char> first, ReadOnlySpan<char> second)
    {
        int sepLen = 1;
        bool needSeparator = first.Length > 0 && first[^1] != Path.DirectorySeparatorChar;
        int combinedLength = first.Length + (needSeparator ? sepLen : 0) + second.Length;
        var buffer = new char[combinedLength];

        first.CopyTo(buffer);
        if (needSeparator) buffer[first.Length] = Path.DirectorySeparatorChar;
        second.CopyTo(buffer.AsSpan(first.Length + (needSeparator ? 1 : 0)));

        return new FilePath(buffer.AsMemory());
    }

    // ------------------- Existence -------------------
    private void EnsureExists()
    {
        if (!IsExist())
            throw new FileNotFoundException("File does not exist.", ToString());
    }

    public bool IsExist() => File.Exists(ToString());

    public bool Delete()
    {
        if (IsExist()) File.Delete(ToString());
        return !IsExist();
    }

    // ------------------- File Info -------------------
    public FileInfo GetInfo() { EnsureExists(); return new FileInfo(ToString()); }
    public DateTime GetCreationTime() { EnsureExists(); return File.GetCreationTime(ToString()); }
    public DateTime GetLastWriteTime() { EnsureExists(); return File.GetLastWriteTime(ToString()); }
    public DateTime GetLastAccessTime() { EnsureExists(); return File.GetLastAccessTime(ToString()); }
    public FileAttributes GetAttributes() { EnsureExists(); return File.GetAttributes(ToString()); }

    // ------------------- Path Components -------------------
    public string GetDrive()
    {
        var span = _value.Span;
        int colonIndex = span.IndexOf(':');
        return colonIndex >= 0 ? span.Slice(0, colonIndex + 1).ToString() : string.Empty;
    }

    public DirectoryPath GetParentPath()
    {
        var span = _value.Span;
        int lastSep = span.LastIndexOf(Path.DirectorySeparatorChar);
        return lastSep >= 0 ? new DirectoryPath(span.Slice(0, lastSep).ToString()) 
            : new DirectoryPath(ReadOnlyMemory<char>.Empty);
    }

    public string GetFileName()
    {
        var span = _value.Span;
        int lastSep = span.LastIndexOf(Path.DirectorySeparatorChar);
        return lastSep >= 0 ? span.Slice(lastSep + 1).ToString() : span.ToString();
    }

    public string GetFileNameWithoutExtension()
    {
        var span = _value.Span;
        int lastSep = span.LastIndexOf(Path.DirectorySeparatorChar);
        int lastDot = span.LastIndexOf('.');
            
        int start = lastSep + 1;
        int length = (lastDot > start) ? lastDot - start : span.Length - start;

        return span.Slice(start, length).ToString();
    }

    public string GetExtension()
    {
        var span = _value.Span;
        int lastDot = span.LastIndexOf('.');
        int lastSep = span.LastIndexOf(Path.DirectorySeparatorChar);

        if (lastDot == -1 || lastDot < lastSep) return string.Empty;
        return span.Slice(lastDot).ToString();
    }

    // ------------------- Rename -------------------
    public FilePath Rename(string newName)
    {
        if (string.IsNullOrEmpty(newName))
            throw new ArgumentException("New name cannot be null or empty.", nameof(newName));

        var span = _value.Span;
        int lastSep = span.LastIndexOf(Path.DirectorySeparatorChar);
        return lastSep >= 0
            ? CombinePaths(span.Slice(0, lastSep + 1), newName.AsSpan())
            : new FilePath(newName);
    }
}