using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingNetworkSync : MonoBehaviour
{

    [SerializeField] private GameObject _damagePrefab = default;
    [SerializeField] private float _damageOffset = .2f;

    private int _id;
    public int ID => _id;

    private int _height;
    public int Height => _height;
    
    private int _width;
    public int Width => _width;

    private List<GameObject> _damageCreated;
    private List<GameObject> _plantCreated;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _damageCreated = new List<GameObject>();
        _plantCreated = new List<GameObject>();
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

        _height = network.height;
        _width = network.width;

        transform.position = new Vector3(network.x, -1.5f, 0f);
        _spriteRenderer.transform.localScale = new Vector3(network.width, network.height, 1f);

        while (network.damage > _damageCreated.Count)
        {
            TakeDamage();
        }
    }


    public void TakeDamage()
    {
        var damage = Instantiate(_damagePrefab, transform);
        damage.transform.localPosition = new Vector3(
            Random.Range(_damageOffset, _spriteRenderer.transform.localScale.x - _damageOffset),
            Random.Range(_damageOffset, _spriteRenderer.transform.localScale.y - _damageOffset),
            0f
        );
        _damageCreated.Add(damage);
    }

    public int DamageTaken()
    {
        return _damageCreated.Count;
    }

    public int PlantCount()
    {
        return _plantCreated.Count;
    }

    public void DealDamage()
    {
        NetworkController.Instance.SendDamageBuilding(new DamageBuildingNetwork()
        {
            id = _id
        });
    }

}
