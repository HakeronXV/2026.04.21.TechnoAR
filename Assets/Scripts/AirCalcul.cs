using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class AirCalcul : MonoBehaviour
{
    
    
    [SerializeField]private LineRenderer _lineRenderer;
    private List<GameObject> _spawnedCubes = new List<GameObject>();
    [SerializeField] private TextMeshPro areaText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _spawnedCubes.Count; i++)
        {
            _lineRenderer.SetPosition(i, _spawnedCubes[i].transform.position);
            _lineRenderer.SetPosition(_spawnedCubes.Count, _spawnedCubes[0].transform.position);
        }
        
        
    }
    private void AddCube(GameObject cube)
    {
        _spawnedCubes.Add(cube);
        Update();
    }

    float CalculateArea()
    {
        float area = 0f; 
        int j = _spawnedCubes.Count - 1;
        for (int i = 0; i < _spawnedCubes.Count; i++)
        {
            Vector3 p1 = _spawnedCubes[i].transform.position;
            Vector3 p2 = _spawnedCubes[j].transform.position;
            area += (p2.x + p1.x) * (p2.z - p1.z);
        }
        return Mathf.Abs(area/2f);
    }
}
