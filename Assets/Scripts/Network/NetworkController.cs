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

    public enum GameStatus
    {
        Menu,
        PreStart,
        Game
    }

    public GameStatus CurrentGameStatus { get; private set; }

    [SerializeField] private string _url = "http://localhost:3000";

    private QSocket _socket;

    private PlayerInputNetwork _inputNetwork;
    private NetworkPlayerInput _playerInput;

    private bool _connected;
    private string _deviceId;

    private bool _loadGameScene;

    protected virtual void Awake()
    {
        Debug.Log("NetworkController?");
        DontDestroyOnLoad(gameObject);
        Instance = this;

        _deviceId = PlayerPrefs.GetString("DEVICE_ID", System.Guid.NewGuid().ToString());
        PlayerPrefs.SetString("DEVICE_ID", _deviceId);

        _inputNetwork = new PlayerInputNetwork();
        _playerInput = GetComponent<NetworkPlayerInput>();

        CurrentGameStatus = GameStatus.Menu;
    }

    public void CreateRoom(string name)
    {
        _socket.Emit("create_room", "{\"roomName\": \"" + name + "\"}");
    }

    public void EnterRoom(string name)
    {
        _socket.Emit("enter_room", "{\"roomName\": \"" + name + "\"}");
    }

    public void StartGame()
    {
        _socket.Emit("start_game", "");
    }

    protected virtual void Start()
    {
        Debug.Log ($"start {_url}");
        _connected = false;
        _socket = IO.Socket (_url);

        StartCoroutine(SendInputCoroutine());
        _socket.On (QSocket.EVENT_CONNECT, () => {
            Debug.Log ("Connected");
            _connected = true;
            // _socket.Emit("create_user", "{\"device_id\": \"" + _deviceId + "\"}");
            
            _socket.On("game_pre_start", () => {
                Debug.Log("game_pre_start");
                _loadGameScene = true;
                CurrentGameStatus = GameStatus.PreStart;
            });
            _socket.On("game_start", () => {
                Debug.Log("game_start");
                CurrentGameStatus = GameStatus.Game;
            });

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

    private void Update()
    {
        if (_loadGameScene)
        {
            _loadGameScene = false;
            UnityEngine.SceneManagement.SceneManager.LoadScene("GamePrototype");
        }
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

    public virtual void SendSeedBuilding(SeedBuildingNetwork seedBuildingNetwork)
    {
        _socket.Emit ("player_seed_building", Newtonsoft.Json.JsonConvert.SerializeObject(seedBuildingNetwork));
    }

    public virtual void SendPickUpTool(PickUpToolNetwork pickUpToolNetwork)
    {
        _socket.Emit ("player_pick_up_tool", Newtonsoft.Json.JsonConvert.SerializeObject(pickUpToolNetwork));
    }

    public virtual void SendDropTool(DropToolNetwork dropToolNetwork)
    {
        _socket.Emit ("player_drop_tool");
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
        yield return new WaitUntil(() => CurrentGameStatus == NetworkController.GameStatus.Game);
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