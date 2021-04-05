using HAF.Domain.Entities;
using System.Data.Entity;

namespace  HAF.DAL
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
            : base("SimpleDMSConnectionString")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        //public DbSet<BackgroundJob> BackgroundJobs { get; set; }
        //public DbSet<Company> Companies { get; set; }
        //public DbSet<DebitorCreditor> DebitorCreditors { get; set; }
        //public DbSet<DiscardedDropscanMailingPage> DiscardedDropscanMailingPages { get; set; }
        public DbSet<DocumentFlag> DocumentFlags { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Asset> Asset { get; set; }
        //public DbSet<DropscanMailingPage> DropscanMailingPages { get; set; }
        //public DbSet<DropscanMailing> DropscanMailings { get; set; }
        //public DbSet<DropscanRecipient> DropscanRecipients { get; set; }
        //public DbSet<Folder> Folders { get; set; }
        //public DbSet<MappedDropscanMailingPage> MappedDropscanMailingPages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<DebitorCreditor>()
            //    .HasRequired(x => x.RootFolder)
            //    .WithMany()
            //    .HasForeignKey(x => x.RootFolderID);

            //modelBuilder.Entity<DebitorCreditor>()
            //    .HasRequired(x => x.Company)
            //    .WithMany(x => x.DebitorCreditors)
            //    .HasForeignKey(x => x.CompanyID);

            //modelBuilder.Entity<DocumentFlag>()
            //    .HasRequired(x => x.Company)
            //    .WithMany(x => x.AllowedDocumentFlags)
            //    .HasForeignKey(x => x.CompanyID);

            modelBuilder.Entity<Document>().HasOptional(x => x.Folder).WithMany(x => x.Files).HasForeignKey(x => x.FolderID);
            modelBuilder.Entity<Document>()
                .HasOptional(x => x.DebitorCreditor)
                .WithMany()
                .HasForeignKey(x => x.DebitorCreditorID);
            modelBuilder.Entity<Document>().HasOptional(x => x.DocumentData).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Document>().Property(x => x.DocumentDataContentType).IsRequired();
            modelBuilder.Entity<Document>().Property(x => x.DocumentFileExtension).IsRequired();

            modelBuilder.Entity<Document>()
                .HasMany(x => x.AssignedFlags)
                .WithMany()
                .Map(x => x.ToTable("Documents2Flags").MapLeftKey("DocumentID").MapRightKey("DocumentFlagID"));

            //modelBuilder.Entity<Folder>()
            //    .HasMany(x => x.Folders)
            //    .WithOptional(x => x.ParentFolder)
            //    .HasForeignKey(x => x.ParentFolderID);

            //modelBuilder.Entity<DropscanMailing>()
            //    .HasRequired(x => x.Recipient)
            //    .WithMany()
            //    .HasForeignKey(x => x.RecipientID);
            //modelBuilder.Entity<DropscanMailing>()
            //    .HasMany(x => x.Pages)
            //    .WithRequired(x => x.Mailing)
            //    .HasForeignKey(x => x.MailingID);
            //modelBuilder.Entity<DropscanMailing>().HasRequired(x => x.FileData).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<DropscanMailing>().HasRequired(x => x.EnvelopeData).WithMany().WillCascadeOnDelete(false);

            //modelBuilder.Entity<DropscanRecipient>()
            //    .HasOptional(x => x.CorrespondingCompany)
            //    .WithMany()
            //    .HasForeignKey(x => x.CorrespondingCompanyID);

            //modelBuilder.Entity<DropscanMailingPage>()
            //    .Property(x => x.MailingID)
            //    .HasUniqueIndexAnnotation("IX_MailingPageNumber", 0);
            //modelBuilder.Entity<DropscanMailingPage>()
            //    .Property(x => x.PageNumber)
            //    .HasUniqueIndexAnnotation("IX_MailingPageNumber", 1);
            //modelBuilder.Entity<DropscanMailingPage>().HasRequired(x => x.PageData).WithMany().WillCascadeOnDelete(false);

            //modelBuilder.Entity<BackgroundJob>().Ignore(x => x.ResultType);

            //modelBuilder.Entity<DiscardedDropscanMailingPage>()
            //    .HasRequired(x => x.Mailing)
            //    .WithMany(x => x.DiscardedPages)
            //    .HasForeignKey(x => x.MailingID);

            //modelBuilder.Entity<DiscardedDropscanMailingPage>()
            //    .Property(x => x.MailingID)
            //    .HasUniqueIndexAnnotation("IX_DiscardedMailingPageNumber", 0);
            //modelBuilder.Entity<DiscardedDropscanMailingPage>()
            //    .Property(x => x.PageNumber)
            //    .HasUniqueIndexAnnotation("IX_DiscardedMailingPageNumber", 1);

            //modelBuilder.Entity<MappedDropscanMailingPage>()
            //    .HasRequired(x => x.Mailing)
            //    .WithMany(x => x.MappedPages)
            //    .HasForeignKey(x => x.MailingID);
            //modelBuilder.Entity<MappedDropscanMailingPage>()
            //    .HasRequired(x => x.Document)
            //    .WithMany()
            //    .HasForeignKey(x => x.DocumentID);

            //modelBuilder.Entity<MappedDropscanMailingPage>()
            //    .Property(x => x.MailingID)
            //    .HasUniqueIndexAnnotation("IX_MappedMailingPageNumber", 0);
            //modelBuilder.Entity<MappedDropscanMailingPage>()
            //    .Property(x => x.PageNumber)
            //    .HasUniqueIndexAnnotation("IX_MappedMailingPageNumber", 1);

            //modelBuilder.Entity<DocumentData>().ToTable("DocumentData");
        }
    }
}