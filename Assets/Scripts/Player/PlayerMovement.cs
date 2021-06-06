using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float _moveSpeed = 5f;
    private float _speed = 5f;
    
    private float _horizontalPosition;
    private LayerHeight _verticalPosition;

    [SerializeField]
    public LayerHeight VerticalPosition => _verticalPosition;

    [SerializeField] private Animator _playerAnimator;

    private void Awake()
    {
        _verticalPosition = LayerHeight.Sidewalk;
    }

    public void Move(float x, int y)
    {
        _verticalPosition = _verticalPosition + y;
        if ((int)_verticalPosition > (int)LayerHeight.Building) _verticalPosition = LayerHeight.Building;
        if ((int)_verticalPosition < (int)LayerHeight.Street) _verticalPosition = LayerHeight.Street;

        float yPos = LayerHeightHelper.GetVerticalPosition(_verticalPosition);

        _speed = x * _moveSpeed;
        _speed = (_verticalPosition == LayerHeight.Building) ? 0 : _speed;
        if(_playerAnimator) _playerAnimator.SetFloat("Speed", _speed);

        float xPos = transform.position.x + (_speed * Time.deltaTime);

        
        transform.position = new Vector3(xPos, yPos);
    }

    public void MoveHorizontal(float x)
    {
        _horizontalPosition = x;
        UpdatePosition();
    }

    public void MoveVertical(int y)
    {
    }

    public void UpdatePosition()
    {
        float yPos = LayerHeightHelper.GetVerticalPosition(_verticalPosition);
        transform.position = new Vector3(_horizontalPosition, yPos, 0f);
    }

}
