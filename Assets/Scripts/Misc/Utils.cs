using System;
using LeopotamGroup.Math;
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

    public static class Names
    {
        public static string Diamonds = "Diamonds";
        public static string Killed = "Killed";
        public static string HpBar = "HpBar";
        
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

        public static int Count = 7;
    }
    
    public enum HexProperties
    {
        HP,
        MaxHP,
        IQ,
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

    static class RotateExtension
    {
        public static void LookAt2D(this Transform me, Vector2 target)
        {
            Vector2 dir = target - (Vector2)me.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            me.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        public static void LookAt2D(this Transform me, Transform target)
        {
            me.LookAt2D(target.position);
        }

        public static void LookAt2D(this Transform me, GameObject target)
        {
            me.LookAt2D(target.transform.position);
        }
    }
}