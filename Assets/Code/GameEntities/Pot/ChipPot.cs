using System;
using System.Collections.Generic;
using Code.GameEntities.Player;
using UnityEngine;
using Code.Enums;
using System.Linq;

namespace Code.GameEntities.Pot
{
    public class ChipPot
    {
        public int TotalAmount { get; set; }
        public List<PlayerModel> EligiblePlayers { get; set; }
    }
}