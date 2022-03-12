namespace AnimeTime.Services.DTO
{
    public class AnimeSearchDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public float Rating { get; set; }
        public int? ReleaseYear { get; set; }
        public string CoverImageUrl { get; set; }
    }
}
