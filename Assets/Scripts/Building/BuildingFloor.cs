using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BuildingFloor : MonoBehaviour
{

    [SerializeField] private SpriteRenderer _naturalized;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private BoxCollider2D _collider;


    private SpriteRenderer _spriteRenderer;

    public BoxCollider2D Collider => _collider;

    public bool Interactable { get; private set; }

    public bool Naturalized { get; private set; }

    public bool Destroyed { get; private set; }

    public bool Graffiti { get; private set; }

    Color baseColor = Color.white;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Reset();
    }

    public void Reset()
    {
        _renderer.material.SetFloat("_Damage", 0f);
        baseColor.a = 0f;
        _naturalized.color = baseColor;
        Interactable = true;
        Naturalized = false;
        Destroyed = false;
        Graffiti = false;
        _spriteRenderer.color = Color.white;
    }

    public void Naturalize()
    {
        StartCoroutine(NaturalizeAnimation());
        Interactable = false;
        Naturalized = true;
    }

    public void DamageFloor()
    {

        StartCoroutine(DamageAnimation());
        Interactable = false;
        Destroyed = true;
    }

    public void GraffitiFloor()
    {

        //StartCoroutine(DamageAnimation());
        Interactable = false;
        Graffiti = true;
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

    public void DemolishFloor()
    {
        _spriteRenderer.color = Color.gray;
    }


}
