using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTool : MonoBehaviour
{

    [SerializeField] private Transform _hand;

    public bool HasTool { get; private set; }
    private ToolNetworkSync _currentTool;

    private void Awake()
    {
        DropTool();
    }
    
    public void HoldTool(ToolNetworkSync tool)
    {
        _currentTool = tool;
        _currentTool.transform.parent = _hand;
        _currentTool.transform.localPosition = Vector3.zero;
        _currentTool.transform.localRotation = Quaternion.identity;
        // _currentTool.transform.localScale = Vector3.one * 4f;
        HasTool = true;
    }

    public void DropTool()
    {
        if (_currentTool)
        {
            _currentTool.transform.parent = null;
            _currentTool.transform.localRotation = Quaternion.identity;
        }
        _currentTool = null;
        HasTool = false;
    }

    public void Use(BuildingController building)
    {
        bool isAvailable = _currentTool.UseTool(building);
        if (!isAvailable) DropTool();
    }

}
