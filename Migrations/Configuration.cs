namespace AnimeTime.Persistence.Migrations
{
    using AnimeTime.Core.Domain;
    using AnimeTime.Core.Domain.Enums;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AnimeTime.Persistence.AnimeTimeDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AnimeTime.Persistence.AnimeTimeDbContext context)
        {
            SeedImageTypes(context);
            SeedImageOrientations(context);
            SeedCharacterRoles(context);
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

            foreach(var id in orientationIds)
            {
                var orientationName = Enum.GetName(typeof(ImageOrientationId), id);
                context.ImageOrientations.AddOrUpdate(new ImageOrientation() { Id = (ImageOrientationId)id, Name = orientationName });
            }
        }
        private void SeedCharacterRoles(AnimeTimeDbContext context)
        {
            foreach(var role in Enum.GetValues(typeof(CharacterRoleId)))
            {
                var id = (CharacterRoleId)role;
                var name = Enum.GetName(typeof(CharacterRoleId), role);

                context.CharacterRoles.AddOrUpdate(new CharacterRole() { Id = id, RoleName = name });
            }
        }
    }
}