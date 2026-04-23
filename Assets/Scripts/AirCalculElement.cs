using System;
using TMPro;
using UnityEngine;

public class AirCalculElement : MonoBehaviour
{
    public event Action<GameObject> OnDestroyEvent;

    [SerializeField] private TMP_Text _text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        OnDestroyEvent?.Invoke(gameObject);
    }

    public void UpdateText(string text)
    {
        _text.text = text;
    }
}
