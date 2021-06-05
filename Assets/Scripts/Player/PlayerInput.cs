using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(BaseMovementController))]
    public class PlayerInput : MonoBehaviour
    {
        BaseMovementController pMovementController;

        private void Awake()
        {
            pMovementController = GetComponent<BaseMovementController>();
        }


        private void Update()
        {

            short pDirection = 0;

            if(Input.GetKey(KeyCode.A))
            {
                pDirection -= 1;
            }
            if (Input.GetKey(KeyCode.D))
            {
                pDirection += 1;
            }

            if (pDirection != 0)
            {
                pMovementController.Move(pDirection);
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                pMovementController.IncreaseDepthLayer();
            }
            if(Input.GetKeyDown(KeyCode.S))
            {
                pMovementController.DecreaseDepthLayer();
            }

            

        }

    }
}
