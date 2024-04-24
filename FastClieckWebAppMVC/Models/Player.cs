namespace FastClieckWebAppMVC.Models
{
    public class Player
    {
        // ID du joueur
        public int Id { get; set; }

        // place du joueur parmis tous le joueur du jeu
        public int Place { get; set; }

        // Pseudo du joueur 
        public string NickName { get; set; }

        // Score du joueur 
        public int Score { get; set; }
    }
}
