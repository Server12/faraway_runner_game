using System;
using Game.Game.Level.Views;

namespace Game.Level.Controller
{
    public interface ILevelController
    {
        event Action<PlatformModel> OnPlatformCreated;
        
        event Action<PlatformModel> OnPlatformReleasedToPool; 
    }
}