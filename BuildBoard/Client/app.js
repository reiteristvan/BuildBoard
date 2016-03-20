//(function () {
    'use strict';

    var locationId = 1;

    var onBroadcastMessage = function (message) {
        console.log(message);

        var $board = $('#board');
        $board.prepend('<li class="list-group-item">' + message + '</li>');
    };

    var serverUrl = 'http://localhost:17173';

    var connection = $.hubConnection();
    var proxy = connection.createHubProxy('MessageBroadcastHub');

    proxy.on('broadcastMessage', onBroadcastMessage);

    connection.start().done(function() {
        proxy.invoke('joinLocation', locationId);
    });
//})();