namespace Game.Data
{
    public static class GameTags
    {
        public const string PlayerTag = "Player";
        public const string AbyssTag = "Abyss";
        public const string GroundTag = "Ground";
        public const string ObstacleTag = "Obstacle";
        
        public static readonly string[] PlayerKillTags = new string[] { ObstacleTag, AbyssTag };
    }
}