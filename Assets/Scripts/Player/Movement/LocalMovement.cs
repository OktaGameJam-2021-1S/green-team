using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    public class LocalMovement : BaseMovementController
    {

        [SerializeField] private float fSpeed;

        public override void IncreaseDepthLayer()
        {
            if (ActualLayer != DepthLayer.Rooftop)
            {
                ActualLayer = (DepthLayer)(ActualLayer + 1);
            }
        }

        public override void DecreaseDepthLayer()
        {
            if (ActualLayer != DepthLayer.Street)
            {
                ActualLayer = (DepthLayer)(ActualLayer - 1);
            }
        }

        public override void Move(short pDirection)
        {
            transform.position += Vector3.right * pDirection * Time.deltaTime * fSpeed;
        }
    }
}
