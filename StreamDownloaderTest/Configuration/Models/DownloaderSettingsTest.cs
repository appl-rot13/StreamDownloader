namespace StreamDownloaderTest.Configuration.Models;

using Shouldly;
using StreamDownloader.Configuration.Models;

[TestClass]
public class DownloaderSettingsTest
{
    [TestMethod]
    public void Validate_ValidFilePath_DoesNotThrow()
    {
        var filePath = Path.GetTempFileName();
        try
        {
            var settings = CreateDownloaderSettings(filePath);
            Should.NotThrow(settings.Validate);
        }
        finally
        {
            File.Delete(filePath);
        }
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow(null)]
    public void Validate_InvalidFilePath_ThrowsInvalidOperationException(string filePath)
    {
        var settings = CreateDownloaderSettings(filePath);
        Should.Throw<InvalidOperationException>(settings.Validate);
    }

    [TestMethod]
    public void Validate_NonexistentFilePath_ThrowsFileNotFoundException()
    {
        var settings = CreateDownloaderSettings(GetNonexistentTempFileName());
        Should.Throw<FileNotFoundException>(settings.Validate);
    }

    private static DownloaderSettings CreateDownloaderSettings(string filePath)
    {
        return new DownloaderSettings { FilePath = filePath, Options = [] };
    }

    private static string GetNonexistentTempFileName()
    {
        while (true)
        {
            var filePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            if (!File.Exists(filePath))
            {
                return filePath;
            }
        }
    }
}
