namespace AnimeTime.Persistence.Migrations
{
    using AnimeTime.Core.Domain;
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
            // Seed image types from enum
            var imageTypes = Enum.GetValues(typeof(ImageTypeId));
            foreach (var type in imageTypes)
            {
                var typeId = (int)type;
                var typeName = Enum.GetName(typeof(ImageTypeId), type);

                context.ImageTypes.AddOrUpdate(new ImageType() { Id = typeId, TypeName = typeName });
            }
        }
    }
}
