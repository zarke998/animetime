﻿// <auto-generated />
namespace AnimeTime.Persistence.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    [GeneratedCode("EntityFramework.Migrations", "6.4.4")]
    public sealed partial class SetAnimeSourceUrlToNullable : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(SetAnimeSourceUrlToNullable));
        
        string IMigrationMetadata.Id
        {
            get { return "202208042316567_SetAnimeSourceUrlToNullable"; }
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