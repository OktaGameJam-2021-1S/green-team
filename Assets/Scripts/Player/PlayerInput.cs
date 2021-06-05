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

    }

}
