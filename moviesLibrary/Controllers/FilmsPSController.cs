using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using moviesLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace moviesLibrary.Controllers
{
    public class FilmsPSController : Controller
    {
        private string connectionString;
        public IConfiguration configuration;

        //GET: StoredProc
        public FilmsPSController(IConfiguration __configuration)
        {
            configuration = __configuration;
        }

        //procedure:getListeFilms
        // GET: Home
        public ActionResult Index()
        {
            SqlCommand cmd;
            SqlDataReader reader;
            List<Film> listeFilms;

            connectionString = configuration.GetConnectionString("moviesLibraryContext");

            SqlConnection conn = new SqlConnection(connectionString);
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GetAllFilms";
            cmd.Connection = conn;

            conn.Open();
            reader = cmd.ExecuteReader();
            listeFilms = new List<Film>();

            while (reader.Read())
            {
                Film film = new Film();
                film.Titre = reader.GetString("titre");
                film.Genre = reader.GetString("genre");
                film.Realisateur = reader.GetString("Realisateur");
                film.AnneeSortie = reader.GetInt32("AnneeSortie");
                film.Acteur = reader.GetString("acteur");

                listeFilms.Add(film);
            }

            return View(listeFilms);
        }

        //procedure:detailsFilms(@title)
        // GET:
        public IActionResult Details(string titre)
        {
            SqlConnection conn;
            SqlCommand cmd;
            SqlDataReader reader;
            Film film = null;

            connectionString = configuration.GetConnectionString("moviesLibraryContext");

            conn = new SqlConnection(connectionString);
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "detailsFilm";
            cmd.Parameters.Add(new SqlParameter("@titre",titre));
            cmd.Connection = conn;

            conn.Open();
            reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    film = new Film();
                    film.Titre = reader.GetString("titre");
                    film.Genre = reader.GetString("genre");
                    film.Realisateur = reader.GetString("Realisateur");
                    film.AnneeSortie = reader.GetInt32("AnneeSortie");
                    film.Acteur = reader.GetString("acteur");
                }
                return View(film);
            }
            return RedirectToAction(nameof(Index));
        }

        //Aucune procedure: affiche un formulaire vide pour saisie d'un nouveau film
        public IActionResult Create()
        {
            Film m = new Film();
            return View(m);
        }

        //procedure:creerFilm(@title, @genre, ...)
        // POST: Home/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Film m)
        {
            try
            {
                SqlConnection conn;
                SqlCommand cmd;

                connectionString = configuration.GetConnectionString("moviesLibraryContext");

                conn = new SqlConnection(connectionString);
                cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "CreateFilm";
                cmd.Parameters.Add(new SqlParameter("@titre", m.Titre));
                cmd.Parameters.Add(new SqlParameter("@genre", m.Genre));
                cmd.Parameters.Add(new SqlParameter("@realisateur", m.Realisateur));
                cmd.Parameters.Add(new SqlParameter("@anneeSortie", m.AnneeSortie));
                cmd.Parameters.Add(new SqlParameter("@acteur", m.Acteur));

                cmd.Connection = conn;
                conn.Open();

                int rowCount = cmd.ExecuteNonQuery();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        //Appel de procedure detailsFilms(@title)
        // GET: Home/Edit/5
        public IActionResult Edit(string titre)
        {
            return View();
        }

        // POST: StoredProc/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string titre, Film m)
        {
            try
            {
                SqlConnection conn;
                SqlCommand cmd;

                connectionString = configuration.GetConnectionString("moviesLibraryContext");

                conn = new SqlConnection(connectionString);
                cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "UpdateFilm";
                cmd.Parameters.Add(new SqlParameter("@titre", m.Titre));
                cmd.Parameters.Add(new SqlParameter("@genre", m.Genre));
                cmd.Parameters.Add(new SqlParameter("@realisateur", m.Realisateur));
                cmd.Parameters.Add(new SqlParameter("@anneeSortie", m.AnneeSortie));
                cmd.Parameters.Add(new SqlParameter("@acteur", m.Acteur));

                cmd.Connection = conn;
                conn.Open();

                int rowCount = cmd.ExecuteNonQuery();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StoredProc/Delete/5
        public IActionResult Delete(string titre)
        {
            SqlConnection conn;
            SqlCommand cmd;
            SqlDataReader reader;
            Film film = null;

            connectionString = configuration.GetConnectionString("moviesLibraryContext");

            conn = new SqlConnection(connectionString);
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "detailsFilm";
            cmd.Parameters.Add(new SqlParameter("@titre", titre));
            cmd.Connection = conn;

            conn.Open();
            reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    film = new Film();
                    film.Titre = reader.GetString("titre");
                    film.Genre = reader.GetString("genre");
                    film.Realisateur = reader.GetString("Realisateur");
                    film.AnneeSortie = reader.GetInt32("AnneeSortie");
                    film.Acteur = reader.GetString("acteur");
                }
                return View(film);
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: StoredProc/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string titre, Film m)
        {

            SqlConnection conn;
            SqlCommand cmd;

            connectionString = configuration.GetConnectionString("moviesLibraryContext");

            conn = new SqlConnection(connectionString);
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "DeleteFilm";
            cmd.Parameters.Add(new SqlParameter("@titre", titre));

            cmd.Connection = conn;
            conn.Open();

            int rowCount = cmd.ExecuteNonQuery();
            return RedirectToAction(nameof(Index));

        }
        // New method to search films by genre
        public IActionResult SearchByGenre(string genre)
        {
            SqlConnection conn;
            SqlCommand cmd;
            SqlDataReader reader;
            List<Film> listeFilms = new List<Film>();

            connectionString = configuration.GetConnectionString("moviesLibraryContext");

            conn = new SqlConnection(connectionString);
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SearchFilmsByGenre";
            cmd.Parameters.Add(new SqlParameter("@genre", genre));
            cmd.Connection = conn;

            conn.Open();
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Film film = new Film();
                film.Titre = reader.GetString("titre");
                film.Genre = reader.GetString("genre");
                film.Realisateur = reader.GetString("Realisateur");
                film.AnneeSortie = reader.GetInt32("AnneeSortie");
                film.Acteur = reader.GetString("acteur");

                listeFilms.Add(film);
            }

            return View(listeFilms);
        }
        public IActionResult SearchByActor(string acteur)
        {
            SqlConnection conn;
            SqlCommand cmd;
            SqlDataReader reader;
            List<Film> listeFilms = new List<Film>();

            connectionString = configuration.GetConnectionString("moviesLibraryContext");

            conn = new SqlConnection(connectionString);
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SearchFilmsByActor";
            cmd.Parameters.Add(new SqlParameter("@acteur", acteur));
            cmd.Connection = conn;

            conn.Open();
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Film film = new Film();
                film.Titre = reader.GetString("titre");
                film.Genre = reader.GetString("genre");
                film.Realisateur = reader.GetString("Realisateur");
                film.AnneeSortie = reader.GetInt32("AnneeSortie");
                film.Acteur = reader.GetString("acteur");

                listeFilms.Add(film);
            }

            return View(listeFilms);
        }
        // Méthode pour trier les films par titre 
        public IActionResult SortByTitle()
        {
            SqlConnection conn;
            SqlCommand cmd;
            SqlDataReader reader;
            List<Film> listeFilms = new List<Film>();

            connectionString = configuration.GetConnectionString("moviesLibraryContext");

            conn = new SqlConnection(connectionString);
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SortFilmsByTitle";
            cmd.Connection = conn;

            conn.Open();
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Film film = new Film();
                film.Titre = reader.GetString("titre");
                film.Genre = reader.GetString("genre");
                film.Realisateur = reader.GetString("Realisateur");
                film.AnneeSortie = reader.GetInt32("AnneeSortie");
                film.Acteur = reader.GetString("acteur");

                listeFilms.Add(film);
            }

            return View(listeFilms);
        }
    }
}
