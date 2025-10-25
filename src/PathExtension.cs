
using KExtensions.Types;

namespace KExtensions;

public static class PathExtension
{
    public static string ItPath(this Environment.SpecialFolder folder)
    {
        return Environment.GetFolderPath(folder);
    }

    public static string SerializePathForJson(this string path)
    {
        return string.Join(
            @"\\",
            Path.GetFullPath(path).Split(
                ["\\"],
                StringSplitOptions.None
            )
        );
    }
    
    public static bool IsValidAsFileName(this string name, out InvalidNameMessage errorMessage)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            errorMessage = new InvalidNameMessage(
                "لايمكن أن يكون الإسم فارغاً",
                "The name cannot be empty."
            );
            return false;
        }

        // Get invalid characters for a directory name
        var invalidChars = Path.GetInvalidPathChars().ToList();
        invalidChars.AddRange([
            '\\',
            '/',
            ':',
            '*',
            '?',
            '"',
            '<',
            '>'
        ]);


        // Check if any invalid character exists in the directory name
        var index = name.IndexOfAny(invalidChars.ToArray());
        errorMessage = new InvalidNameMessage(
            @"اسم الملف لا يمكن أن يحتوي على أي من الأحرف التالية: \/:*?""<>",
            @"A file name can't contain any of the following characters: \/:*?""<>"
            );
        return index == -1;
    }

    /// <summary>
    /// Checks the path of files or directories and returns [TRUE] if it exists.
    /// </summary>
    /// <param name="path">[String] Path of file or directory to check.</param>
    /// <returns>[Boolean]</returns>
    public static bool PathExists(this string path)
    {
        if(string.IsNullOrWhiteSpace(path)) return false;
        return File.Exists(path) || Directory.Exists(path);
    }
}