using FastClieckWebAppMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using Docker.DotNet;


namespace FastClieckWebAppMVC.Controllers
{
    public class HomeController : Controller
    {
        public MySqlConnection connection;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(SelectPlayer());
        }

        [HttpPost]
        public void ClieckEvent()
        {
            Console.WriteLine("Cliqué !!!!");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public void ClosConnection()
        {
            // Ferme la connexion
            connection.Close();
        }

        /// <summary>
        /// Fonction qui va inseret le nouveau joueur dans la base donnée 
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="playerScore"></param>
        [HttpPost]
        public void InsertPlayerScore(string playerName, int playerScore)
        {
            // Valeurs de connection de la base de donnée
            string srv_addr = "192.168.0.10";                          // Adresse du  serveur 
            string dbname = "db_clickGame";                          // nom de la base de donnée
            string uid = "root";                                     // Utilisateur
            string pass = "root";                                    // Mot de passe
            string port = "3306";                                    // Port 

            // Chaine de connexion permettant de de se connecter à la base de donnée
            string connectStr = "SERVER=" + srv_addr + ";" + "PORT=" + port + ";" + "DATABASE=" + dbname + ";" + "UID=" + uid + ";" + "PASSWORD=" + pass + ";";

            // attribue la chaine de connexion 
            connection = new MySqlConnection(connectStr);

            // Ouvre la connexion à la base de donnée
            connection.Open();

            // Requête SQL pour insérer des valeurs dans la table `t_joueur`.
            string insertQuery = $"insert into t_joueur (name, score) values ('{playerName}', {playerScore});";

            // Crée la commande avec la requête d'insertion.
            MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection);

            // Exécute la requête
            MySqlDataReader insertDataReader = insertCommand.ExecuteReader();

            insertDataReader.Read();

            // Ferme la connexion à la base de données.
            connection.Close();
        }

        /// <summary>
        /// Fonction qui va chercher les premier joueur de la base donnée
        /// </summary>
        /// <returns>La liste des joueurs</returns>
        public List<Player> SelectPlayer()
        {
            //getContainerIp();

            int place = 1;

            // Valeurs de connection de la base de donnée
            string srv_addr = "192.168.0.10";                          // Adresse du  serveur 
            string dbname = "db_clickGame";                          // nom de la base de donnée
            string uid = "root";                                     // Utilisateur
            string pass = "root";                                    // Mot de passe
            string port = "3306";                                    // Port 

            // Chaine de connexion permettant de de se connecter à la base de donnée
            string connectStr = "SERVER=" + srv_addr + ";" + "DATABASE=" + dbname + ";" + "UID=" + uid + ";" + "PASSWORD=" + pass + ";" + "PORT=" + port;

            // attribue la chaine de connexion 
            connection = new MySqlConnection(connectStr);

            // Ouvre la connexion à la base de données.
            connection.Open();

            // Crée une liste pour stocker les joueurs.
            List<Player> playerList = new List<Player>();

            // Requête SQL pour sélectionner les 10 meilleurs joueurs par score, en ordre décroissant.
            string selectQuery = "select * from t_joueur order by score desc limit 10;";

            MySqlCommand cmd = new MySqlCommand(selectQuery, connection);

            // Exécute la requête 
            MySqlDataReader reader = cmd.ExecuteReader();

            // Parcourt chaque ligne des résultats obtenu.
            while (reader.Read())
            {
                // Crée un nouveau joueur 
                Player player = new Player();

                // Récupère les valeurs des colonnes et les assigne au joueur.
                player.Id = Convert.ToInt32(reader["id"]);
                player.Place = place;
                player.NickName = reader["name"].ToString();
                player.Score = Convert.ToInt32(reader["score"]);

                // Ajoute le joueur à la liste des joueurs.
                playerList.Add(player);

                // Incrémente la place pour le prochain joueur.
                place++;
            }

            // Ferme la connexion à la base de données.
            connection.Close();

            // Renvoie la liste des joueurs
            return playerList;
        }

        static async void getContainerIp()
        {
            var dockerClient = new DockerClientConfiguration(new Uri("http://ubuntu-docker.cloudapp.net:4243")).CreateClient();

            var containers = await dockerClient.Containers.InspectContainerAsync("1903e9c65613846836d4977d3bdd476f0d69c7bb8e657f1f3f96f6582f79e10f");


            var inspectResult = await dockerClient.Containers.InspectContainerAsync(containers.ID);
            var ipAddress = inspectResult.NetworkSettings.Networks["bridge"].IPAddress;
            Console.WriteLine($"MySQL Container IP Address: {ipAddress}");
            
            //_ipAddress = ipAddress;

        }
    }
}