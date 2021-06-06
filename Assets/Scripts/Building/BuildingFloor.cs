using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BuildingFloor : MonoBehaviour
{

    [SerializeField] private SpriteRenderer _naturalized;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private BoxCollider2D _collider;

    public BoxCollider2D Collider => _collider;

    public bool Interactable { get; private set; }


    Color baseColor = Color.white;

    private void Start()
    {
        _renderer.material.SetFloat("_Damage", 0f);
        baseColor.a = 0f;
        _naturalized.color = baseColor;
        Interactable = true;
    }

    public void Naturalize()
    {
        StartCoroutine(NaturalizeAnimation());
        Interactable = false;
    }

    public void DamageFloor()
    {

        StartCoroutine(DamageAnimation());
        Interactable = false;
    }

    IEnumerator DamageAnimation()
    {
        float fTime = 0;
        float fMaxTime = 0.9f;
        while(fTime <= fMaxTime)
        {
            _renderer.material.SetFloat("_Damage", fTime / fMaxTime);
            fTime += Time.deltaTime;
            yield return null;
        }
        _renderer.material.SetFloat("_Damage", 1f);
    }

    IEnumerator NaturalizeAnimation()
    {
        float fTime = 0;
        float fMaxTime = 0.9f;
        while (fTime <= fMaxTime)
        {
            baseColor.a = fTime / fMaxTime;

            _naturalized.color = baseColor;
            fTime += Time.deltaTime;
            yield return null;
        }
        baseColor.a = 1f;
        _naturalized.color = baseColor;
    }

}
