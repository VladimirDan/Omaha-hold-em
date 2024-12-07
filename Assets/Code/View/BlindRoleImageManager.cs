using UnityEngine;
using UnityEngine.UI;
using Code.Enums;
using ScriptableObjects;

namespace Code.View
{
    public class BlindRoleImageManager : MonoBehaviour
    {
        [SerializeField] private Image roleImage;
        [SerializeField] private BlindRoleSprites sprites;

        public void UpdateRoleImage(PlayerRole role)
        {
            switch (role)
            {
                case PlayerRole.SmallBlind:
                    roleImage.sprite = sprites.smallBlindSprite;
                    roleImage.enabled = true;
                    break;
                case PlayerRole.BigBlind:
                    roleImage.sprite = sprites.bigBlindSprite;
                    roleImage.enabled = true;
                    break;
                case PlayerRole.Regular:
                    roleImage.enabled = false;
                    break;
            }
        }
    }
}