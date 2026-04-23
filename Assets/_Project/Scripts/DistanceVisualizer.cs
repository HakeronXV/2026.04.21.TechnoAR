using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class DistanceVisualizer : MonoBehaviour
{
    private ObjectSpawner _objectSpawner;
    private LineRenderer _lineRenderer;
    
    
    private List<GameObject> _objectList;
    [SerializeField] private float _offset=.1f;
    [SerializeField] private GameObject _distanceTextPrefab;
    private List<DistanceTextElement> _textElementList;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _objectSpawner = GetComponent<ObjectSpawner>();
        _lineRenderer = GetComponent<LineRenderer>();
        
        _objectList = new List<GameObject>();
        _textElementList = new List<DistanceTextElement>();
    }

    private void OnEnable()
    {
        _objectSpawner.objectSpawned += OnObjectSpawn;
    }

    private void OnDisable()
    {
        _objectSpawner.objectSpawned -= OnObjectSpawn;
    }

    private void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject instance = Instantiate(_distanceTextPrefab, transform);
            _textElementList.Add(instance.GetComponent<DistanceTextElement>());
            instance.SetActive(false);
        }
    }

    private void OnObjectSpawn(GameObject obj)
    {
        Debug.Log("Object Spawned:  " + obj.name);
        DistanceVisualizerElement element = obj.GetComponent<DistanceVisualizerElement>();
        if (element != null)
        {
            element.OnDestroyEvent += ElementOnOnDestroyEvent;
            _objectList.Add(obj);
        }
    }

    private void ElementOnOnDestroyEvent(GameObject obj)
    {
        Debug.Log("Object Destroyed");
        _objectList.Remove(obj);
    }

    // Update is called once per frame
    void Update()
    { 
        _lineRenderer.positionCount = _objectList.Count;
        if (_objectList.Count > 1)
        {
            //Tracer la ligne
            for (int i = 0; i < _objectList.Count; i++)
            {
                _lineRenderer.SetPosition(i, _objectList[i].transform.position+_offset*Vector3.up);
            }

            foreach (var VARIABLE in _textElementList)
            {
                VARIABLE.gameObject.SetActive(false);
            }

            for (int i = 0; i < _objectList.Count-1; i++)
            {
                float distance = Vector3.Distance(_objectList[i].transform.position, _objectList[i+1].transform.position);
                _textElementList[i].UpdateDistance(distance);
                
                //Vector3 direction = (_objectList[i+1].transform.position- _objectList[i].transform.position).normalized;
                //var position = _objectList[i].transform.position + direction * (distance * .5f);
                Vector3 position = Vector3.Lerp(
                    _objectList[i].transform.position,
                    _objectList[i+1].transform.position,.5f);
                _textElementList[i].UpdatePosition(position+_offset*Vector3.up);
                _textElementList[i].gameObject.SetActive(true);
                
            }
        }
        if (_objectList.Count == 4)
        {
            _lineRenderer.positionCount = 5;
            _lineRenderer.SetPosition(4, _objectList[0].transform.position+_offset*Vector3.up);
            
            float distance = Vector3.Distance(_objectList[3].transform.position, _objectList[0].transform.position);
            _textElementList[3].UpdateDistance(distance);
            
            Vector3 position = Vector3.Lerp(
                _objectList[0].transform.position,
                _objectList[3].transform.position,.5f);
            _textElementList[3].UpdatePosition(position+_offset*Vector3.up);
            _textElementList[3].gameObject.SetActive(true);

            GameObject[] orderElements = ReorderPointsClockwise(_objectList);
            for (var i = 0; i < orderElements.Length; i++)
            {
                orderElements[i].GetComponent<DistanceVisualizerElement>().UpdateText(i.ToString());
            }

        }
        
    }
/// <summary>
/// Reorder my points clockwise. This is Fucking magical!
/// </summary>
/// <returns>
///     return the ordered list
/// </returns>
    private GameObject[] ReorderPointsClockwise(List<GameObject> elements)
    {
        Vector3 meanPoint = new Vector3();
        foreach (GameObject element in elements)
        {
            meanPoint+= element.transform.position;
        }
        meanPoint /= elements.Count;

        var result = elements.OrderBy(obj => Mathf.Atan2(
            obj.transform.position.x - meanPoint.x,
            obj.transform.position.z - meanPoint.z));
        return result.ToArray();
    }
}
