using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    public enum LayerHeight
    {
        Street = 0,
        Sidewalk = 1,
        Building = 2,
        TopBuilding1 = 3,
        TopBuilding2 = 4,
    }

    private float _horizontalPosition;
    private LayerHeight _verticalPosition;

    [SerializeField]
    public LayerHeight VerticalPosition => _verticalPosition;

    private void Awake()
    {
        _verticalPosition = LayerHeight.Sidewalk;
    }

    public void MoveHorizontal(float x)
    {
        _horizontalPosition = x;
        UpdatePosition();
    }

    public void MoveVertical(int y)
    {
        _verticalPosition = (LayerHeight) y;
        if ((int)_verticalPosition > (int)LayerHeight.TopBuilding2) _verticalPosition = LayerHeight.TopBuilding2;
        if ((int)_verticalPosition < (int)LayerHeight.Street) _verticalPosition = LayerHeight.Street;
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        float yPos = 0f;
        switch (_verticalPosition)
        {
            // The building height is different per building
            case LayerHeight.TopBuilding2:
                yPos = 2f;
                break;
            case LayerHeight.TopBuilding1:
                yPos = 1f;
                break;
            case LayerHeight.Building:
                yPos = 0f;
                break;
            case LayerHeight.Sidewalk:
                yPos = -1.20f;
                break;
            case LayerHeight.Street:
                yPos = -2.30f;
                break;
        }

        transform.position = new Vector3(_horizontalPosition, yPos, 0f);
    }

}
