using System;


namespace Client.Scripts.Miscellaneous
{
    public static class Prefabs
    {
        public const string Hex = "Prefabs/HexPrefab";
        public const string Spirit = "Prefabs/SpiritPrefab";
        public const string Player = "Prefabs/PlayerPrefab";
        public const string Property = "Prefabs/PropertyPrefab";
        public const string Empty = "Prefabs/EmptyPrefab";
    }
    
    public static class Names
    {
        public static string Diamonds = "Diamonds";
        public static string Killed = "Killed";
        public static string HpBar = "HpBar";
        
        public static string PlayerPosVector = "PlayerPosVector";
        public static string PlayerPosChunk = "PlayerPosChunk";
        public static string PlayerPosOffset = "PlayerPosOffset";
        public static string PlayerPosHexel = "PlayerPosHexel";
        public static string LoadedChunks = "LoadedChunks";
    }

    public enum HexTypes
    {
        Grass,
        Water,
        Forest,
        Swamp,
        Obstacle,
        Diamond,
        Enemy,
        Empty,
        Spawn,
    }


    public enum HexProperties
    {
        Hp,
        MaxHp,
        Iq,
        Speed,
        AgroSpeed,
        JumpSpeed,
        AgroRadius,
        Flying,
    }
    
    /*public enum GameStates
    {
        Start,
        Pause,
        Restart,
        Exit
    }*/

}