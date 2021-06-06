using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolPosition : MonoBehaviour
{

    public LayerHeight VerticalPosition { get; private set; }
    
    public void Setup(float position, LayerHeight height)
    {
        VerticalPosition = height;
        transform.position = new Vector3(
            position,
            LayerHeightHelper.GetVerticalPosition(height)
        );
    }

}
