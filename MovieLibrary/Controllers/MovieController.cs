using Microsoft.AspNetCore.Mvc;
using MovieLibrary.Models;
using MovieLibrary.Services;

namespace MovieLibrary.Controllers
{
    public class MovieController : Controller
    {
        private readonly TMDBService _tmdbService;

        public MovieController(TMDBService tmdbService)
        {
            _tmdbService = tmdbService;
        }

        // Index: películas populares  GET: /Movie/Index
        public async Task<IActionResult> Index()
        {
            var movies = await _tmdbService.GetAllMoviesAsync(); // Cargará 10 páginas por defecto

            var genres = await _tmdbService.GetGenresAsync();
            foreach (var movie in movies)
            {
                movie.GenreNames = movie.GenreIds
                    .Where(id => genres.ContainsKey(id))
                    .Select(id => genres[id])
                    .ToList();
            }
            return View(movies);
        }


        // Search: búsqueda por título  GET: /Movie/Search
        public async Task<IActionResult> Search(string query, int? genreId, int? year)
        {
            // Siempre empezamos con todas las películas
            List<Movie> movies = await _tmdbService.GetPopularMoviesAsync();

            // Filtrar por query si existe
            if (!string.IsNullOrWhiteSpace(query))
            {
                movies = await _tmdbService.SearchMoviesAsync(query);
            }

            // Filtrar por género si existe
            if (genreId.HasValue)
            {
                movies = movies.Where(m => m.GenreIds.Contains(genreId.Value)).ToList();
            }

            // Filtrar por año si existe
            if (year.HasValue)
            {
                // Definir rango de fechas que cubra todo el año
                string startDate = $"{year}-01-01";
                string endDate = $"{year}-12-31";

                // Llamar al método que usa rango de fechas
                movies = await _tmdbService.GetMoviesByDateAsync(startDate, endDate);
            }

            // Obtener géneros
            var genres = await _tmdbService.GetGenresAsync();

            // Mapear nombres de géneros a cada película
            foreach (var movie in movies)
            {
                movie.GenreNames = movie.GenreIds
                    .Where(id => genres.ContainsKey(id))
                    .Select(id => genres[id])
                    .ToList();
            }

            // Crear ViewModel
            var vm = new MoviesViewModel
            {
                Movies = movies,
                Genres = genres,
                Query = query,
                Year = year,
                SelectedGenreId = genreId
            };


            return View(vm);
        }

        // Details: detalles de película  /Movie/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            ViewData["Message"] = "Hola, esta es la página de detalles de la película.";

            return View(); // Retorna Details.cshtml
        }


    }
}
