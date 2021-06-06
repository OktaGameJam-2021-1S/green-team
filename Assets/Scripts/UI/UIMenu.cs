using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMenu : MonoBehaviour
{
    
    [SerializeField] private Button _createRoomButton;
    [SerializeField] private Button _enterRoomButton;
    [SerializeField] private Button _startGameButton;
    [SerializeField] private TMP_InputField _roomName;

    private void Awake()
    {
        _createRoomButton.onClick.AddListener(() => {
            NetworkController.Instance.CreateRoom(_roomName.text);
        });

        _enterRoomButton.onClick.AddListener(() => {
            NetworkController.Instance.EnterRoom(_roomName.text);
        });

        _startGameButton.onClick.AddListener(() => {
            NetworkController.Instance.StartGame();
        });
    }

}
