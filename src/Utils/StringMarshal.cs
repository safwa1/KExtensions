namespace KExtensions.Utils;

public static class StringMarshal
{
    public static int GetFirstSpaceIndex(string input)
    {
        int index1 = input.IndexOf(' ');
        int index2 = input.IndexOf('\t');
        int index3 = input.IndexOf('\n');
        int index4 = input.IndexOf('\r');

        int minIndex = -1;

        if (index1 != -1) minIndex = index1;
        if (index2 != -1) minIndex = minIndex == -1 ? index2 : Math.Min(minIndex, index2);
        if (index3 != -1) minIndex = minIndex == -1 ? index3 : Math.Min(minIndex, index3);
        if (index4 != -1) minIndex = minIndex == -1 ? index4 : Math.Min(minIndex, index4);

        return minIndex;
    }

    public static int GetLastSpaceIndex(string input)
    {
        int index1 = input.LastIndexOf(' ');
        int index2 = input.LastIndexOf('\t');
        int index3 = input.LastIndexOf('\n');
        int index4 = input.LastIndexOf('\r');

        int maxIndex = -1;

        if (index1 != -1) maxIndex = index1;
        if (index2 != -1) maxIndex = maxIndex == -1 ? index2 : Math.Max(maxIndex, index2);
        if (index3 != -1) maxIndex = maxIndex == -1 ? index3 : Math.Max(maxIndex, index3);
        if (index4 != -1) maxIndex = maxIndex == -1 ? index4 : Math.Max(maxIndex, index4);

        return maxIndex;
    }

    public static char[] GetFirstAndLastWordsChars(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return [];

        int inputLen = input.Length;
        int firstSpaceIndex = GetFirstSpaceIndex(input);
        int lastSpaceIndex = GetLastSpaceIndex(input);

        int firstWordLength = firstSpaceIndex == -1 ? inputLen : firstSpaceIndex;
        int lastWordLength = lastSpaceIndex == -1 ? 0 : (inputLen - lastSpaceIndex - 1);
        int resultLength = firstWordLength + lastWordLength;

        char[] result = new char[resultLength];
        int index = 0;

        for (int i = 0; i < firstWordLength; i++)
            result[index++] = input[i];

        if (lastSpaceIndex > 0)
        {
            for (int i = lastSpaceIndex + 1; i < inputLen; i++)
                result[index++] = input[i];
        }

        return result;
    }
}
