using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Security;
using iTextSharp.text;
using RestSharp;
using HAF.Domain;

namespace  HAF.Connectors.Dropscan
{
    public static class Api
    {
        private static readonly byte[] CertificatePublicKey =
        {
            0x30, 0x82, 0x01, 0x0a, 0x02, 0x82, 0x01, 0x01, 0x00, 0xb1, 0x58, 0x20, 0xf0, 0x67, 0x14, 0x2c, 0x14, 0x96,
            0xce, 0xb9, 0x47, 0x26, 0xb3, 0x7a, 0x9a, 0xda, 0x24, 0xd9, 0xb9, 0x4f, 0x10, 0xc7, 0x56, 0xb8, 0x92, 0x50,
            0xc3, 0xaf, 0x3f, 0x5f, 0x91, 0x0e, 0x94, 0x1a, 0x81, 0x1e, 0xc1, 0x6e, 0x6d, 0x51, 0xfa, 0x79, 0x79, 0x50,
            0xb0, 0x90, 0x66, 0xfc, 0x3e, 0x6e, 0x0d, 0xa7, 0x4a, 0x0f, 0xa4, 0xa7, 0xb6, 0xdd, 0x1c, 0x8c, 0x59, 0xab,
            0x35, 0x96, 0xfb, 0xba, 0x9e, 0xf8, 0x77, 0xd6, 0x38, 0x03, 0x24, 0xd7, 0x72, 0xfd, 0x9c, 0xa1, 0xcb, 0xb0,
            0x78, 0xe6, 0xb8, 0x7d, 0x94, 0x0e, 0x1e, 0x35, 0x75, 0x21, 0xde, 0x99, 0xa1, 0x45, 0xb4, 0xe3, 0x0d, 0x56,
            0x1c, 0xbf, 0x09, 0x6c, 0xe7, 0x22, 0xfc, 0x0d, 0x41, 0xb7, 0x3b, 0xc3, 0x63, 0x11, 0x83, 0x7f, 0x17, 0x33,
            0x89, 0xb1, 0x8b, 0xb3, 0xb3, 0x99, 0xff, 0x06, 0xf3, 0xed, 0x38, 0xcb, 0x95, 0x2f, 0xa5, 0xd9, 0x75, 0x97,
            0xa5, 0x96, 0x14, 0x07, 0xb9, 0x6d, 0xc7, 0x6c, 0xda, 0x75, 0xa0, 0x50, 0x39, 0x85, 0xa3, 0x35, 0x04, 0xa3,
            0x04, 0x75, 0x46, 0x29, 0xe9, 0x02, 0xea, 0xc7, 0x23, 0xc8, 0x32, 0xf4, 0x24, 0xf9, 0xee, 0x4c, 0x5f, 0x28,
            0x83, 0xc2, 0xe9, 0xcc, 0x6b, 0x9e, 0x64, 0xdc, 0xec, 0x68, 0x0c, 0xba, 0x5e, 0x28, 0x9b, 0x7a, 0x32, 0xa9,
            0xf0, 0x01, 0x6d, 0x91, 0x32, 0xc8, 0x8e, 0x97, 0xdd, 0x52, 0x8f, 0x20, 0x13, 0x52, 0x84, 0x58, 0x98, 0xfe,
            0x7b, 0x1c, 0xa6, 0x5d, 0xec, 0xe9, 0x6c, 0x72, 0x2f, 0x8e, 0xc8, 0x12, 0x09, 0x2f, 0xdf, 0x25, 0xea, 0x09,
            0x0a, 0x47, 0x6f, 0x0c, 0xde, 0x1a, 0xe6, 0x77, 0x49, 0x53, 0xcb, 0x0f, 0xc7, 0x46, 0x86, 0x62, 0x57, 0x6a,
            0x59, 0x1d, 0xf2, 0xf8, 0xe4, 0x15, 0x8c, 0x0c, 0x1a, 0x6c, 0x7d, 0x3f, 0x21, 0x02, 0x03, 0x01, 0x00, 0x01
        };

        private static readonly RestClient _client = new RestClient(ConfigurationManager.AppSettings["MockUpUrl"]);
        private static readonly ImageConverter _imageConverter = new ImageConverter();

        static Api()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) =>
                sslPolicyErrors == SslPolicyErrors.None ||
                sslPolicyErrors == SslPolicyErrors.RemoteCertificateNameMismatch &&
                certificate.GetPublicKey().SequenceEqual(CertificatePublicKey);
        }

        public static int ScanboxId { get; } = 2854;
            
        public static void DestroyMailing(int scanboxId, string mailingId)
        {
            RequestAction(scanboxId, mailingId, "destroy");
        }

        public static Bitmap GetEnvelope(int scanboxId, string mailingId) =>
            (Bitmap)_imageConverter.ConvertFrom(GetEnvelopeRawData(scanboxId, mailingId));

        public static byte[] GetEnvelopeRawData(int scanboxId, string mailingId)
        {
            var request = CreateRequest("scanboxes/{scanboxId}/mailings/{mailingId}/envelope", scanboxId, mailingId);
            var rawBytes = _client.Execute(request).RawBytes;
            return rawBytes;
        }

        public static IEnumerable<Mailing> GetMailings(int scanboxId)
        {
            var request = CreateRequest("scanboxes/{id}/mailings/");
            request.AddUrlSegment("id", scanboxId.ToString());
            return _client.Execute<List<Mailing>>(request).Data;
        }

        public static Document GetPdf(int scanboxId, string mailingId) =>
            PdfHelpers.GetPdfDocumentFromBytes(GetPdfRawData(scanboxId, mailingId));

        public static byte[] GetPdfRawData(int scanboxId, string mailingId) =>
            _client.Execute(CreateRequest("scanboxes/{scanboxId}/mailings/{mailingId}/pdf", scanboxId, mailingId)).RawBytes;

        public static IEnumerable<Scanbox> GetScanboxes() => _client.Execute<List<Scanbox>>(CreateRequest("scanboxes")).Data;

        public static byte[] GetZipRawData(int scanboxId, string mailingId) =>
            _client.Execute(CreateRequest("scanboxes/{scanboxId}/mailings/{mailingId}/zip", scanboxId, mailingId)).RawBytes;

        public static void ScanMailing(int scanboxId, string mailingId)
        {
            RequestAction(scanboxId, mailingId, "scan");
        }

        private static RestRequest CreateRequest(string resource, int scanboxId, string mailingId)
        {
            var request = CreateRequest(resource);
            request.AddUrlSegment("scanboxId", scanboxId.ToString());
            request.AddUrlSegment("mailingId", mailingId);
            return request;
        }

        private static RestRequest CreateRequest(string resource)
        {
            var request = new RestRequest(resource);
           request.AddHeader("Authorization", ConfigurationManager.AppSettings["Authorization"]);
            return request;
        }

        private static void RequestAction(int scanboxId, string mailingId, string actionType)
        {
            var request = CreateRequest("scanboxes/{scanboxId}/mailings/{mailingId}/action_requests", scanboxId, mailingId);
            request.Method = Method.POST;
            request.AddJsonBody(new { action_type = actionType });
            _client.Execute(request);
        }
    }
}