using UnityEngine;
using UnityEngine.UI;

public class BuildingFloor : MonoBehaviour
{

    [SerializeField] private GameObject _naturalized;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private BoxCollider2D _collider;

    public BoxCollider2D Collider => _collider;

    public bool Interactable { get; private set; }

    private void Start()
    {
        _renderer.material.SetFloat("_Damage", 0f);
        _naturalized.SetActive(false);
        Interactable = true;
    }

    public void Naturalize()
    {
        _naturalized.SetActive(true);
        Interactable = false;
    }

    public void DamageFloor()
    {
        _renderer.material.SetFloat("_Damage", 1f);
        Interactable = false;
    }
    
}
