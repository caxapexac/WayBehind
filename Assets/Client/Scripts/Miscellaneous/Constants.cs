using System;


namespace Client.Scripts.Miscellaneous
{
    [Obsolete]
    public static class Tags
    {
        public static string CameraTag = "MainCamera";
        public static string PlayerTag = "Player";
        public static string EnemyTag = "Enemy";
        public static string BackgroundTag = "Background";
        public static string ForegroundTag = "Foreground";
        public static string CanvasTag = "Canvas";
        public static string JoystickTag = "Joystick";
    }


    public static class Names
    {
        public static string Diamonds = "Diamonds";
        public static string Killed = "Killed";
        public static string HpBar = "HpBar";
    }


    public static class Prefabs
    {
        public static string Hex = "Prefabs/HexPrefab";
        public static string Player = "Prefabs/PlayerPrefab";
        public static string Property = "Prefabs/PropertyPrefab";
        public static string Spirit = "Prefabs/SpiritPrefab";
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


    public enum Pool
    {
        //back
        Grass = 0,
        Water = 1,
        Forest = 2,
        Swamp = 3,

        //fore
        Obstacle = 4,
        Diamond = 5,
        Enemy = 6,
    }


    public enum NoiseTypes
    {
        Perlin,
        Fractal,
        Simplex,
        Heighbours
    }
}