namespace Game.Level.Data
{
    public interface ICollectableEffectAction
    {
        CollectableType CollectableType { get; }
        
        void Activate();

        void Deactivate();

        void UpdateTimer(float deltaTime);
    }
}