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
    [SerializeField]private float _lerp = 0.2f;
    private bool _isNearby;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        var direction = _text.transform.position - _cam.transform.position;
        direction = Vector3.ProjectOnPlane(direction, Vector3.up);
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        /*Quaternion rotation = Quaternion.Lerp(
            _text.transform.rotation,
            targetRotation,
            _lerp*Time.deltaTime);
        _text.transform.rotation = rotation;*/
        float sqrDistance = direction.sqrMagnitude;
        if (sqrDistance > Mathf.Pow(_colorSwapLimit,2)) _meshRenderer.material = _materialOri;
        else _meshRenderer.material = _materialRed;
        _text.text = "Distance: " + Mathf.Sqrt(sqrDistance).ToString("F2");
        //_text.transform.position += new Vector3(5, 5, 5); // Animation de mouvement
        _text.transform.rotation *= Quaternion.Euler(0, 90*Time.deltaTime, 0); // Animation de rotation sur soi-même
    }
}
