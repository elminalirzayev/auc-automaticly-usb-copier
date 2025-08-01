# auc-automaticly-usb-copier

A background Windows service that automatically copies files from USB drives to `C:\auc-backup` as soon as they are connected.

## Features

- Runs invisibly as a Windows Service
- Automatically detects inserted USB drives
- Recursively copies all files and folders to `C:\auc-backup`
- Timestamp-based folder naming to avoid overwriting
- Lightweight and efficient

## Requirements

- Windows 10/11 or Windows Server
- .NET 8 or newer
- Admin rights (for installing the service)

## Installation

1. **Build & Publish:**
   Use Visual Studio → Right-click project → `Publish` → Target Folder → `Self-contained`

2. **Install the service:**
   Run the following in an **Administrator Command Prompt**:

   ```bash
   sc create AucUsbCopier binPath= "C:\path\to\auc-automaticly-usb-copier.exe"
   sc start AucUsbCopier
   ```

3. **Test:**
   Insert a USB drive. The contents should be copied automatically to `C:\auc-backup`.

## Metadata

- **Author:** Elmin Alirzayev
- **Company:** Easy Code Tools
- **Product:** auc-automaticly-usb-copier
- **Repository:** https://github.com/elminalirzayev/auc-automaticly-usb-copier

## License

MIT License 

---

© 2017 Elmin Alirzayev / Easy Code Tools 
