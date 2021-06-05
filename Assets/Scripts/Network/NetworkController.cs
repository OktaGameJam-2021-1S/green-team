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
    private NetworkPlayerInput _playerInput;

    private bool _connected;

    protected virtual void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;

        _inputNetwork = new PlayerInputNetwork();
        _playerInput = GetComponent<NetworkPlayerInput>();
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
            _socket.On("player_connect", HandlePlayerConnect);
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

    private void HandlePlayerConnect(object data)
    {
        try
        {
            var json = (JObject)data;
            var playerNetwork = json.ToObject<PlayerNetwork>();
            _playerInput.Construct(playerNetwork);
        }
        catch (System.Exception err)
        {
            Debug.LogError(err.Message);
            throw;
        }
    }

    private void HandleGameState(object data)
    {
        try
        {
            var json = (JObject)data;
            var gameState = json.ToObject<GameStateNetwork>();
            DispatchGameState(gameState);
        }
        catch (System.Exception err)
        {
            Debug.LogError(err.Message);
            throw;
        }
    }

    protected void DispatchGameState(GameStateNetwork gameState)
    {
        OnGameState?.Invoke(gameState);
    }

    public virtual void SendDamageBuilding(DamageBuildingNetwork damageBuildingNetwork)
    {
        _socket.Emit ("player_damage_building", Newtonsoft.Json.JsonConvert.SerializeObject(damageBuildingNetwork));
    }

    public virtual void SendInput(PlayerInputNetwork inputNetwork)
    {
        _inputNetwork.horizontal = inputNetwork.horizontal;
        if (inputNetwork.vertical != 0) _inputNetwork.vertical = inputNetwork.vertical;
        if (inputNetwork.use) _inputNetwork.use = inputNetwork.use;
    }

    private IEnumerator SendInputCoroutine()
    {
        yield return new WaitUntil(() => _connected);
        while (true)
        {
            yield return new WaitForSeconds(.1f);
            _socket.Emit ("player_input", Newtonsoft.Json.JsonConvert.SerializeObject(_inputNetwork));
            _inputNetwork.Clear();
        }
    }

    private void OnDestroy ()
    {
        _socket.Disconnect ();
    }

}