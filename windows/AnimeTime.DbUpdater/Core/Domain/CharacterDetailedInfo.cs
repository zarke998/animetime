namespace AnimeTimeDbUpdater.Core.Domain
{
    public class CharacterDetailedInfo
    {
        public CharacterBasicInfo BasicInfo { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}