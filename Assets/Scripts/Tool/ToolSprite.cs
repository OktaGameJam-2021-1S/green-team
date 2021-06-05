using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolSprite : MonoBehaviour
{

    public enum Tool
    {
        Hammer = 0,
        Paint = 1,
        Seed = 2
    }

    [SerializeField] private GenericDictionary<Tool, Sprite> _toolSprites = default;

    private SpriteRenderer _spriteRenderer = default;

    private Tool _currentTool;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    
    public void Construct(Tool tool)
    {
        _currentTool = tool;
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        _spriteRenderer.sprite = _toolSprites[_currentTool];
    }
}
