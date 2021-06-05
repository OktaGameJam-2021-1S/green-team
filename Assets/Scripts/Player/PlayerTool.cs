using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTool : MonoBehaviour
{

    public bool HasTool { get; private set; }
    private Transform _currentTool;

    private void Awake()
    {
        DropTool();
    }
    
    public void HoldTool(Transform tool)
    {
        _currentTool = tool;
        _currentTool.parent = transform;
        _currentTool.localPosition = Vector3.zero;
        HasTool = true;
    }

    public void DropTool()
    {
        if (_currentTool) _currentTool.parent = null;
        _currentTool = null;
        HasTool = false;
    }

}
