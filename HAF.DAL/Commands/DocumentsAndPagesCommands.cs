using System.Data.Entity;
using System.Linq;
using HAF.Domain;
using HAF.Domain.CommandParameters;
using HAF.Domain.Entities;

namespace  HAF.DAL.Commands
{
    public class DocumentsAndPagesCommands : ICommand<AddDocument>,
                                             ICommand<AddDropscanMailingAsDocument>,
                                             ICommand<AddDropscanMailingPagesAsDocument>,
                                             ICommand<DiscardPages>,
                                             ICommand<UndiscardPages>,
                                             ICommand<AddDropscanMailingPages>,
                                             ICommand<SetMailingPageCount>
    {
        public void Execute(AddDocument parameters)
        {
            using (var context = new DatabaseContext())
            {
                AddDocument(context, parameters.Document);
                context.SaveChanges();
            }
        }

        public void Execute(AddDropscanMailingAsDocument parameters)
        {
            using (var context = new DatabaseContext())
            {
                var dropscanMailing = context.DropscanMailings.Single(x => x.ID == parameters.MailingID);
                AddDocument(context, parameters.Document);
                dropscanMailing.MappingStatus = DropscanMailingMappingStatus.AddedAsDocument;
                dropscanMailing.DocumentID = parameters.Document.ID;
                context.SaveChanges();
            }
        }

        public void Execute(AddDropscanMailingPages parameters)
        {
            using (var context = new DatabaseContext())
            {
                if (parameters.RemoveExistingPages)
                {
                    foreach (var mailingID in parameters.Pages.GroupBy(x => x.MailingID).Select(x => x.Key))
                    {
                        context.DropscanMailingPages.RemoveRange(
                            context.DropscanMailingPages.Where(x => x.MailingID == mailingID));
                    }
                }

                context.DropscanMailingPages.AddRange(parameters.Pages);
                context.SaveChanges();

                foreach (var group in parameters.Pages.GroupBy(x => x.MailingID))
                    UpdateMappingStatus(context, group.Key);
            }
        }

        public void Execute(AddDropscanMailingPagesAsDocument parameters)
        {
            using (var context = new DatabaseContext())
            {
                AddDocument(context, parameters.Document);
                context.MappedDropscanMailingPages.AddRange(
                    parameters.PageNumbers.Select(
                        x => new MappedDropscanMailingPage
                        {
                            MailingID = parameters.MailingID, PageNumber = x, Document = parameters.Document
                        }));
                var pagesToUndiscard = context.DiscardedDropscanMailingPages.Where(
                    x => x.MailingID == parameters.MailingID && parameters.PageNumbers.Contains(x.PageNumber));
                context.DiscardedDropscanMailingPages.RemoveRange(pagesToUndiscard);
                context.SaveChanges();

                UpdateMappingStatus(context, parameters.MailingID);
            }
        }

        public void Execute(DiscardPages parameters)
        {
            using (var context = new DatabaseContext())
            {
                var pagesToAdd = parameters.PageNumbers.Except(
                        context.DiscardedDropscanMailingPages.Where(x => x.MailingID == parameters.MailingID)
                            .Select(x => x.PageNumber))
                    .ToList();
                context.DiscardedDropscanMailingPages.AddRange(
                    pagesToAdd.Select(
                        x => new DiscardedDropscanMailingPage { MailingID = parameters.MailingID, PageNumber = x }));
                context.SaveChanges();

                UpdateMappingStatus(context, parameters.MailingID);
            }
        }

        public void Execute(SetMailingPageCount parameters)
        {
            using (var context = new DatabaseContext())
            {
                var mailing = context.DropscanMailings.Single(x => x.ID == parameters.ID);
                mailing.PageCount = parameters.PageCount;
                context.SaveChanges();
            }
        }

        public void Execute(UndiscardPages parameters)
        {
            using (var context = new DatabaseContext())
            {
                var pagesToUndiscard = context.DiscardedDropscanMailingPages.Where(
                    x => x.MailingID == parameters.MailingID && parameters.PageNumbers.Contains(x.PageNumber));
                context.DiscardedDropscanMailingPages.RemoveRange(pagesToUndiscard);
                context.SaveChanges();

                UpdateMappingStatus(context, parameters.MailingID);
            }
        }

        private static void AddDocument(DatabaseContext context, Document parametersDocument)
        {
            if (parametersDocument.AssignedFlags != null)
            {
                foreach (var documentFlag in parametersDocument.AssignedFlags)
                    context.DocumentFlags.Attach(documentFlag);
            }

            context.Documents.Add(parametersDocument);
        }

        private static void UpdateMappingStatus(DatabaseContext context, int parametersMailingId)
        {
            var discardedPages = context.DiscardedDropscanMailingPages.Where(x => x.MailingID == parametersMailingId)
                .Select(x => x.PageNumber)
                .ToList();
            var mappedPages = context.MappedDropscanMailingPages.Where(x => x.MailingID == parametersMailingId)
                .Select(x => x.PageNumber)
                .ToList();
            var dropscanMailing = context.DropscanMailings.Include(x => x.Pages).Single(x => x.ID == parametersMailingId);

            if (dropscanMailing.Pages.All(x => discardedPages.Contains(x.PageNumber)))
                dropscanMailing.MappingStatus = DropscanMailingMappingStatus.Discarded;
            else if (dropscanMailing.Pages.All(
                x => mappedPages.Contains(x.PageNumber) || discardedPages.Contains(x.PageNumber)))
                dropscanMailing.MappingStatus = DropscanMailingMappingStatus.Splitted;
            else
                dropscanMailing.MappingStatus = DropscanMailingMappingStatus.PartiallySplitted;

            context.SaveChanges();
        }
    }
}