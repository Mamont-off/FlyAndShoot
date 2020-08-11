using UnityEngine;

namespace Gameplay
{
    public abstract class AbstractBulletSpawner : MonoBehaviour
    {
        public abstract void BulletFly(Transform createPlace);
    }
}