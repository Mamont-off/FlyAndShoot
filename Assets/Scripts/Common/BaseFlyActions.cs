using Gameplay;
using UnityEngine;

namespace Common
{
    public class BaseFlyActions : MonoBehaviour, IMove, IAcceleration, IRotation, IShoot
    {
        [Header("Action")]
        [SerializeField] private AbstractBulletSpawner _bulletSpawner;

        protected Transform _cachedTransform;
        private int _accelerate;

        protected void Awake()
        {
            _cachedTransform = transform;
        }

        public void Accelerate(int strValue)
        {
            _accelerate = strValue;
        }

        public void Move(int speed)
        {
            _cachedTransform.Translate(Vector3.up * (speed + _accelerate) * Time.deltaTime, Space.Self);
        }

        public void Rotation(float verticalRotation, float horizontalRotation)
        {
            _cachedTransform.Rotate(verticalRotation * Time.deltaTime, horizontalRotation * Time.deltaTime, 0);
        }

        public void Shoot(Transform buleetSpawnPos)
        {
            _bulletSpawner.BulletFly(buleetSpawnPos);
        }
    }
}