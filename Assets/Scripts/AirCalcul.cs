using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class AirCalcul : MonoBehaviour
{
    
    
    private LineRenderer _lineRenderer;
    private List<GameObject> _spawnedObjects;
    [SerializeField] private TextMeshPro areaText;
    private ObjectSpawner _objectSpawner;
    [SerializeField]private float _offset = 0.1f;
    [SerializeField]private GameObject _distanceBetweenObjects;
    private List<DistanceTextElement> _textElementList;

    void Awake()
    {
        _objectSpawner = GetComponent<ObjectSpawner>(); //allez chercher le composant script de l'object Spawner
        _lineRenderer = GetComponent<LineRenderer>();
        _spawnedObjects = new List<GameObject>(); // création de la list qui va contenir les objets spawned
        _textElementList = new List<DistanceTextElement>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject instance = Instantiate(_distanceBetweenObjects, transform);
            _textElementList.Add(instance.GetComponent<DistanceTextElement>());
            instance.SetActive(false);
        }
    }

    private void OnEnable()
    {
        _objectSpawner.objectSpawned += OnObjectSpawn; //allez chercher l'action "objets spawn du prefab" via le script de l'object Spawner
    }

    private void OnDisable()
    {
        _objectSpawner.objectSpawned -= OnObjectSpawn;
    }

    private void OnObjectSpawn(GameObject obj)
    {
        Debug.Log("Object Spawned:"  + obj.name);
        AirCalculElement element =  obj.GetComponent<AirCalculElement>();
        if (element != null)
        {
            element.OnDestroyEvent += ElementOnDestroyEvent;
            _spawnedObjects.Add(obj);
        }
    }

    private void ElementOnDestroyEvent(GameObject obj)
    {
        Debug.Log("Object Destroyed");
        _spawnedObjects.Remove(obj);
    }

    // Update is called once per frame
    void Update()
    {
        _lineRenderer.positionCount = _spawnedObjects.Count; //la liste de point est égale à la list d'objet (pour augmenter le nombre de point de la ligne)
        if (_spawnedObjects.Count > 1)
        {
            //Tracer la ligne "ligne renderer"
            for (int i = 0; i < _spawnedObjects.Count; i++)
            {
                _lineRenderer.SetPosition(i, _spawnedObjects[i].transform.position+_offset*Vector3.up); //récupère la position des cubes et mettre le point de la ligne dessus
            }

            foreach (var variable in _spawnedObjects)
            {
                variable.gameObject.SetActive(false);
            }
            
            for (int i = 0; i < _spawnedObjects.Count-1; i++)
            {
                float distance = Vector3.Distance(_spawnedObjects[i].transform.position, _spawnedObjects[i + 1].transform.position);
                _textElementList[i].UpdateDistance(distance);
                
                Vector3 position = Vector3.Lerp(
                    _spawnedObjects[i].transform.position,
                    _spawnedObjects[i + 1].transform.position,
                    .5f);
                _textElementList[i].UpdatePosition(position+_offset*Vector3.up);
                _textElementList[i].gameObject.SetActive(true);
            }
            if (_spawnedObjects.Count == 4)
            {
                _lineRenderer.positionCount = 5;
                _lineRenderer.SetPosition(4, _spawnedObjects[0].transform.position+_offset*Vector3.up);
                
                Vector3 position = Vector3.Lerp(
                    _spawnedObjects[0].transform.position,
                    _spawnedObjects[3].transform.position,.5f);
                _textElementList[3].UpdatePosition(position+_offset*Vector3.up);
                _textElementList[3].gameObject.SetActive(true);
                
                float distance = Vector3.Distance(_spawnedObjects[3].transform.position, _spawnedObjects[0].transform.position);
                _textElementList[3].UpdateDistance(distance);
                
                GameObject[] orderElements = ReorderPointsClockWise();
                for (var i = 0; i < orderElements.Length; i++)
                {
                    orderElements[i].GetComponent<AirCalculElement>().UpdateText(i.ToString());
                }
            }
        }
    }

    private GameObject[] ReorderPointsClockWise()
    {
        Vector3 meanPoint = new Vector3();
        foreach (GameObject element in _spawnedObjects)
        {
            meanPoint += element.transform.position;
        }
        meanPoint = meanPoint/_spawnedObjects.Count;

        var result = _spawnedObjects.OrderBy(obj => Mathf.Atan2(
            obj.transform.position.x,
            obj.transform.position.z));
        return result.ToArray();
    }
    
    private void AddCube(GameObject cube)
    {
        _spawnedObjects.Add(cube);
        Update();
    }

    private void Distance()
    {

    }

    float CalculateArea()
    {
        float area = 0f; 
        int j = _spawnedObjects.Count - 1;
        for (int i = 0; i < _spawnedObjects.Count; i++)
        {
            Vector3 p1 = _spawnedObjects[i].transform.position;
            Vector3 p2 = _spawnedObjects[j].transform.position;
            area += (p2.x + p1.x) * (p2.z - p1.z);
        }
        return Mathf.Abs(area/2f);
    }
}
