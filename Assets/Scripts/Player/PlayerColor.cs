using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColor : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] sprites;
    public Color color = Color.white;

    public void SetPlayerColor()
    {
        foreach(SpriteRenderer s in sprites)
        {
            s.color = color;
        }
    }
}
