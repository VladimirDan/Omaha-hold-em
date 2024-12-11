using System.Collections.Generic;
using UnityEngine;
using Code.Enums;
using Code.Data;
using Code.View;
using UnityEngine.Serialization;

namespace Code.GameEntities.Player
{
    public class PlayerRoleManager : MonoBehaviour
    {
        [SerializeField] public BlindRoleImageManager blindRoleImageManager;

        private PlayerRole role;

        public PlayerRole Role
        {
            get => role;
            set
            {
                if (role != value) 
                {
                    role = value;
                    blindRoleImageManager.UpdateRoleImage(role); 
                }
            }
        }

        public void Reset()
        {
            Role = PlayerRole.Regular;
        }
        
        public void SetRole(PlayerRole newRole)
        {
            Role = newRole;
            //UpdateRoleImage();
        }

        private void UpdateRoleImage()
        {
            blindRoleImageManager.UpdateRoleImage(Role);
        }
    }
}