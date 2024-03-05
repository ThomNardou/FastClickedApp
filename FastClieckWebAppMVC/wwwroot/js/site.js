// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let Timer = 30;
let score = 0;
let frame = 0;

let pseudo = "";

let gameStarted = false;

let clickButton = document.getElementById("clickButton");
let startButton = document.getElementById("startButton");
let nicknameLabel = document.getElementById("nickname");
let scoreLabel = document.getElementById("score");

const move = () => {
    

    if (frame % 200 == 0) {
        if (gameStarted) {
            if (Timer > 0) {
                Timer -= 1;
            }
            else {
                gameStarted = false;

                const audio = document.querySelector("audio");
                audio.pause()
                audio.currentTime = 0;

                $.ajax({
                    type: "POST",
                    url: "/Home/insertPlayerScore",
                    data: {
                        playerName: pseudo,
                        playerScore: score
                    }
                });

                score = 0;
                pseudo = "User"
            }
        }
        else {
            startButton.style.display = "block";
            clickButton.style.display = "none";
            Timer = 30
        }
    }


    scoreLabel.innerHTML = "Score : " + score;

    frame += 1;
    console.log(Timer);
}

setInterval(move, 1);

function startGame() {

    pseudo = prompt("What is you player name ? ", "User");

    startBGMusic();
    nicknameLabel.innerHTML = "Nickname : " + pseudo

    gameStarted = true;

    startButton.style.display = "none";
    clickButton.style.display = "block";

}

function upgradeScore() {
    if (gameStarted) {
        score += 1;

        var i = Math.floor(Math.random() * 420);
        var j = Math.floor(Math.random() * 420);
        clickButton.style.transition = "0.15s"
        clickButton.style.left = i + "px";
        clickButton.style.top = j + "px";
    }
}

function changeContent(isHovered) {
    let button = document.getElementById("playButton");

    if (isHovered) {
        button.innerHTML = "Play"
    }
    else {
        button.innerHTML = "▶"
    }
    button.style.transition = 0.35;
}

function startBGMusic() {
    const audio = document.querySelector("audio");
    audio.volume = 0.2;
    audio.play();
}