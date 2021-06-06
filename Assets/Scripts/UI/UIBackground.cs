using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBackground : MonoBehaviour
{
    
    [SerializeField] private Color _endColor = Color.red;

    private UITimer _timer;
    private Image _backgroundImage;

    private void Awake()
    {
        _timer = GameObject.FindObjectOfType<UITimer>();
        _backgroundImage = GetComponent<Image>();
    }

    private void Update()
    {
        float t = _timer.DeltaTime / _timer.MaxTime;
        _backgroundImage.color = Color.Lerp(_endColor, Color.white, t);
    }

}
