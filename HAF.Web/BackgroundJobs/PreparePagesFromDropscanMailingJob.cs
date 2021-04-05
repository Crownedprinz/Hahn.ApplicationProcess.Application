using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using HAF.Domain;
using HAF.Domain.CommandParameters;
using HAF.Domain.Entities;
using HAF.Domain.QueryParameters;

namespace HAF.Web.BackgroundJobs
{
    public class PreparePagesFromDropscanMailingJob : BackgroundJobHandler<PreparePagesFromDropscanMailingResult>
    {
        private readonly ICommand<AddDropscanMailingPages> _addPages;
        private readonly IQuery<MailingForPageCreation, DropscanMailing> _queryMailingForPageCreation;
        private readonly ICommand<SetMailingPageCount> _setMailingPageCount;

        public PreparePagesFromDropscanMailingJob(
            ICommand<AddDropscanMailingPages> addPages,
            IQuerySingle<DropscanMailing> querySingleDropscanMailing,
            ICommand<StartBackgroundJobProcessing> startJobProcessing,
            ICommand<SucceedBackgroundJob> succeedJob,
            ICommand<FailBackgroundJob> failJob,
            IQuery<MailingForPageCreation, DropscanMailing> queryMailingForPageCreation,
            ICommand<SetMailingPageCount> setMailingPageCount)
            : base(startJobProcessing, succeedJob, failJob)
        {
            if (addPages == null)
                throw new ArgumentNullException(nameof(addPages));
            if (querySingleDropscanMailing == null)
                throw new ArgumentNullException(nameof(querySingleDropscanMailing));
            if (queryMailingForPageCreation == null)
                throw new ArgumentNullException(nameof(queryMailingForPageCreation));
            if (setMailingPageCount == null)
                throw new ArgumentNullException(nameof(setMailingPageCount));
            _addPages = addPages;
            _queryMailingForPageCreation = queryMailingForPageCreation;
            _setMailingPageCount = setMailingPageCount;
        }

        public void Execute(int dropscanMailingID, int jobID, bool recreateIfExist)
        {
            Execute(
                jobID,
                () =>
                {
                    var dropscanMailing =
                        _queryMailingForPageCreation.Execute(new MailingForPageCreation(dropscanMailingID));
                    if (dropscanMailing == null)
                    {
                        throw new ArgumentException(
                            $"No Dropscan Mailing found with the ID {dropscanMailingID}",
                            nameof(dropscanMailingID));
                    }

                    if (dropscanMailing.Pages == null || dropscanMailing.PageCount == null ||
                        dropscanMailing.Pages.Count != dropscanMailing.PageCount || recreateIfExist)
                    {
                        using (var stream = new MemoryStream(dropscanMailing.FileData.Data))
                        {
                            using (var pdfFile = new PdfFile(stream))
                            {
                                var removeExistingPages = false;
                                var missingPages = Enumerable.Range(1, pdfFile.PageCount);
                                if (dropscanMailing.Pages != null && !recreateIfExist)
                                    missingPages = missingPages.Except(dropscanMailing.Pages.Select(x => x.PageNumber));

                                if (dropscanMailing.Pages == null || recreateIfExist)
                                {
                                    dropscanMailing.Pages = new List<DropscanMailingPage>();
                                    removeExistingPages = true;
                                }

                                dropscanMailing.PageCount = pdfFile.PageCount;

                                _setMailingPageCount.Execute(new SetMailingPageCount(dropscanMailingID, pdfFile.PageCount));

                                foreach (var batch in missingPages.ToBuckets(10))
                                {
                                    var batchPages = new List<DropscanMailingPage>();
                                    foreach (var missingPage in batch.Items)
                                    {
                                        using (var pageDataStream = new MemoryStream())
                                        {
                                            pdfFile.GetPageImage(150, missingPage).Save(pageDataStream, ImageFormat.Png);
                                            batchPages.Add(
                                                new DropscanMailingPage
                                                {
                                                    MailingID = dropscanMailingID,
                                                    PageData = new DocumentData(pageDataStream.ToArray()),
                                                    PageContentType = "image/png",
                                                    PageNumber = missingPage
                                                });
                                        }
                                    }

                                    _addPages.Execute(new AddDropscanMailingPages(batchPages, removeExistingPages));
                                    dropscanMailing.Pages.AddRange(batchPages);
                                }
                            }
                        }
                    }

                    return new PreparePagesFromDropscanMailingResult
                    {
                        MailingID = dropscanMailingID,
                        PageNumbers = dropscanMailing.Pages.Select(x => x.PageNumber).ToArray()
                    };
                });
        }
    }
}