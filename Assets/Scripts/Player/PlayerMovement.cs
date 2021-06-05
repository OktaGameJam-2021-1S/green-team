using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
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
        float yPos = LayerHeightHelper.GetVerticalPosition(_verticalPosition);
        transform.position = new Vector3(_horizontalPosition, yPos, 0f);
    }

}
