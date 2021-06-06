using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolSprite : MonoBehaviour
{

    [SerializeField] private GenericDictionary<ToolType, Sprite> _toolSprites = default;

    private SpriteRenderer _spriteRenderer = default;

    private ToolType _type;
    public ToolType Type => _type;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    
    public void Construct(ToolType tool)
    {
        _type = tool;
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        _spriteRenderer.sprite = _toolSprites[_type];
    }
}
