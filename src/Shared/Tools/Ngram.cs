namespace Shared.Tools;

public static class StringExtensions
{
    public static string ToNgram(this string sentence)
    {
        int n = 3;
        if (string.IsNullOrWhiteSpace(sentence)) return string.Empty;
        var words = sentence.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var allGrams = new List<string>();

        foreach (var word in words)
        {
            if (word.Length < n)
            {
                allGrams.Add(word);
                continue;
            }
            var ngrams = Enumerable.Range(0, word.Length - n + 1)
                .Select(i => word.Substring(i, n))
                .ToList();
            allGrams.AddRange(ngrams);
        }
        return string.Join(" ", allGrams);
    }
}
