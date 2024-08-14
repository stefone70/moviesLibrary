namespace moviesLibrary.Models
{
    public class Film
    {
        public int Id { get; set; }
        public string Titre { get; set; }
        public string Genre { get; set; }
        public string Realisateur { get; set; }
        public int AnneeSortie { get; set; }
        public string Acteur { get; set; }
    }
}
