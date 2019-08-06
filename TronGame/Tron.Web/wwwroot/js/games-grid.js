function createGamesGrid(dataAddress, playerName) {
    const columnDefs = [
        { headerName: 'Game name', field: 'name' },
        { headerName: 'Number of players', field: 'numberOfPlayers' },
        { headerName: 'Joined players', field: 'joinedPlayersCount' },
        {
            headerName: '',
            field: 'name',
            cellRenderer: function (params) {
                //return '<a onclick="joinGame(\'' + params.value + '\',\'' + playerName + '\')">Join</a>';
                return '<a href="Home/JoinGame?gameName=' + params.value + '&&playerName=' + playerName + '">Join</a>';
            }
        }
    ];
    const gridOptions = {
        columnDefs: columnDefs,
        defaultColDef: {
            sortable: true,
            filter: true
        }
    };
    const eGridDiv = document.querySelector('#myGrid');
    new agGrid.Grid(eGridDiv, gridOptions);

    const loadDataInternal = () => {
        fetch(dataAddress).then(function (response) {
            return response.json();
        }).then(function (data) {
            gridOptions.api.setRowData(data);
        });
    };

    loadDataInternal();
    return { relodData: loadDataInternal };
}