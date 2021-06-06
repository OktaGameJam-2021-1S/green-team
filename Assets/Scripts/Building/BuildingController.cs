using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{

    public delegate void DemolishHandler();
    public event DemolishHandler OnDemolish;

    public GameConfiguration _configurationAsset;

    [SerializeField] GameObject _baseFloor;
    [SerializeField] GameObject _topFloor;
    [SerializeField] List<GameObject> _variableFloors;
    [SerializeField] private BuildCount _playerCount;
    [SerializeField] private BuildCount _personCount;
    [SerializeField] private BuildCount _scoreCount;

    [SerializeField] Transform _spawnRoot;
    [SerializeField] Transform _demolishedRoot;

    [SerializeField] GameObject _peoplePrefab;

    [SerializeField] GameObject _alertPlant;
    [SerializeField] GameObject _alertHammer;

    private BuildingFloor _lastFloorCreated;

    private List<BuildingFloor> _floors;

    private BuildingNetwork _buildingNetworkReference;


    public int PeopleInBuilding { get; set; }

    private int _maxDamage;
    private int _maxPlant;

    public bool Demolished { get; private set; }

    public int Graffiti
    {
        get
        {
            int value = 0;

            for (int i = 0; i < _floors.Count; i++)
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
        _scoreCount.gameObject.SetActive(false);
        _spawnRoot.gameObject.SetActive(true);
        _demolishedRoot.gameObject.SetActive(false);
        _maxDamage = pBuildingData.maxDamage;
        _maxPlant = pBuildingData.maxPlant;

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

        _playerCount.gameObject.transform.position = new Vector3(_playerCount.transform.position.x, topFloor.transform.position.y + 0.3f, _playerCount.transform.position.z);
        _personCount.gameObject.transform.position = new Vector3(_personCount.transform.position.x, topFloor.transform.position.y + 0.3f, _personCount.transform.position.z);


        _alertHammer.transform.position = new Vector3(_alertHammer.transform.position.x, topFloor.transform.position.y + 0.3f, _alertHammer.transform.position.z);
        _alertPlant.transform.position = new Vector3(_alertPlant.transform.position.x, topFloor.transform.position.y + 0.3f, _alertPlant.transform.position.z);

        UpdateMarkers();

    }

    public void UpdateMarkers()
    {
        if(PeopleInBuilding > 0)
        {
            _personCount.gameObject.SetActive(true);
            _personCount.SetText(PeopleInBuilding.ToString());
        }
        else
        {
            _personCount.gameObject.SetActive(false);
        }


        if (PlayersInside.Count > 0)
        {
            _playerCount.gameObject.SetActive(true);
            _playerCount.SetText(PlayersInside.Count.ToString());
        }
        else
        {
            _playerCount.gameObject.SetActive(false);
        }

        _alertPlant.SetActive((Naturalized >= _maxPlant && !Demolished));
        _alertHammer.SetActive((DamageTaken >= _maxDamage && !Demolished));

    }

    public void DealDamageFloor()
    {
        if (_maxDamage <= DamageTaken)
        {
            DemolishBuilding();
        }
        else
        {
            BuildingFloor floor = GetRandomAvaibleFloor();
            floor.DamageFloor();
            UpdateMarkers();
        }
    }

    public void NaturalizeFloor()
    {
        if (_maxPlant <= Naturalized)
        {
            DemolishBuilding();
        }
        else
        {
            BuildingFloor floor = GetRandomAvaibleFloor();
            floor.Naturalize();
            UpdateMarkers();
        }
    }

    public void GraffitiFloor()
    {
        // WIP
    }

    public void AirHorn()
    {
        if (PeopleInBuilding > 0)
        {
            StartCoroutine(AirHornAction());
        }
    }

    public void Yell()
    {
        if (PeopleInBuilding > 0)
        {
            SpawnCitizen();
            PeopleInBuilding--;
            UpdateMarkers();
        }
    }

    public void DemolishBuilding()
    {
        Demolished = true;
        Debug.Log("Puft");
        _spawnRoot.gameObject.SetActive(false);
        _demolishedRoot.gameObject.SetActive(true);
        _alertHammer.SetActive(false);
        _alertPlant.SetActive(false);
        _playerCount.gameObject.SetActive(false);
        _personCount.gameObject.SetActive(false);
        UpdateMarkers();
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
        Demolished = false;
    }

    private BuildingFloor GetRandomAvaibleFloor()
    {
        List<BuildingFloor> availablesFloors = new List<BuildingFloor>();
        for (int i = 0; i < _floors.Count; i++)
        {
            if (_floors[i].Interactable)
                availablesFloors.Add(_floors[i]);
        }

        int index = Random.Range(0, availablesFloors.Count);

        return availablesFloors[index];
    }

    IEnumerator AirHornAction()
    {
        float fTime = 0;
        float fRelease = 0.3f;
        while (PeopleInBuilding > 0)
        {
            if (fTime >= fRelease)
            {
                SpawnCitizen();
                fTime = 0f;
                PeopleInBuilding--;
            }
            fTime += Time.deltaTime;
            UpdateMarkers();
            yield return null;
        }

        PeopleInBuilding -= 1;
    }

    private void SpawnCitizen() {
        Instantiate(_peoplePrefab, transform.position, Quaternion.identity);
    }

    public bool IsIdeal()
    {
        List<BuildingFloor> availablesFloors = new List<BuildingFloor>();
        for (int i = 0; i < _floors.Count; i++)
        {
            if (_floors[i].Interactable) return false;
        }
        return true;
    }

    public void ShowScore()
    {
        _playerCount.gameObject.SetActive(false);
        _personCount.gameObject.SetActive(false);
        _alertPlant.SetActive(false);
        _alertHammer.SetActive(false);

        _scoreCount.gameObject.SetActive(true);

        if (IsIdeal())
        {
            Score score = _configurationAsset.Scores.Find(x => (ScoreType)x.ScoreType == ScoreType.IdealBuilding);
            _scoreCount.SetText(score.Amount.ToString());
        }
        else
        {
            Score score = _configurationAsset.Scores.Find(x => (ScoreType)x.ScoreType == ScoreType.NonIdealBuilding);
            _scoreCount.SetText(score.Amount.ToString());
            _scoreCount.SetColor(Color.red);
        }
    }

}
