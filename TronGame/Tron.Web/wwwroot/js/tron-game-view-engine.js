function createTronViewEngine(step, lineWidth, canvasId) {
    const canvas = document.getElementById(canvasId);
    const context = canvas.getContext('2d');
    context.lineWidth = lineWidth;

    const drawPlayerInternal = (playerPath) => {
        for (let j = 0; j < playerPath.length - 1; j++) {
            context.beginPath();
            context.moveTo(playerPath[j].positionX * step,
                playerPath[j].positionY * step);
            context.lineTo(playerPath[j + 1].positionX * step,
                playerPath[j + 1].positionY * step);
            context.stroke();
        }
    };

    const drawePlayerCrashInternal = (position) => {
        context.beginPath();
        context.arc(position.PositionX * step, position.PositionY * step, lineWidth / 2, 0, 2 * Math.PI);
        context.stroke();
    };

    const clearInternal = () => {
        context.clearRect(0, 0, canvas.width, canvas.height);
    };

    return {
        drawPlayer: drawPlayerInternal,
        drawePlayerCrash: drawePlayerCrashInternal,
        clear: clearInternal
    };
}