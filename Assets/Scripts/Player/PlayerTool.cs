using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTool : MonoBehaviour
{

    public bool HasTool { get; private set; }
    private ToolNetworkSync _currentTool;

    private void Awake()
    {
        DropTool();
    }
    
    public void HoldTool(ToolNetworkSync tool)
    {
        _currentTool = tool;
        _currentTool.transform.parent = transform;
        _currentTool.transform.localPosition = Vector3.zero;
        HasTool = true;
    }

    public void DropTool()
    {
        if (_currentTool) _currentTool.transform.parent = null;
        _currentTool = null;
        HasTool = false;
    }

    public void Use(PlayerMovement movement)
    {
        _currentTool.UseTool(movement);
    }

}
