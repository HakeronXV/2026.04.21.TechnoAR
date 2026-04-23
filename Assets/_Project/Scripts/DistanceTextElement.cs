using TMPro;
using UnityEngine;

namespace _Project.Scripts
{
    
    public class DistanceTextElement : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        public void UpdateDistance(float distance)
        {
            string text = distance.ToString("F1") + "m";
            _text.SetText(text);
        }

        public void UpdatePosition(Vector3 position)
        {
            transform.position = position;
        }
    }
}