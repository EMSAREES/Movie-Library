using Microsoft.Extensions.Configuration;
using MovieLibrary.Models;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MovieLibrary.Services
{
    public class TMDBService
    {
        private readonly string apiKey;
        private readonly string baseUrl;
        private readonly HttpClient client;

        public TMDBService(IConfiguration configuration)
        {
            apiKey = configuration["TMDB:ApiKey"];
            baseUrl = configuration["TMDB:BaseUrl"];
            client = new HttpClient();
        }

        // obtener toda las peliculas
        public async Task<List<Movie>> GetAllMoviesAsync(int maxPages = 10)
        {
            var movies = new List<Movie>();
            int page = 1;

            while (page <= maxPages)
            {
                var response = await client.GetStringAsync(
                    $"{baseUrl}discover/movie?api_key={apiKey}&language=es-ES&sort_by=popularity.desc&page={page}"
                );

                var jsonDoc = JsonDocument.Parse(response);

                foreach (var item in jsonDoc.RootElement.GetProperty("results").EnumerateArray())
                {
                    movies.Add(new Movie
                    {
                        Id = item.GetProperty("id").GetInt32(),
                        Title = item.GetProperty("title").GetString(),
                        Overview = item.GetProperty("overview").GetString(),
                        PosterPath = "https://image.tmdb.org/t/p/w500" + item.GetProperty("poster_path").GetString(),
                        ReleaseDate = item.GetProperty("release_date").GetString(),
                        GenreIds = item.GetProperty("genre_ids").EnumerateArray().Select(x => x.GetInt32()).ToList()
                    });
                }

                page++;
            }

            return movies;
        }




        // Obtener películas populares
        public async Task<List<Movie>> GetPopularMoviesAsync()
        {
            var movies = new List<Movie>();

            var response = await client.GetStringAsync($"{baseUrl}movie/popular?api_key={apiKey}&language=es-ES&page=1");
            var jsonDoc = JsonDocument.Parse(response);

            foreach (var item in jsonDoc.RootElement.GetProperty("results").EnumerateArray())
            {
                movies.Add(new Movie
                {
                    Id = item.GetProperty("id").GetInt32(),
                    Title = item.GetProperty("title").GetString(),
                    Overview = item.GetProperty("overview").GetString(),
                    PosterPath = "https://image.tmdb.org/t/p/w500" + item.GetProperty("poster_path").GetString(),
                    ReleaseDate = item.GetProperty("release_date").GetString(),
                    GenreIds = item.GetProperty("genre_ids").EnumerateArray().Select(x => x.GetInt32()).ToList()
                });
            }

            return movies;
        }


        // Buscar películas por nombre
        public async Task<List<Movie>> SearchMoviesAsync(string query)
        {
            var response = await client.GetStringAsync($"{baseUrl}search/movie?api_key={apiKey}&language=es-ES&query={query}");
            var jsonDoc = JsonDocument.Parse(response);
            var movies = new List<Movie>();

            foreach (var item in jsonDoc.RootElement.GetProperty("results").EnumerateArray())
            {
                movies.Add(new Movie
                {
                    Id = item.GetProperty("id").GetInt32(),
                    Title = item.GetProperty("title").GetString(),
                    Overview = item.GetProperty("overview").GetString(),
                    PosterPath = "https://image.tmdb.org/t/p/w500" + item.GetProperty("poster_path").GetString(),
                    ReleaseDate = item.GetProperty("release_date").GetString(),
                    GenreIds = item.GetProperty("genre_ids").EnumerateArray().Select(x => x.GetInt32()).ToList()
                });
            }
            return movies;
        }

        // Filtrar películas por categoría
        public async Task<List<Movie>> GetMoviesByGenreAsync(int genreId)
        {
            var response = await client.GetStringAsync($"{baseUrl}discover/movie?api_key={apiKey}&with_genres={genreId}&language=es-ES");
            var jsonDoc = JsonDocument.Parse(response);
            var movies = new List<Movie>();

            foreach (var item in jsonDoc.RootElement.GetProperty("results").EnumerateArray())
            {
                movies.Add(new Movie
                {
                    Id = item.GetProperty("id").GetInt32(),
                    Title = item.GetProperty("title").GetString(),
                    Overview = item.GetProperty("overview").GetString(),
                    PosterPath = "https://image.tmdb.org/t/p/w500" + item.GetProperty("poster_path").GetString(),
                    ReleaseDate = item.GetProperty("release_date").GetString(),
                    GenreIds = item.GetProperty("genre_ids").EnumerateArray().Select(x => x.GetInt32()).ToList()
                });
            }
            return movies;
        }

        // Filtrar películas por una sola fecha
        public async Task<List<Movie>> GetMoviesByDateAsync(string startDate, string endDate)
        {
            var response = await client.GetStringAsync(
                $"{baseUrl}discover/movie?api_key={apiKey}&primary_release_date.gte={startDate}&primary_release_date.lte={endDate}&language=es-ES"
            );

            var jsonDoc = JsonDocument.Parse(response);
            var movies = new List<Movie>();

            foreach (var item in jsonDoc.RootElement.GetProperty("results").EnumerateArray())
            {
                movies.Add(new Movie
                {
                    Id = item.GetProperty("id").GetInt32(),
                    Title = item.GetProperty("title").GetString(),
                    Overview = item.GetProperty("overview").GetString(),
                    PosterPath = "https://image.tmdb.org/t/p/w500" + item.GetProperty("poster_path").GetString(),
                    ReleaseDate = item.GetProperty("release_date").GetString(),
                    GenreIds = item.GetProperty("genre_ids").EnumerateArray().Select(x => x.GetInt32()).ToList()
                });
            }
            return movies;
        }


        internal async Task<string?> GetMovieDetailsAsync(int id)
        {
            throw new NotImplementedException();
        }

        // obtener po géneros de película
        public async Task<Dictionary<int, string>> GetGenresAsync()
        {
            var response = await client.GetStringAsync($"{baseUrl}genre/movie/list?api_key={apiKey}&language=es-ES");
            var jsonDoc = JsonDocument.Parse(response);
            var genres = new Dictionary<int, string>();

            foreach (var item in jsonDoc.RootElement.GetProperty("genres").EnumerateArray())
            {
                int id = item.GetProperty("id").GetInt32();
                string name = item.GetProperty("name").GetString();
                genres.Add(id, name);
            }

            return genres;
        }

    }
}
