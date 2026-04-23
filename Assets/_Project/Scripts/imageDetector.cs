using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Object = UnityEngine.Object;

public class imageDetector : MonoBehaviour
{
    private ARTrackedImageManager _trackedImageManager;

    

    private void Awake()
    {
        _trackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnEnable()
    {
        _trackedImageManager.trackablesChanged.AddListener(OnChanged);
    }

    private void OnDisable()
    {
        _trackedImageManager.trackablesChanged.RemoveListener(OnChanged);
    }

    private void OnChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        foreach (ARTrackedImage image in eventArgs.added) //le "ARTrackedImage est la variable donc le "var" peut le remplacer mais il faut savoir de quoi ça parle.
        {
            string imageName = image.referenceImage.name;
            Debug.Log("Image Added: " + image.referenceImage.name);
            Vector3 spawnPosition = image.transform.position;
            switch (imageName)
            {
                case "one":
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.SetParent(image.transform, false);
                    cube.transform.localScale = new Vector3(.1f, .1f, .1f);
                    break;
                case "unitylogowhiteonblack":
                    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphere.transform.SetParent(image.transform, false);
                    sphere.transform.localScale = new Vector3(.1f, .1f, .1f);
                    break;
                default:
                    break;
            }
        }
        foreach (var image in eventArgs.updated)
        {
            
        }

        foreach (var image in eventArgs.removed)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
