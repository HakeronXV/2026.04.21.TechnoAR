using System;
using UnityEngine;

public class ChangeColorIfNearby : MonoBehaviour
{
    [SerializeField, Tooltip("Distance at which we change the color")] private float _colorSwapThreshold = 1.4f;
    [SerializeField, Tooltip("Default Material of this cube")] private Material _defaultMaterial;
    [SerializeField] private Material _nearbyMaterial;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private TMPro.TMP_Text _text;
    [SerializeField] private Transform _textContainer;
    
    private Camera _camera;
    [SerializeField] private float _lerp;
    [SerializeField] private float _rotationSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(_renderer == null) Debug.LogWarning("No MeshRenderer found!");
    }

    private void Start()
    {
        _camera = Camera.main;
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 direction = _text.transform.position - _camera.transform.position;
        direction = Vector3.ProjectOnPlane(direction, Vector3.up);
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        /*Quaternion rotation = Quaternion.RotateTowards(_text.transform.rotation,
            targetRotation, 
            Time.deltaTime * _rotationSpeed);*
        
        
        /*Quaternion rotation = Quaternion.Lerp(
            _text.transform.rotation,
            targetRotation,
            _lerp*Time.deltaTime);*/
        
        //_text.transform.position += new Vector3(5,5,5);
        _text.transform.rotation *= Quaternion.Euler(0,90*Time.deltaTime,0);


        float sqrDistance =  direction.sqrMagnitude;

        if (sqrDistance > Mathf.Pow(_colorSwapThreshold,2)) _renderer.material = _defaultMaterial;
        else _renderer.material = _nearbyMaterial;
        
        _text.SetText(Mathf.Sqrt(sqrDistance).ToString("F2")); 
        
    }
}


//Quaternion rotation = Quaternion.Lerp(_text.transform.rotation,  targetRotation, _lerp*Time.deltaTime);
//Quaternion rotation = Quaternion.RotateTowards(_text.transform.rotation, targetRotation, _rotationSpeed);
//