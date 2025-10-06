namespace MovieLibrary.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Overview { get; set; }
        public string PosterPath { get; set; }
        public string ReleaseDate { get; set; }
        public List<int> GenreIds { get; set; }  // Para filtrar por categorías


        public List<string> GenreNames { get; set; } = new List<string>();
    }

    public class MoviesViewModel
    {
        public List<Movie> Movies { get; set; } = new List<Movie>();
        public Dictionary<int, string> Genres { get; set; } = new Dictionary<int, string>();
        public string Query { get; set; } = "";
        public int? Year { get; set; }
        public int? SelectedGenreId { get; set; }
    }

}
