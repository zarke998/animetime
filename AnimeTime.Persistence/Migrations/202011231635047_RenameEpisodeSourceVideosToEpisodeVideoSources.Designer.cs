﻿// <auto-generated />
namespace AnimeTime.Persistence.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    [GeneratedCode("EntityFramework.Migrations", "6.4.4")]
    public sealed partial class RenameEpisodeSourceVideosToEpisodeVideoSources : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(RenameEpisodeSourceVideosToEpisodeVideoSources));
        
        string IMigrationMetadata.Id
        {
            get { return "202011231635047_RenameEpisodeSourceVideosToEpisodeVideoSources"; }
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
