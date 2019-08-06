"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();

//Disable send button until connection is established
//document.getElementById("sendButton").disabled = true;
const user = document.getElementById("playerName").value;
const game = document.getElementById("gameName").value;
connection.start().then(function () {
    connection.invoke("AddUser", user, game).catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});

const viewEngine = createTronViewEngine(10, 5, "canvas");
connection.on("movePayers", function (playerPaths) {
    viewEngine.clear();
    for (let i = 0; i < playerPaths.length; i++) {
        viewEngine.drawPlayer(playerPaths[i]);
    }
});

connection.on("killPayer", function (playerPosition) {
    viewEngine.drawePlayerCrash(playerPosition);
});

const canvas = document.getElementById("canvas");

canvas.addEventListener('keydown', function (event) {
    event.preventDefault();
    console.log(event.keyCode);
    connection.invoke("MovePlayer", game, event.keyCode).catch(function (err) {
        return console.error(err.toString());
    });
});
