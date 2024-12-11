using System;
using UnityEngine;
using System.Collections.Generic;
using Code.Enums;
using Code.View;
using UnityEngine.Serialization;

namespace Code.GameEntities.Player
{
    public class PlayerModel : MonoBehaviour
    {
        [SerializeField] public string name = "";
        [SerializeField] public PlayerHand hand;
        [SerializeField] public ChipsManager stackChipsManager;
        [SerializeField] public ChipsManager betChipsManager;

        [SerializeField] public PlayerRoleManager playerRoleManager;
        [SerializeField] public PlayerActionsManager playerActionsManager;

        public void Initialize()
        {
            stackChipsManager.Initialize();
            hand.Initialize();
            playerActionsManager.Initialize(hand);
        }

        public void Reset()
        {
            playerActionsManager.Reset();
            playerRoleManager.Reset();
            hand.Reset();
        }

        public int GetPlayerInGameChipsAmount()
        {
            return stackChipsManager.TotalChips + betChipsManager.TotalChips;
        }
    }
}