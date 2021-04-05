using System;
using System.Drawing;
using System.IO;
using Ghostscript.NET;
using Ghostscript.NET.Rasterizer;
using HAF.Domain;

namespace HAF.Web
{
    public class PdfFile : IDisposable
    {
        private static readonly string GhostscriptDllPath;
        private readonly GhostscriptRasterizer _rasterizer;

        static PdfFile()
        {
            GhostscriptDllPath = Path.Combine(
                PathHelpers.GetBaseDirectory(),
                Environment.Is64BitProcess ? "gsdll64.dll" : "gsdll32.dll");
        }

        public PdfFile(Stream pdf)
        {
            _rasterizer = new GhostscriptRasterizer();
            _rasterizer.Open(pdf, new GhostscriptVersionInfo(GhostscriptDllPath), false);
        }

        public int PageCount => _rasterizer.PageCount;

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        public Image GetPageImage(int dpi, int pageNumber) => _rasterizer.GetPage(dpi, dpi, pageNumber);

        private void ReleaseUnmanagedResources()
        {
            _rasterizer.Dispose();
        }

        ~PdfFile()
        {
            ReleaseUnmanagedResources();
        }
    }
}