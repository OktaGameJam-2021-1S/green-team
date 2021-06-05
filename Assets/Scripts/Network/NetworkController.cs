using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Socket.Quobject.SocketIoClientDotNet.Client;

public class NetworkController : MonoBehaviour
{

    public static NetworkController Instance { get; private set; }

    public delegate void GameStateHandler(GameStateNetwork state);
    public event GameStateHandler OnGameState;

    [SerializeField] private string _url = "http://localhost:3000";

    private QSocket _socket;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    private void Start()
    {
        Debug.Log ($"start {_url}");
        _socket = IO.Socket (_url);

        _socket.On (QSocket.EVENT_CONNECT, () => {
            Debug.Log ("Connected");

            Debug.Log("Sending PING");
            // _socket.Emit("ping");
            // _socket.On("pong", () => {
            //     Debug.Log("Received PONG");
            // });
            // _socket.On("game_state", HandleGameState);
            _socket.On("game_state", (data) => {
                Debug.Log("received game_state " + data);
            });

        });

        _socket.On (QSocket.EVENT_CONNECT_ERROR, () => {
            Debug.Log ("EVENT_CONNECT_ERROR");
        });

        _socket.On (QSocket.EVENT_ERROR, () => {
            Debug.Log ("EVENT_ERROR");
        });

        _socket.On (QSocket.EVENT_CONNECT_TIMEOUT, () => {
            Debug.Log ("EVENT_CONNECT_TIMEOUT");
        });

        _socket.On (QSocket.EVENT_DISCONNECT, () => {
            Debug.Log ("EVENT_DISCONNECT");
        });

        _socket.On (QSocket.EVENT_MESSAGE, () => {
            Debug.Log ("EVENT_MESSAGE");
        });
    }

    private void HandleGameState(object data)
    {
        Debug.Log("Received game_state: " + data.ToString());
        var gameState = Newtonsoft.Json.JsonConvert.DeserializeObject<GameStateNetwork>(data.ToString());
        OnGameState?.Invoke(gameState);
    }

    public void SendInput(PlayerInputNetwork inputNetwork)
    {
        // _socket.Emit ("player_input", Newtonsoft.Json.JsonConvert.SerializeObject(inputNetwork));
    }

    private void OnDestroy ()
    {
        _socket.Disconnect ();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleGameState("{\"players\":[{\"id\":0,\"x\":1.3,\"y\":1,\"tool\":0},{\"id\":1,\"x\":-1.15,\"y\":0,\"tool\":1}],\"buildings\":[{\"id\":0,\"x\":3,\"width\":2,\"height\":3,\"color\":\"#ffcc00\",\"damage\":1,\"plant\":2,\"graffiti\":false},{\"id\":1,\"x\":5,\"width\":2,\"height\":2,\"color\":\"#21ff07\",\"damage\":7,\"plant\":0,\"graffiti\":true}]}");
        }
    }

}