using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTool : MonoBehaviour
{

    public enum Tool
    {
        None,
        Hammer,
        Paint
    }

    [SerializeField] private SpriteRenderer _toolRenderer = default;
    [SerializeField] private GenericDictionary<Tool, Sprite> _toolSprites = default;

    private Tool _currentTool;

    private void Awake()
    {
        DropTool();
    }
    
    public void HoldTool(Tool tool)
    {
        _currentTool = tool;
        UpdateSprite();
    }

    public void DropTool()
    {
        _currentTool = Tool.None;
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        _toolRenderer.sprite = _toolSprites[_currentTool];
    }

}
