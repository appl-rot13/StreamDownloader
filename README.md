[English](README.md) | [日本語](README.ja.md)

# Stream Downloader

A tool for downloading streams.

> [!WARNING]
> The use of this tool may violate the law and each site's Terms of Service.  
> I will not be held responsible for any losses or damages caused by the use of this tool.

## Overview

Use yt-dlp to download the specified streams.

## Usage

1. Download [yt-dlp](https://github.com/yt-dlp/yt-dlp) and [its dependencies](https://github.com/yt-dlp/FFmpeg-Builds), then place them in the same folder.
2. Set up the `appsettings.json` file, then run the tool.

### Set Up `appsettings.json` File

Set up the `appsettings.json` file to match your environment.

#### yt-dlp Settings

- `FilePath` - Set the file path of yt-dlp.
- `Options` - Optional: Set the command-line options for yt-dlp.

```json
"DownloaderSettings": {
  "FilePath": "yt-dlp.exe",
  "Options": [
    {
      "Enabled": true,
      "Value": "--cookies cookies.txt"
    }
  ]
}
```

## License

This software is licensed under the [Unlicense](LICENSE).
