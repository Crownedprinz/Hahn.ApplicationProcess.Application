using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using HAF.Domain;
using HAF.Domain.CommandParameters;
using HAF.Domain.Entities;
using HAF.Domain.QueryParameters;

namespace  HAF.Connectors.Dropscan
{
    public class Connector
    {
        private static readonly DateTime MinimumReceiveDate = new DateTime(2017, 1, 1);
        private readonly ICommand<AddMailings> _addMailings;
        private readonly IQuery<KnownMailingsUuids, IEnumerable<string>> _queryKnownMailingUuids;

        public Connector(
            ICommand<AddMailings> addMailings,
            ICommand<AddRecipients> addRecipients,
            IQuery<KnownMailingsUuids, IEnumerable<string>> queryKnownMailingUuids)
        {
            if (addRecipients == null)
                throw new ArgumentNullException(nameof(addRecipients));
            _addMailings = addMailings ?? throw new ArgumentNullException(nameof(addMailings));
            _queryKnownMailingUuids = queryKnownMailingUuids ?? throw new ArgumentNullException(nameof(queryKnownMailingUuids));
        }

        public IEnumerable<int> ImportNewMailings()
        {
            var scanboxes = Api.GetScanboxes();
            var recipients = scanboxes.SelectMany(x => x.Recipients)
                .ToDictionary(x => x.Id, x => new DropscanRecipient { ExternalID = x.Id, Name = x.Name });
            var mailings = Api.GetMailings(Api.ScanboxId);
            var knownMailingUuids = _queryKnownMailingUuids.Execute(new KnownMailingsUuids());

            var newEntities = new List<DropscanMailing>();
            var relevantMailings = mailings.Where(
                x => x.ScannedAt != null && !knownMailingUuids.Contains(x.Uuid) && x.ReceivedAt >= MinimumReceiveDate);
            foreach (var mailing in relevantMailings.OrderBy(x => x.ScannedAt.GetValueOrDefault()))
            {
                var result = Mapper.Map<Mailing, DropscanMailing>(mailing);
                result.MappingStatus = DropscanMailingMappingStatus.Imported;
                result.FileData = new DocumentData(Api.GetPdfRawData(Api.ScanboxId, mailing.Uuid));
                result.EnvelopeData = new DocumentData(Api.GetEnvelopeRawData(Api.ScanboxId, mailing.Uuid));
                DropscanRecipient recipient;
                if (recipients.TryGetValue(mailing.RecipientId, out recipient))
                    result.Recipient = recipient;
                newEntities.Add(result);
            }

            _addMailings.Execute(new AddMailings(newEntities));
            return newEntities.Select(x => x.ID);
        }
    }
}