# PDF Maker

PDF Maker is a historical C# prototype that generates a structured PDF report
from text, images, and JSON data. It demonstrates document composition with
iTextSharp, including headings, tables, image scaling, page numbers,
backgrounds, and repeated image watermarks.

## Features

- Create a multi-page PDF report with document metadata.
- Read introduction and conclusion sections from UTF-8 text files.
- Convert JSON records into a PDF table.
- Scale and position images within page bounds.
- Add page numbers, background colors, and tiled watermarks.
- Render Chinese text with a Windows font.

## Technology

- C# and .NET Framework 4.8
- iTextSharp 5.5.13.3 and XML Worker
- Newtonsoft.Json 13.0.1

## Build and Run

1. Use Windows with Visual Studio and .NET Framework 4.8.
2. Open `pdfmaker.sln` and restore the NuGet packages from `packages.config`.
3. Create `sample-data`, place authorized input files under it, and review the
   expected filenames in `Program.cs`.
4. Build and run the console application. Generated reports are written to
   `output/report.pdf`.

## Important Notes

- The project uses generic relative sample paths and does not include company
  reports, captured data, or generated binaries.
- iTextSharp 5 uses AGPL/commercial licensing. Review its license before
  distributing an application based on this code.
- Generated build output is excluded by `.gitignore`.
