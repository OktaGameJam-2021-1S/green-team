
using UnityEngine;

namespace Player
{
    public abstract class BaseMovementController : MonoBehaviour
    {
        [SerializeField]
        DepthLayer _actualLayer;

        public DepthLayer ActualLayer
        {
            get
            {
                return _actualLayer;
            }
            
            protected set
            {
                _actualLayer = value;
            }
        }

        public abstract void Move(short pDirection);
        public abstract void IncreaseDepthLayer();

        public abstract void DecreaseDepthLayer();
    }

    public enum DepthLayer
    {
        Street = 0,
        Pavement = 1,
        Building = 2,
        Rooftop = 3,
    }
}
