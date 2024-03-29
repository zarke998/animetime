﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using AnimeTime.Core.Domain;

namespace AnimeTime.Persistence.EntityConfigurations
{
    public class AnimeConfiguration : EntityTypeConfiguration<Anime>
    {
        public AnimeConfiguration()
        {
            HasOptional(e => e.Metadata).WithRequired(m => m.Anime);
            HasOptional(e => e.Status).WithMany(s => s.Animes).HasForeignKey(e => e.Status_Id).WillCascadeOnDelete(false);
        }
    }
}
