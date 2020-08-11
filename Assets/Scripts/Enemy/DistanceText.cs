using TMPro;
using UnityEngine;

namespace Gameplay
{
    /// <summary>
    /// Rotate the distance text face to player
    /// </summary>
    public sealed class DistanceText : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _text;
        [SerializeField] private Transform _player;
        [SerializeField] private Transform _camera;

        const int _lockRotateYAngle = 180;
        private Transform _cachedtransform;

        private void Start()
        {
            _cachedtransform = transform;
        }
        private void LateUpdate()
        {
            _cachedtransform.LookAt(_camera);
            _cachedtransform.Rotate(0, _lockRotateYAngle, 0);
            int distance = (int)(_cachedtransform.position - _player.position).sqrMagnitude;
            _text.text = distance.ToString();
        }
    }
}