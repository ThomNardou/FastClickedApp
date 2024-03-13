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
            return View(selectPlayers());
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

        [HttpPost]
        public void insertPlayerScore(string playerName, int playerScore)
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
            connection.Open();

            string insertQuery = $"insert into t_joueur (name, score) values ('{playerName}', {playerScore});";

            MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection);

            MySqlDataReader insertDataReader = insertCommand.ExecuteReader();

            insertDataReader.Read();

            connection.Close();
        }

        public List<Player> selectPlayers()
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
            Console.WriteLine("==================================================================================\n" + connection.ConnectionString + "\n==================================================================================");
            connection.Open();


            List<Player> playerList = new List<Player>();

            string selectQuery = "select * from t_joueur order by score desc limit 10;";
            MySqlCommand cmd = new MySqlCommand(selectQuery, connection);

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Player player = new Player();

                player.Id = Convert.ToInt32(reader["id"]);
                player.Place = place;
                player.NickName = reader["name"].ToString();
                player.Score = Convert.ToInt32(reader["score"]);

                playerList.Add(player);

                place++;
            }

            connection.Close();

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