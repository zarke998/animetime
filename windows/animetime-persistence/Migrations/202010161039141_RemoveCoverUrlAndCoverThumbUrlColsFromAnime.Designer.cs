﻿// <auto-generated />
namespace AnimeTime.Persistence.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    [GeneratedCode("EntityFramework.Migrations", "6.4.0")]
    public sealed partial class RemoveCoverUrlAndCoverThumbUrlColsFromAnime : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(RemoveCoverUrlAndCoverThumbUrlColsFromAnime));
        
        string IMigrationMetadata.Id
        {
            get { return "202010161039141_RemoveCoverUrlAndCoverThumbUrlColsFromAnime"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}
