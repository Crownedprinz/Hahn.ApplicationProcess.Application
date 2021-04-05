using System.Collections.Generic;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace  HAF.Domain
{
    public static class PdfHelpers
    {
        public static Document GetPdfDocumentFromBytes(byte[] pdfData)
        {
            using (var ms = new MemoryStream())
            {
                var outputDocument = new Document();
                var writer = new PdfCopy(outputDocument, ms);
                outputDocument.Open();

                var reader = new PdfReader(pdfData);
                for (var i = 1; i <= reader.NumberOfPages; i++)
                    writer.AddPage(writer.GetImportedPage(reader, i));
                writer.FreeReader(reader);
                reader.Close();

                writer.Close();
                outputDocument.Close();
                return outputDocument;
            }
        }

        public static byte[] GetPdfWithPages(byte[] pdfData, IEnumerable<int> pageNumbers)
        {
            using (var ms = new MemoryStream())
            {
                var outputDocument = new Document();
                var writer = new PdfCopy(outputDocument, ms);
                outputDocument.Open();

                var reader = new PdfReader(pdfData);
                foreach (var pageNumber in pageNumbers)
                    writer.AddPage(writer.GetImportedPage(reader, pageNumber));

                writer.FreeReader(reader);
                reader.Close();

                writer.Close();
                outputDocument.Close();
                return ms.ToArray();
            }
        }
    }
}