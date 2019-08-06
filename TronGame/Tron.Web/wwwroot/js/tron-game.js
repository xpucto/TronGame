// this file is not used it was an initial implementation of only client game
function createGameEngine(canvasId, viewEngine) {
    let width;
    let height;
    let gameField;
    let numberOfPlayers;
    let players;
    let interval;
    let gameInterval;

    const createGameField = () => {
        gameField = new Array(height);
        let i = 0;
        for (; i < height; i++) {
            gameField[i] = new Array(width);
        }

            for (i = 0; i < height; i++) {
                for (let j = 0; j < width; j++) {
                    if (i === 0 || j === 0 || i === height - 1 || j === width - 1) {
                        gameField[i][j] = 1;
                    } else {
                        gameField[i][j] = 0;
                    }
                }
            }
    };

    const createPlayers = () => {
        players = new Array(numberOfPlayers);
        const halfWidth = Math.round(width / 2);
        const halfHeight = Math.round(height / 2);
        for (var i = 0; i < numberOfPlayers; i++) {
            if (i === 0) {
                players[i] = new Player(new PlayerPosition(1, halfHeight, Directions.right));
            } else if (i === 1) {
                players[i] = new Player(new PlayerPosition(width - 2, halfHeight, Directions.left));
            } else {
                players[i] = new Player(new PlayerPosition(halfWidth, 0, Directions.up));
            }
        }
    };

    const intializeInternal = (widthParam,
                                heightParam,
                                numberOfPlayersParam,
                                intevalParam
                                ) => {
        width = widthParam;
        height = heightParam;
        numberOfPlayers = numberOfPlayersParam;
        interval = intevalParam;

        createGameField();

        createPlayers();
    };

    const canvas = document.getElementById(canvasId);

    canvas.addEventListener('keydown', function (event) {
        event.preventDefault();
        players[0].changeDirection(event.keyCode);
    });

    const drawPlayers = () => {
        viewEngine.clear();
        for (let i = 0; i < players.length; i++) {
            if (players[i].isAlive) {
                viewEngine.drawPlayer(players[i].playerPath);
            }
        }
    };

    const killPlayer = (player) => {
        player.isAlive = false;
        viewEngine.drawPlayer(player.playerPath);
        viewEngine.drawePlayerCrash(player.playerPosition.positionX, player.playerPosition.positionY);

        // clear the field
        for (let i = 0; i < player.playerPath.length; i++) {
            gameField[player.playerPath[i].positionY][player.playerPath[i].positionX] = 0;
        }

        // check is there more alive players
        for (let i = 0; i < players.length; i++) {
            if (players[i].isAlive) {
                return;
            }
        }

        clearInterval(gameInterval);
    };

    const generatePlayer = (player) => {
        player.move();
        if (gameField[player.playerPosition.positionY][player.playerPosition.positionX] === 1) {
            killPlayer(player);
            return;
        }

        gameField[player.playerPosition.positionY][player.playerPosition.positionX] = 1;
        player.score++;
        drawPlayers();
    };

    const runInternal = () => {
        viewEngine.clear();
        gameInterval = setInterval(function () {
            for (let i = 0; i < players.length; i++) {
                if (players[i].isAlive) {
                    generatePlayer(players[i]);
                }
            }
        }, interval);
    };

    return {
        initialize: intializeInternal,
        run: runInternal,
    };
}

class PlayerPosition {
    constructor(x, y, direction) {
        this.positionX = x;
        this.positionY = y;
        this.direction = direction;
    }
}

class Player {
    constructor(playerPosition) {
        this.playerPosition = playerPosition;
        this.isAlive = true;
        this.playerPath =
            [new PlayerPosition(this.playerPosition.positionX, this.playerPosition.positionY, this.playerPosition.direction)];
        this.score = 0;
    }

    changeDirection = (keyCode) => {
        if (keyCode === Directions.up && this.playerPosition.direction !== Directions.down) {
            this.playerPosition.direction = Directions.up;
        } else if (keyCode === Directions.left && this.playerPosition.direction !== Directions.right) {
            this.playerPosition.direction = Directions.left;
        } else if (keyCode === Directions.right && this.playerPosition.direction !== Directions.left) {
            this.playerPosition.direction = Directions.right;
        } else if (keyCode === Directions.down && this.playerPosition.direction !== Directions.up) {
            this.playerPosition.direction = Directions.down;
        }
    };

    move() {
        switch (this.playerPosition.direction) {
            case Directions.right:
                this.playerPosition.positionX++;
                break;
            case Directions.left:
                this.playerPosition.positionX--;
                break;
            case Directions.up:
                this.playerPosition.positionY--;
                break;
            case Directions.down:
                this.playerPosition.positionY++;
                break;
        }

        this.playerPath.push(
            new PlayerPosition(this.playerPosition.positionX, this.playerPosition.positionY, this.playerPosition.direction));
    }
}

class Directions {
    static up = 38;
    static down = 40;
    static left = 37;
    static right = 39;
}