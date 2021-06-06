using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationController : MonoBehaviour
{
    [SerializeField] private PlayerColor _color;
    [SerializeField] private Animator _charAnimator;
    
    private float _moveDirection = 1;
    private float _moveSpeed = 1.25f;
    private float _moveRandomGain = 0.75f;
    private float _randomHeight = 0.6f;

    void Start()
    {
        //Paint with random color
        _color.SetPlayerColor(Random.ColorHSV(0f, 1f, 0.4f, 0.6f, 0.9f, 0.9f));

        //Random direction and speed
        if (Random.value < 0.5f)
        {
            _moveDirection = 1;
        }
        else
        {
            _moveDirection = -1;
        }
        _moveSpeed = Random.Range(_moveSpeed, _moveSpeed + _moveRandomGain);
        float moveNormal = _moveDirection * _moveSpeed / (_moveSpeed + _moveRandomGain);
        _charAnimator.SetFloat("Speed", moveNormal);
        _charAnimator.SetFloat("Offset", Random.Range(0f, 1f));

        //Random height (y position)
        float height = Random.Range(-_randomHeight, 0f);
        transform.position = new Vector3(transform.position.x, transform.position.y + height, transform.position.z + 1f + height);
    }

    // Update is called once per frame
    void Update()
    {
        float xPos = transform.position.x + (_moveDirection * _moveSpeed * Time.deltaTime);
        float yPos = transform.position.y;
        float zPos = transform.position.z;

        transform.position = new Vector3(xPos, yPos, zPos);

        //Destroy if outside game camera
        if(xPos > GameController.Instance.MaxWorldBounds+2f || xPos < GameController.Instance.MinWorldBounds - 2f)
        {
            Destroy(gameObject);
        }
    }
}
