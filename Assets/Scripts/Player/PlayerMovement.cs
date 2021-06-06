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

    private BuildingController _insideBuilding;
    public BuildingController InsideBuilding => _insideBuilding;

    private void Awake()
    {
        _verticalPosition = LayerHeight.Sidewalk;
    }

    public void Move(float x, int y)
    {
        LayerHeight lastLayerHeight = _verticalPosition;
        _verticalPosition = _verticalPosition + y;
        if (_verticalPosition > LayerHeight.Building) _verticalPosition = LayerHeight.Building;
        if (_verticalPosition < LayerHeight.Street) _verticalPosition = LayerHeight.Street;

        if (GameController.Instance.IsGameEnded && _verticalPosition == LayerHeight.Building) _verticalPosition = LayerHeight.Sidewalk;

        float yPos = LayerHeightHelper.GetVerticalPosition(_verticalPosition);

        if(_verticalPosition == LayerHeight.Building && lastLayerHeight == LayerHeight.Sidewalk)
        {
            EnterBuilding();
        }

        if (_verticalPosition == LayerHeight.Sidewalk && lastLayerHeight == LayerHeight.Building)
        {
            LeaveBuilding();
        }

        _speed = x * _moveSpeed;
        _speed = (_verticalPosition == LayerHeight.Building) ? 0 : _speed;
        if(_playerAnimator) _playerAnimator.SetFloat("Speed", _speed);

        float xPos = transform.position.x + (_speed * Time.deltaTime);

        xPos = Mathf.Clamp(xPos, GameController.Instance.MinWorldBounds + 1, GameController.Instance.MaxWorldBounds - 1);
        transform.position = new Vector3(xPos, yPos);
    }

    private void EnterBuilding()
    {
        if (GameController.Instance.IsGameEnded) return;
        var building = GameController.Instance.GetBuilding(this, false);
        if (building)
        {
            building.PlayersInside.Add(this);
            _insideBuilding = building;
            _insideBuilding.OnDemolish += HandleBuildingDemolish;
            building.UpdateMarkers();
        }
        else
        {
            _verticalPosition = LayerHeight.Sidewalk;
        }
    }

    public void LeaveBuilding()
    {
        if (_insideBuilding)
        {
            _insideBuilding.PlayersInside.Remove(this);
            _insideBuilding.OnDemolish -= HandleBuildingDemolish;
            _verticalPosition = LayerHeight.Sidewalk;
            _insideBuilding.UpdateMarkers();
            _insideBuilding = null;
        }
    }

    private void HandleBuildingDemolish()
    {
        Debug.Log("DEMOLISH");
        LeaveBuilding();
    }
    
    public void UpdatePosition()
    {
        float yPos = LayerHeightHelper.GetVerticalPosition(_verticalPosition);
        transform.position = new Vector3(_horizontalPosition, yPos, 0f);
    }

}
