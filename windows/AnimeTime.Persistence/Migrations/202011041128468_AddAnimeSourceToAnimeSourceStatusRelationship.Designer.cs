﻿// <auto-generated />
namespace AnimeTime.Persistence.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    [GeneratedCode("EntityFramework.Migrations", "6.4.4")]
    public sealed partial class AddAnimeSourceToAnimeSourceStatusRelationship : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(AddAnimeSourceToAnimeSourceStatusRelationship));
        
        string IMigrationMetadata.Id
        {
            get { return "202011041128468_AddAnimeSourceToAnimeSourceStatusRelationship"; }
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