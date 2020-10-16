namespace AnimeTime.Persistence.Migrations
{
    using AnimeTime.Core.Domain;
    using AnimeTime.Core.Domain.Enums;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AnimeTimeDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AnimeTimeDbContext context)
        {
            SeedImageTypes(context);
            SeedImageOrientations(context);
            SeedCharacterRoles(context);
            SeedImageLodLevels(context);
        }

        private void SeedImageTypes(AnimeTimeDbContext context)
        {
            // Seed image types from enum
            var imageTypes = Enum.GetValues(typeof(ImageTypeId));
            foreach (var type in imageTypes)
            {
                var typeId = (int)type;
                var typeName = Enum.GetName(typeof(ImageTypeId), type);

                context.ImageTypes.AddOrUpdate(new ImageType() { Id = (ImageTypeId)typeId, TypeName = typeName });
            }
        }
        private void SeedImageOrientations(AnimeTimeDbContext context)
        {
            var orientationIds = Enum.GetValues(typeof(ImageOrientationId));

            foreach (var id in orientationIds)
            {
                var orientationName = Enum.GetName(typeof(ImageOrientationId), id);
                context.ImageOrientations.AddOrUpdate(new ImageOrientation() { Id = (ImageOrientationId)id, Name = orientationName });
            }
        }
        private void SeedCharacterRoles(AnimeTimeDbContext context)
        {
            foreach (var role in Enum.GetValues(typeof(CharacterRoleId)))
            {
                var id = (CharacterRoleId)role;
                var name = Enum.GetName(typeof(CharacterRoleId), role);

                context.CharacterRoles.AddOrUpdate(new CharacterRole() { Id = id, RoleName = name });
            }
        }
        private void SeedImageLodLevels(AnimeTimeDbContext context)
        {
            context.ImageLodLevels.AddOrUpdate(
                new ImageLodLevel() 
                {
                    Id = 1,
                    Level = LodLevel.Big,
                    Name = LodLevel.Big.ToString(),
                    MaxWidthLandscape = 800,
                    MaxHeightPortrait = 800,
                    Quality = 0.7F
                },
                new ImageLodLevel() 
                {
                    Id = 2,
                    Level = LodLevel.Medium,
                    Name = LodLevel.Medium.ToString(),
                    MaxWidthLandscape = 500,
                    MaxHeightPortrait = 500,
                    Quality = 0.9F
                },
                new ImageLodLevel()
                {
                    Id = 3,
                    Level = LodLevel.Small,
                    Name = LodLevel.Small.ToString(),
                    MaxWidthLandscape = 200,
                    MaxHeightPortrait = 200,
                    Quality = 1.0F
                });
        }
    }
}