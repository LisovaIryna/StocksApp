// Create a WebSocket to perform communication with server
const token = document.querySelector("#FinnhubToken").value;
const socket = new WebSocket(`wss://ws.finnhub.io?token=${token}`);
var stockSymbol = document.getElementById("StockSymbol").value; // get symbol from input hidden

// Connection opened. Subscribe to a symbol
socket.addEventListener('open', function (event) {
    socket.send(JSON.stringify({ 'type': 'subscribe', 'symbol': stockSymbol }))
});

// Listen (ready to receive) for messages
socket.addEventListener('message', function (event) {

    // if error message is received from server
    if (event.data.type == "error") {
        $(".price").text(event.data.msg);
        return;
    }

    var eventData = JSON.parse(event.data);
    if (eventData) {
        if (eventData.data) {
            var updatedPrice = JSON.parse(event.data).data[0].p;
            var timeStamp = JSON.parse(event.data).data[0].t;

            $(".price").text(updatedPrice.toFixed(2));
        }
    }
});

// Unsubscribe
var unsubscribe = function (symbol) {
    socket.send(JSON.stringify({ 'type': 'unsubscribe', 'symbol': symbol }))
}

// when the page is being closed, unsubscribe from the WebSocket
window.onunload = function () {
    unsubscribe(stockSymbol);
};