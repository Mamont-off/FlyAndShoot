using System.Collections;
using UnityEngine;

namespace Gameplay
{
    /// <summary>
    /// Spawn/Respawn enemy in random range
    /// </summary>
    public sealed class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Enemy _baseEnemy;
        //TODO: Scriptable Object with parameters
        [SerializeField] private int _enemyCount;
        [SerializeField] private int _minRespawnDelay = 0;
        [SerializeField] private int _maxRespawnDelay = 5;

        private void Start()
        {
            CreateEnemies(_enemyCount);
            // _baseEnemy.OnLifeIsEnd += (enemy) => StartCoroutine(RespawnDelay(enemy));
            // _baseEnemy.Revive();
            _baseEnemy.gameObject.SetActive(false);
        }

        /// <summary>
        /// Instantiate enemies
        /// </summary>
        /// <param name="count">count of new enemy</param>
        private void CreateEnemies(int count)
        {
            for (; count > 0; count--)
            {
                var newEnemy = Instantiate(_baseEnemy);
                newEnemy.transform.position = GetSpawnPos();
                newEnemy.OnLifeIsEnd += (enemy) => StartCoroutine(RespawnDelay(enemy));//after enemy dies he will be revived
                newEnemy.Revive();
            }
        }

        /// <summary>
        /// Revive enemy after random delay
        /// </summary>
        /// <param name="e">dead enemy</param>
        private IEnumerator RespawnDelay(Enemy e)
        {
            e.gameObject.SetActive(false);
            e.transform.position = GetSpawnPos();
            yield return new WaitForSeconds(Random.Range(_minRespawnDelay, _maxRespawnDelay));
            e.gameObject.SetActive(true);
            e.Revive();
        }

        /// <summary>
        /// Return random point in space
        /// </summary>
        /// <returns>random point</returns>
        private Vector3 GetSpawnPos()
        {
            const int minRandom = -10;
            const int maxRandom = 10;
            var tempV3 = transform.position;
            tempV3.x += Random.Range(minRandom, maxRandom);
            tempV3.y += Random.Range(minRandom, maxRandom);
            tempV3.z += Random.Range(minRandom, minRandom);
            return tempV3;
        }
    }
}