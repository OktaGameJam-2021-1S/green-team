using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingNetworkSync : MonoBehaviour
{

    [SerializeField] private GameObject _damagePrefab = default;
    [SerializeField] private GameObject _plantPrefab = default;
    [SerializeField] private float _damageOffset = .2f;
    [SerializeField] private float _plantOffset = .2f;


    private int _peopleNumber;

    public int Peoples => _peopleNumber;

    private int _damage;
    private int _maxDamage;
    public int MaxDamage => _maxDamage;

    private int _plant;
    private int _maxPlant;
    public int MaxPlant => _maxPlant;


    private int _id;
    public int ID => _id;

    private int _height;
    public int Height => _height;
    
    private int _width;
    public int Width => _width;

    private List<GameObject> _damageCreated;
    private List<GameObject> _plantCreated;
    private List<GameObject> _graffitiCreated;

    public int GraffitiCount => _graffitiCreated.Count;

    public int DamageTaken => _damageCreated.Count;

    public int PlantCount => _plantCreated.Count;


    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _damageCreated = new List<GameObject>();
        _plantCreated = new List<GameObject>();
        _graffitiCreated = new List<GameObject>();
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

        while (network.plant > _plantCreated.Count)
        {
            PlantSeed();
        }

        _maxDamage = network.maxDamage;
        _maxPlant = network.maxPlant;
        _peopleNumber = network.people;
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

    public void PlantSeed()
    {
        var plant = Instantiate(_plantPrefab, transform);
        plant.transform.localPosition = new Vector3(
            Random.Range(_plantOffset, _spriteRenderer.transform.localScale.x - _plantOffset),
            Random.Range(_plantOffset, _spriteRenderer.transform.localScale.y - _plantOffset),
            0f
        );
        _plantCreated.Add(plant);
    }

    public void DealDamage()
    {
        _damage += 1;
        TakeDamage();
    }
    
    public void Seed()
    {
        _plant += 1;
        PlantSeed();
    }

}
