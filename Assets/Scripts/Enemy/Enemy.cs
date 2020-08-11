using UnityEngine;
using Common;
using System;
using System.Collections;
using Random = UnityEngine.Random;

namespace Gameplay
{
    /// <summary>
    /// Enemy class
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public sealed class Enemy : AbstractEnemy
    {
        /// <summary>
        /// Invoke Event when enemy is died
        /// </summary>
        public event Action<Enemy> OnLifeIsEnd = (enemy) => { };

        [Space]
        [SerializeField] private Transform _bulletSpawnPos;

        private Coroutine _rotateRoutine;
        private Coroutine _shootRoutine;
        
        /// <summary>
        /// While enemy is live, he gets random rotation direction and delay after
        /// </summary>
        private IEnumerator RandomRotate()
        {
            const int minRotateAngle = -1;
            const int maxRotateAngle = 1;
            const int minChangeDirectionTime = 1;
            const int maxChangeDirectionTime = 4;

            while (_isLive)
            {
                _verticalAngle = Random.Range(minRotateAngle, maxRotateAngle);
                _horizontalAngle = Random.Range(minRotateAngle, maxRotateAngle);
                yield return new WaitForSeconds(Random.Range(minChangeDirectionTime, maxChangeDirectionTime));
            }
            _rotateRoutine = null;
        }

        /// <summary>
        /// While enemy is live, check for shoot with delay after
        /// </summary>
        private IEnumerator WantShoot()
        {
            const int minShootDelay = 1;
            const int maxShootDelay = 3;

            while (_isLive)
            {
                if (CheckPlayer())
                {
                    Shoot(_bulletSpawnPos);
                }
                yield return new WaitForSeconds(Random.Range(minShootDelay, maxShootDelay));
            }
            _shootRoutine = null;
        }

        /// <summary>
        /// Check player in front of
        /// </summary>
        /// <returns>check result</returns>
        private bool CheckPlayer()
        {
            const int checkDistance = 100;
            const int debugLineTime = 2;
            RaycastHit hit;
            if (Physics.Raycast(_cachedTransform.position, _cachedTransform.TransformDirection(Vector3.up), out hit, checkDistance))
            {
                if (hit.collider.CompareTag(Tag.Player))
                {
                    return true;
                }
            }
            Debug.DrawLine(transform.position, _cachedTransform.position + _cachedTransform.TransformDirection(Vector3.up) * checkDistance, Color.red, debugLineTime);
            return false;
        }

        /// <summary>
        /// Dead enemy
        /// </summary>
        protected override void LifeIsEnd()
        {
            _isLive = false;
            OnLifeIsEnd.Invoke(this);
        }

        protected override void ReviveEvent()
        {
            if(_rotateRoutine == null) { _rotateRoutine = StartCoroutine(RandomRotate());}
            if(_shootRoutine == null) { _shootRoutine = StartCoroutine(WantShoot());}
        }
    }
}