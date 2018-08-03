using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Misc
{
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

    public static class Prefabs
    {
        //back
        public static string Grass = "Prefabs/GrassPrefab";
        public static string Water = "Prefabs/WaterPrefab";
        public static string Forest = "Prefabs/ForestPrefab";
        public static string Swamp = "Prefabs/SwampPrefab";

        //fore
        public static string Obstacle = "Prefabs/ObstaclePrefab";
        public static string Diamond = "Prefabs/DiamondPrefab";
        public static string Enemy = "Prefabs/EnemyPrefab";
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

    public static class RandomEnum
    {
        /// <summary>
        /// Gives you random enum value from T
        /// </summary>
        /// <returns></returns>
        public static T Value<T>() where T : struct, IConvertible
        {
            Array values = Enum.GetValues(typeof(T));
            return (T) values.GetValue((int) Mathf.Round(Random.value * (values.Length - 1)));
        }
    }
}