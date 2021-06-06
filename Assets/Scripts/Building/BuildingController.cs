using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{

    public delegate void DemolishHandler();
    public event DemolishHandler OnDemolish;

    [SerializeField] GameObject _baseFloor;
    [SerializeField] GameObject _topFloor;
    [SerializeField] List<GameObject> _variableFloors;

    [SerializeField] Transform _spawnRoot;
    [SerializeField] Transform _demolishedRoot;

    [SerializeField] GameObject _peoplePrefab;

    private BuildingFloor _lastFloorCreated;

    private List<BuildingFloor> _floors;

    private BuildingNetwork _buildingNetworkReference;

    public int PeopleInBuilding { get; set; }

    private int _numInterationsDone;

    public bool Demolished
    {
        get
        {
            return _numInterationsDone >= _floors.Count;
        }
    }

    public int Graffiti
    {
        get
        {
            int value = 0;

            for(int i = 0; i < _floors.Count; i++)
            {
                value += _floors[i].Graffiti ? 1 : 0;
            }

            return value;
        }
    }
    public int Naturalized
    {
        get
        {
            int value = 0;

            for (int i = 0; i < _floors.Count; i++)
            {
                value += _floors[i].Naturalized ? 1 : 0;
            }

            return value;
        }
    }

    public int DamageTaken
    {
        get
        {
            int value = 0;

            for (int i = 0; i < _floors.Count; i++)
            {
                value += _floors[i].Destroyed ? 1 : 0;
            }

            return value;
        }
    }

    public List<PlayerMovement> PlayersInside { get; set; }

    public void Setup(BuildingNetwork pBuildingData)
    {
        _spawnRoot.gameObject.SetActive(true);
        _demolishedRoot.gameObject.SetActive(false);
        _numInterationsDone = 0;
        PlayersInside = new List<PlayerMovement>();
        PeopleInBuilding = pBuildingData.people;

        _floors = new List<BuildingFloor>();
        _buildingNetworkReference = pBuildingData;
        
        int amount = _buildingNetworkReference.height - 1;

        _lastFloorCreated = Instantiate(_baseFloor, _spawnRoot).GetComponent<BuildingFloor>();
        _floors.Add(_lastFloorCreated);
        while (amount > 0)
        {
            amount--;

            int index = Random.Range(0, _variableFloors.Count);

            var prefab = _variableFloors[index];

            var spawned = Instantiate(prefab, _spawnRoot);
            spawned.transform.position = new Vector3(spawned.transform.position.x, _lastFloorCreated.transform.position.y + (_lastFloorCreated.Collider.size.y * _lastFloorCreated.transform.localScale.y * transform.localScale.y), spawned.transform.position.z);
            _lastFloorCreated = spawned.GetComponent<BuildingFloor>();
            _floors.Add(_lastFloorCreated);
        }

        var topFloor = Instantiate(_topFloor, _spawnRoot);
        topFloor.transform.position = new Vector3(topFloor.transform.position.x, _lastFloorCreated.transform.position.y + (_lastFloorCreated.Collider.size.y * _lastFloorCreated.transform.localScale.y * transform.localScale.y), topFloor.transform.position.z);

    }

    public void DealDamageFloor()
    {
        if (_numInterationsDone >= _buildingNetworkReference.height)
        {
            DemolishBuilding();
        }
        else
        {
            _numInterationsDone++;
            BuildingFloor floor = GetRandomAvaibleFloor();
            floor.DamageFloor();
        }
    }

    public void NaturalizeFloor()
    {
        if (_numInterationsDone >= _buildingNetworkReference.height)
        {
            DemolishBuilding();
        }
        else
        {
            _numInterationsDone++;
            BuildingFloor floor = GetRandomAvaibleFloor();
            floor.Naturalize();
        }
    }

    public void GraffitiFloor()
    {
        if (_numInterationsDone >= _buildingNetworkReference.height)
        {
            DemolishBuilding();
        }
        else
        {
            _numInterationsDone++;
            BuildingFloor floor = GetRandomAvaibleFloor();
            floor.GraffitiFloor();
        }
    }

    public void AirHorn()
    {
        if (PeopleInBuilding > 0)
        {
            Instantiate(_peoplePrefab, transform.position, Quaternion.identity);
            PeopleInBuilding -= 1;
        }
    }

    public void DemolishBuilding()
    {
        Debug.Log("Puft");
        _spawnRoot.gameObject.SetActive(false);
        _demolishedRoot.gameObject.SetActive(true);

        OnDemolish?.Invoke();
    }

    public void ReconstructBuilding()
    {
        for (int i = 0; i < _floors.Count; i++)
        {
            _floors[i].Reset();
        }
        _spawnRoot.gameObject.SetActive(true);
        _demolishedRoot.gameObject.SetActive(false);
        _numInterationsDone = 0;
    }

    private BuildingFloor GetRandomAvaibleFloor()
    {
        List<BuildingFloor> availablesFloors = new List<BuildingFloor>();
        for(int i = 0; i < _floors.Count; i++)
        {
            if (_floors[i].Interactable)
                availablesFloors.Add(_floors[i]);
        }

        int index = Random.Range(0, availablesFloors.Count);

        return availablesFloors[index];
    }

}
