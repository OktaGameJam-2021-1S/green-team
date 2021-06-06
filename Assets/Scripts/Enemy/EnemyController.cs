using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private float _moveSpeed = -4f;

    // Update is called once per frame
    void Update()
    {
        float xPos = transform.position.x + (_moveSpeed * Time.deltaTime);
        float yPos = transform.position.y;

        transform.position = new Vector3(xPos, yPos);

        if (xPos > GameController.Instance.MaxWorldBounds + 2f || xPos < GameController.Instance.MinWorldBounds - 2f)
        {
            _moveSpeed *= -1;
        }
    }
}
