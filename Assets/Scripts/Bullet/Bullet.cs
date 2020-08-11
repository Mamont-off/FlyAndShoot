using Common;
using UnityEngine;

namespace Gameplay
{    
    /// <summary>
    /// Script for bullets
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public sealed class Bullet : MonoBehaviour, IPoolObject
    {
        public bool Fly = false;
        //TODO: Scriptable Object with parameters
        [SerializeField] private float _speed = 20;
        [SerializeField] private int _lifetime = 5;
        private Transform _cachedTransform;
        private float _currentFlyTime;
        private IPool<Bullet> _myPool;

        private void Start()
        {
            _cachedTransform = transform;
            tag = Tag.Finish;
        }
        private void Update()
        {
            if (Fly)
            {
                _cachedTransform.Translate(Vector3.up * _speed * Time.deltaTime, Space.Self);
                _currentFlyTime += Time.deltaTime;
                if (_currentFlyTime > _lifetime)
                {
                    EndFly();
                }
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Tag.Enemy))
            {
                EndFly();
            }
        }

        /// <summary>
        /// Stop bullet flying
        /// </summary>
        private void EndFly()
        {
            Fly = false;
            _currentFlyTime = 0;
            ReturnToPool();
        }
        
        internal void SetMyPool(IPool<Bullet> pool)
        {
            _myPool = pool;
        }

        public void ReturnToPool()
        {
            _myPool.SetToPool(this);
        }
    }
}