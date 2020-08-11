using UnityEngine;
using Common;

namespace Gameplay
{
    public abstract class AbstractEnemy : BaseFlyActions
    {
        //TODO: Scriptable Object with parameters
        [Header("Parameters")]
        [SerializeField] private int _speed = 5;
        [SerializeField] private int _verticalSpeed = 80;
        [SerializeField] private int _horizontalSpeed = 80;

        [Space]
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Transform _mark;
        [SerializeField] private Camera _camera;

        protected bool _isLive = false;
        protected float _verticalAngle, _horizontalAngle;
        protected abstract void LifeIsEnd();
        protected abstract void ReviveEvent();
        protected void Start()
        {
            tag = Tag.Enemy;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Tag.Finish))
            {
                LifeIsEnd();
            }
        }

        private void Update()
        {
            if (_isLive)
            {
                Move(_speed);
                Rotation( _verticalAngle * _verticalSpeed, _horizontalAngle * _horizontalSpeed);
                ShowMarkOnScreen();
            }
        }

        private void OnBecameInvisible()
        {
            if(_mark){_mark.gameObject.SetActive(true);}
        }

        private void OnBecameVisible()
        {
            if(_mark){_mark.gameObject.SetActive(false);}
        }

        /// <summary>
        /// Show marks on the edge of the screen
        /// </summary>
        private void ShowMarkOnScreen()
        {
            bool notOnScreen = !_meshRenderer.isVisible;
            if (notOnScreen)
            {
                Vector3 viewportPoint = _camera.WorldToViewportPoint(_cachedTransform.position);
                viewportPoint = GetPosOnScreen(viewportPoint);
                _mark.position = _camera.ScreenToWorldPoint(viewportPoint);
            }
        }

        /// <summary>
        /// Position on screen for mark
        /// </summary>
        private Vector3 GetPosOnScreen(Vector3 viewport)
        {
            const int markZpos = 5;

            const int topScreen = 1;
            const int bottomScreen = 0;
            const int rightScreen = 1;
            const int leftScreen = 0;

            if (viewport.z < 0)//enemy behind us
            {
                viewport = Vector3.one - viewport;
            }

            viewport.x = Mathf.Clamp01(viewport.x);
            viewport.y = Mathf.Clamp01(viewport.y);

            if (viewport.x < rightScreen && viewport.y < topScreen)
            {
                if (viewport.x > viewport.y)
                {
                    viewport.y = bottomScreen;
                }
                else
                {
                    viewport.x = leftScreen;
                }
            }
            viewport.x *= Screen.width;
            viewport.y *= Screen.height;
            viewport.z = markZpos;
            return viewport;
        }

        /// <summary>
        /// Enable enemy
        /// </summary>
        public void Revive()
        {
            _isLive = true;
            if(_mark.parent != _camera.transform)
            {
                _mark.SetParent(_camera.transform);
            }
            ReviveEvent();
        }
    }
}