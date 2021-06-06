using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildCount : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;


    public void SetText(string sText)
    {
        _text.text = sText;
    }

}
