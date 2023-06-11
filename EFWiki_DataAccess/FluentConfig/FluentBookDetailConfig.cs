using EFWiki_Model.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFWiki_DataAccess.FluentConfig
{
    public class FluentBookDetailConfig: IEntityTypeConfiguration<Fluent_BookDetail>
    {
        public void Configure(EntityTypeBuilder<Fluent_BookDetail> modelBuilder)
        {
            //name of the table
            modelBuilder.ToTable("Fluent_BookDetails");
            //name of the columns
            modelBuilder.Property(u => u.NumberOfChapters).HasColumnName("NoOfChapters");
            //primary key
            modelBuilder.Property(u => u.NumberOfChapters).IsRequired();
            //other validatons
            modelBuilder.HasKey(u => u.BookDetail_Id);
            //relations
            modelBuilder.HasOne(b => b.Book).WithOne(b => b.BookDetail)
                .HasForeignKey<Fluent_BookDetail>(u => u.Book_Id);
        }
    }
}
