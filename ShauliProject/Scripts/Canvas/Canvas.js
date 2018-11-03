
var canvas = $('#canvasInAPerfectWorld');
context = canvas[0].getContext("2d");
var clickX = new Array();
var clickY = new Array();
var clickDrag = new Array();
var paint;
var colorPurple = "#cb3594";
var colorGreen = "#659b41";
var colorYellow = "#ffcf33";
var colorRed = "#df4b26";
var colorGray = "#f0f0f0";
var curTool = "no";
var curColor = colorPurple;
var clickColor = new Array();
var canvasImage;

context.fillStyle = colorGray;
context.fillRect(20, 20, canvas[0].width, canvas[0].height);
canvas.mousedown(function (event) {
    var mouseX = event.pageX - this.offsetLeft;
    var mouseY = event.pageY - this.offsetTop;

    paint = true;
    addClick(event.pageX - this.offsetLeft, event.pageY - this.offsetTop);
    redraw();
});
canvas.mousemove(function (event) {
    if (paint) {
        addClick(event.pageX - this.offsetLeft, event.pageY - this.offsetTop, true);
        redraw();
    }
});
canvas.mouseup(function (event) {
    paint = false;
});
canvas.mouseleave(function (event) {
    paint = false;
});

$('#choosePurple').mousedown (function (event) {
    curColor = colorPurple;
    curTool = "no";
});
$('#chooseGreen').mousedown(function (event) {
    curColor = colorGreen;
    curTool = "no";
});
$('#chooseYellow').mousedown(function (event) {
    curColor = colorYellow;
    curTool = "no";
});
$('#chooseRed').mousedown(function (event) {
    curColor = colorRed;
    curTool = "no";
});
$('#Eraser').mousedown(function (event) {
    curTool = "eraser";
});


function addClick(x, y, dragging) {
    clickX.push(x);
    clickY.push(y);
    clickDrag.push(dragging);
    if (curTool == "eraser") {
        clickColor.push(colorGray);
    } else {
        clickColor.push(curColor);
    }
}
function download() {
    var image = canvas[0].toDataURL("image/png");
    var download = $('<a/>')[0];
    download.href = image;
    download.download = 'image.png';
    download.click();
    delete download;
    delete image;
}
function redraw() {
    
    context.lineJoin = "round";
    context.lineWidth = 5;

    for (var i = 0; i < clickX.length; i++) {
        context.beginPath();
        if (clickDrag[i] && i) {
            context.moveTo(clickX[i - 1], clickY[i - 1]);
        } else {
            context.moveTo(clickX[i] - 1, clickY[i]);
        }
        context.lineTo(clickX[i], clickY[i]);
        context.closePath();
        context.strokeStyle = clickColor[i];
        context.stroke();
    }
}