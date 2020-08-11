using Common;
using UnityEngine;
//TODO: add flight restrictions zone? (float error)
namespace Gameplay
{
    /// <summary>
    /// Player control
    /// </summary>
    public sealed class Player : BaseFlyActions
    {
        #region Serialize
        [SerializeField] private Transform _bulletSpawnPos;
        //TODO: Scriptable Object with parameters
        [Header("Parameters")]
        [SerializeField] private int _playerUpSpeed = 1;
        [SerializeField] private int _verticalSpeed = 100;
        [SerializeField] private int _horizontalSpeed = 100;
        [SerializeField] private int _accelerateValue = 5;
        #endregion
        
        const string Horizontal = "Horizontal";
        const string Vertical = "Vertical";
        private void Start()
        {   
            tag = Tag.Player;
        }

        private void Update()
        {
            PlayerRotation();
            Move(_playerUpSpeed);
            ShootCheck();
            AccelerateCheck();
        }

        /// <summary>
        /// Accelerate player
        /// </summary>
        private void AccelerateCheck()
        {
            if (Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.LeftShift))
            {
                Accelerate(_accelerateValue);
            }

            if (Input.GetKeyUp(KeyCode.RightShift) || Input.GetKeyUp(KeyCode.LeftShift))
            {
                Accelerate(0);
            }
        }

        /// <summary>
        /// Shoot
        /// </summary>
        private void ShootCheck()
        {
            if (Input.GetKeyUp(KeyCode.RightControl) || Input.GetKeyUp(KeyCode.LeftControl))
            {
                Shoot(_bulletSpawnPos);
            }
        }

        /// <summary>
        /// Player rotation
        /// </summary>
        private void PlayerRotation()
        {
            float VerticalRotation = Input.GetAxis(Vertical) * _verticalSpeed;
            float HorizontalRotation = Input.GetAxis(Horizontal) * _horizontalSpeed;
            Rotation(VerticalRotation, HorizontalRotation);
        }
    }
}