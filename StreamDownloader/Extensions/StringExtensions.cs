
namespace StreamDownloader.Extensions;

using System.Diagnostics.CodeAnalysis;

public static class StringExtensions
{
    extension(string? value)
    {
        [return: NotNullIfNotNull(nameof(value))]
        public string? DoubleQuoted()
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return $"\"{value}\"";
        }
    }
}
