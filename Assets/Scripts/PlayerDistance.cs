using TMPro;
using UnityEngine;

public class PlayerDistance : MonoBehaviour
{
    [SerializeField, Tooltip("Distance at which we change the color")]
    private float _colorSwapLimit = 1f;
    [SerializeField]private TMP_Text _text;
    private Camera _cam;
    private Vector3 _difDistance;
    [SerializeField]private MeshRenderer _meshRenderer;
    [SerializeField]private Material _materialOri;
    [SerializeField] private Material _materialRed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        _difDistance = transform.position - _cam.transform.position;
        float distance = _difDistance.magnitude;
        _text.text = "Distance: " + distance.ToString("F2");
        var rotation = _text.transform.position - _cam.transform.position;
        rotation.y = 0;
        _text.transform.rotation = Quaternion.LookRotation(rotation);
        _meshRenderer.material = distance > _colorSwapLimit ? _materialRed : _materialOri;
    }
}
