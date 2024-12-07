using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "BlindRoleSprites", menuName = "Game/BlindRoleSprites", order = 1)]
    public class BlindRoleSprites : ScriptableObject
    {
        public Sprite smallBlindSprite;
        public Sprite bigBlindSprite;
    }
}