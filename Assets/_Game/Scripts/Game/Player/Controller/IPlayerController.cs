using System;
using Game.Player.Data;
using UnityEngine;

namespace Game.Controllers
{
    public interface IPlayerController
    {
        event Action OnDied;
        
        PlayerConfig Config { get; }

        Bounds Bounds { get; }

        Transform Transform { get; }

        PlayerState CurrentState { get; }

        void SetFlyMode(bool enable);

        float MoveSpeed { get; set; }
    }
}