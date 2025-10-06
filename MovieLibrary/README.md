Movie Library 🎬
Aplicación web ASP.NET Core MVC para explorar películas usando la API de TMDB.
Características

Ver películas populares organizadas por año
Buscar películas por título
Filtrar por género
Filtrar por año de lanzamiento
Diseño responsive con Bootstrap

Requisitos previos

.NET 8.0 SDK
Visual Studio 2022 o Visual Studio Code
Cuenta en TMDB para obtener API Key


Restaura las dependencias:

bash   dotnet restore

Configura tu API Key de TMDB:
Crea un archivo appsettings.Development.json en la raíz del proyecto:

appsettings.json:
  "TMDB": {
    "ApiKey": "*****",
    "BaseUrl": "https://api.themoviedb.org/3/"
  }
}

Obtén tu API Key de TMDB:

Regístrate en https://www.themoviedb.org/
Ve a tu perfil → Settings → API
Solicita una API Key (es gratis)
Copia tu API Key y pégala en appsettings.Development.json


Ejecuta la aplicación:

bash   dotnet run
O desde Visual Studio: presiona F5

Abre tu navegador:

   https://localhost:5001
Estructura del proyecto
MovieLibrary/
├── Controllers/
│   └── MovieController.cs
├── Models/
│   └── Movie.cs
├── Services/
│   └── TMDBService.cs
├── Views/
│   ├── Movie/
│   │   ├── Index.cshtml
│   │   └── Search.cshtml
│   └── Shared/
│       └── _MovieCard.cshtml
├── wwwroot/
├── appsettings.json
└── Program.cs




