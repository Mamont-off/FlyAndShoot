using UnityEngine;

namespace Gameplay
{
    /// <summary>
    /// Camera script for following the player
    /// </summary>
    public sealed class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _lerpTime = 10;

        private Transform _cachedTransform;

        private Vector3 _positionOffset;
        private Quaternion _rotationOffset;
        private Quaternion _baseRotation;

        private void Start()
        {
            _cachedTransform = transform;

            _positionOffset = _cachedTransform.position - _target.position;
            _rotationOffset = _cachedTransform.rotation * Quaternion.Inverse(_target.rotation);

            _baseRotation = _target.rotation;
        }

        private void LateUpdate()
        {
            var deltaRotation = _target.rotation * Quaternion.Inverse(_baseRotation);
            var resultPosition = _target.position + deltaRotation * _positionOffset;
            
            _cachedTransform.position = resultPosition;
            _cachedTransform.rotation = Quaternion.Lerp(_cachedTransform.rotation, _target.rotation * _rotationOffset, _lerpTime * Time.deltaTime);
        }
    }
}