using UnityEngine;
using Common;

namespace Gameplay
{
    /// <summary>
    /// Control for all bullets (pool)
    /// </summary>
    public sealed class BulletSpawner : AbstractBulletSpawner
    {
        [SerializeField] private Bullet _bullet;
        private IPool<Bullet> _bulletPool;
        private void Start()
        {
            const int poolSize = 50;
            const int poolGrowth = 5;
            _bulletPool = new SimplePool<Bullet>(_bullet, transform, poolSize, poolGrowth);
        }

        /// <summary>
        /// Create bullet in position
        /// </summary>
        /// <param name="spawnPos">position for bullet creating</param>
        public override void BulletFly(Transform spawnPos)
        {
            var newBullet = _bulletPool.GetOnPool();
            newBullet.SetMyPool(_bulletPool);

            var bTransform = newBullet.transform;
            bTransform.position = spawnPos.position;
            bTransform.rotation = spawnPos.rotation;
            bTransform.SetParent(null);

            newBullet.Fly = true;
            newBullet.gameObject.SetActive(true);
        }
    }
}