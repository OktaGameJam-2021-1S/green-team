using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    private float _horizontalPosition;
    private int _verticalPosition;
    public int VerticalPosition => _verticalPosition;

    private void Awake()
    {
        _verticalPosition = 1;
    }

    public void MoveHorizontal(float x)
    {
        _horizontalPosition = x;
        UpdatePosition();
    }

    public void MoveVertical(int y)
    {
        _verticalPosition = y;
        if (_verticalPosition > 2) _verticalPosition = 2;
        if (_verticalPosition < 0) _verticalPosition = 0;
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        float yPos = 0f;
        switch (_verticalPosition)
        {
            case 1:
                yPos = -2.5f;
                break;
            case 0:
                yPos = -4f;
                break;
        }

        transform.position = new Vector3(_horizontalPosition, yPos, 0f);
    }

}
