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

        public PlayerRole role;

        public void SetRole(PlayerRole newRole)
        {
            role = newRole;
            UpdateRoleImage();
        }

        private void UpdateRoleImage()
        {
            blindRoleImageManager.UpdateRoleImage(role);
        }
    }
}