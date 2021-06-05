using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingNetworkSync : MonoBehaviour
{

    [SerializeField] private GameObject _damagePrefab = default;
    [SerializeField] private float _damageOffset = .2f;

    private int _id;
    public int ID => _id;

    private List<GameObject> _damageCreated;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _damageCreated = new List<GameObject>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    
    public void Sync(BuildingNetwork network)
    {
        _id = network.id;

        Color color;
        if (ColorUtility.TryParseHtmlString(network.color, out color))
        {
            _spriteRenderer.color = color;
        }

        transform.position = new Vector3(network.x, -1.5f, 0f);
        _spriteRenderer.transform.localScale = new Vector3(network.width, network.height, 1f);

        while (network.damage > _damageCreated.Count)
        {
            var damage = Instantiate(_damagePrefab, transform);
            damage.transform.localPosition = new Vector3(
                Random.Range(_damageOffset, network.width - _damageOffset),
                Random.Range(_damageOffset, network.height - _damageOffset),
                0f
            );
            _damageCreated.Add(damage);
        }
    }

}