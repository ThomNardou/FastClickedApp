// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//////////////////////////////////////////////////////////////// DECLARATION DES VARIABLES ////////////////////////////////////////////////////////////////
let Timer = 30;
let score = 0;
let frame = 0;

let pseudo = "";

let gameStarted = false;

let clickButton = document.getElementById("clickButton");
let startButton = document.getElementById("startButton");
let nicknameLabel = document.getElementById("nickname");
let scoreLabel = document.getElementById("score");
let timeLabel = document.getElementById("time");

/*
 * Moteur du jeu 
 */
const move = () => {

    // Regarde si la partie à commencé 
    if (gameStarted) {
        // Regarde si le timer du jeu n'est pas arrivé à 0'
        if (Timer > 0) {
            // Déincrémente de 1 le timer du jeu
            Timer -= 1;
        }
        // Si la partie s'est terminé 
        else {
            // Met à jour le boolean pour terminé la partie 
            gameStarted = false;

            // Stop la musique de jeu 
            const audio = document.querySelector("audio");
            audio.pause()
            audio.currentTime = 0;

            // Appel d'une fonction c# pour inserer le score du joueur 
            $.ajax({
                type: "POST",
                url: "/Home/InsertPlayerScore",
                data: {
                    playerName: pseudo,
                    playerScore: score
                }
            });

            // Reset le score du joueur 
            score = 0;
            pseudo = "User"

            // Remet en place le bouton pour commencer la partie 
            startButton.style.display = "block";
            clickButton.style.display = "none";

            // Reset le timer
            Timer = 30
        }
    }
    // Si la partie n'a pas commencé '
    else {
        startButton.style.display = "block";
        clickButton.style.display = "none";
        Timer = 30
    }

    // met à jour le scoreboard avec les valeur actuel 
    timeLabel.innerHTML = "Remaining Time : " + Timer;
    scoreLabel.innerHTML = "Score : " + score;
}

setInterval(move, 1000);

// Load les particules de la page d'accueil
particlesJS.load('particles-js', 'assets/particles.json', function () {
    console.log('callback - particles-js config loaded');
});

/*
 * Fonction qui demande le pseudo du joueur 
 */
function startGame() {

    // Afficher un prompt sur le navigateur
    pseudo = prompt("What is you player name ? ", "User");

    // Regarde si le pseudo a bien été remplis 
    if (pseudo != null) {
        // Appel la fonction pour démarer la musique 
        startBGMusic();

        // modifie le scoreboard pour afficher le pseudo du joueur 
        nicknameLabel.innerHTML = "Nickname : " + pseudo

        // Modifie le boolean pour lancer la partie 
        gameStarted = true;

        // Fais dsparraitre le bouton de départ pour laisser place au bouton du jeu
        startButton.style.display = "none";
        clickButton.style.display = "block";
    }

}
/*
 * Fonction qui met à jour le score et déplace le bouton de jeu
 */
function upgradeScore() {
    // Vérifie si la partie s'est bien lancé '
    if (gameStarted) {
        // Incrémente de 1 le score du joueur 
        score += 1;

        // met à jour le scoreboard avec le nouveau score 
        scoreLabel.innerHTML = "Score : " + score;

        // Génére un position aléatoire pour le bouton de jeu 
        var i = Math.floor(Math.random() * 420);
        var j = Math.floor(Math.random() * 420);

        // Fait bouger le bouton du jeu
        clickButton.style.transition = "0.15s"
        clickButton.style.left = i + "px";
        clickButton.style.top = j + "px";
    }
}

/*
 * Fonction qui change le style du bouton "Play" de la page d'accueil
 */
function changeContent(isHovered) {
    let button = document.getElementById("playButton");

    // Regarde si la souris de l'tilisateur passe sur le bouton
    if (isHovered) {
        // Change le contenu du bouton
        button.innerHTML = "Play"
    }
    else {
        // Change le contenu du bouton
        button.innerHTML = "▶"
    }
    button.style.transition = 0.35;
}

/*
 * Fonction qui permet de lancer la musique de fond
 */
function startBGMusic() {
    const audio = document.querySelector("audio");
    audio.volume = 0.2;
    audio.play();
}