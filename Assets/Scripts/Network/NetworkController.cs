using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Socket.Quobject.SocketIoClientDotNet.Client;
using Socket.Newtonsoft.Json.Linq;

public class NetworkController : MonoBehaviour
{

    public static NetworkController Instance { get; protected set; }

    public delegate void GameStateHandler(GameStateNetwork state);
    public event GameStateHandler OnGameState;

    [SerializeField] private string _url = "http://localhost:3000";

    private QSocket _socket;

    private PlayerInputNetwork _inputNetwork;

    private bool _connected;

    protected virtual void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    private void Start()
    {
        Debug.Log ($"start {_url}");
        _connected = false;
        _socket = IO.Socket (_url);

        StartCoroutine(SendInputCoroutine());
        _socket.On (QSocket.EVENT_CONNECT, () => {
            Debug.Log ("Connected");
            _connected = true;
            _socket.On("game_state", HandleGameState);
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
        var json = (JObject)data;
        var gameState = json.ToObject<GameStateNetwork>();
        DispatchGameState(gameState);
    }

    protected void DispatchGameState(GameStateNetwork gameState)
    {
        OnGameState?.Invoke(gameState);
    }

    public virtual void SendInput(PlayerInputNetwork inputNetwork)
    {
        _inputNetwork = inputNetwork;
    }

    private IEnumerator SendInputCoroutine()
    {
        yield return new WaitUntil(() => _connected);
        while (true)
        {
            yield return new WaitForSeconds(.1f);
            _socket.Emit ("player_input", Newtonsoft.Json.JsonConvert.SerializeObject(_inputNetwork));
        }
    }

    private void OnDestroy ()
    {
        _socket.Disconnect ();
    }

}