using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    private int _horizontal;
    private int _vertical;
    private bool _use;

    private int _lastVertical;

    private void Update()
    {
        _horizontal = (int) Input.GetAxisRaw("Horizontal");
        
        _vertical = 0;
        int vertical = (int) Input.GetAxisRaw("Vertical");
        if (vertical != _lastVertical)
        {
            _lastVertical = vertical;
            _vertical = vertical;
        }

        _use = false;
        if (Input.GetKeyDown(KeyCode.E))
        {
            _use = true;
        }

        SendInput();
    }

    private void SendInput()
    {
        
        // Dispatch event with this 
        if (NetworkController.Instance)
        {
            var input = new PlayerInputNetwork()
            {
                horizontal = _horizontal,
                vertical = _vertical,
                use = _use
            };
            NetworkController.Instance.SendInput(input);
        }
        else
        {
            SinglePlayerSync();
        }

    }

    // Delete this
    [SerializeField] private float _moveSpeed = 5f;
    private PlayerNetworkSync _sync;
    private PlayerMovement _movement;
    private void SinglePlayerSync()
    {
        if (_sync == null) _sync = FindObjectOfType<PlayerNetworkSync>();
        if (_movement == null) _movement = FindObjectOfType<PlayerMovement>();

        _sync.Sync(new PlayerNetwork()
        {
            x = _sync.transform.position.x + (_horizontal * _moveSpeed * Time.deltaTime),
            y = (int)(_movement.VerticalPosition + _vertical),
            tool = 1
        });

        if (_use)
        {
            var pBuilding = FindObjectOfType<BuildingNetworkSync>();
            pBuilding.TakeDamage();
        }

    }

}
