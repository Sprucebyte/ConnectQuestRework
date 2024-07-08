using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class Util
{
    public static void DrawArrow(Vector3 from, Vector3 to, float lineCuttof = .1f, float thickness = 3f, int arrowSize = 8)
    {
        Vector3 direction = from - to;
        float distance = direction.magnitude;

        Vector3 fromPos = Vector3.Lerp(to, from, lineCuttof);
        Vector3 toPos = Vector3.Lerp(to, from, 1 - lineCuttof);

        Handles.DrawLine(fromPos, toPos, thickness);
        Vector3 arrowPos = Vector3.Lerp(to, from, .5f);

        Handles.ConeHandleCap(0, arrowPos, Quaternion.LookRotation(direction), arrowSize, EventType.Repaint);
    }


    // Convert from 2D coordinate to 1D array position
    public static int To1D(int x, int y, int width)
    {
        return y * width + x;
    }

    // Convert from 1D array position to 2D coordinate
    public static (int x, int y) To2D(int position, int width)
    {
        int y = position / width;
        int x = position % width;
        return (x, y);
    }

    public static bool IsValueBetween(float value, float min, float max)
    {
        return value >= min && value <= max;
    }


    public static void DrawCross(Vector3 pos, float size)
    {
        Gizmos.DrawLine(new Vector3(pos.x - size, pos.y, pos.z), new Vector3(pos.x + size, pos.y, pos.z));
        Gizmos.DrawLine(new Vector3(pos.x, pos.y - size, pos.z), new Vector3(pos.x, pos.y + size, pos.z));
    }

    public static void DrawCross(Vector2 pos, float size)
    {
        Gizmos.DrawLine(new Vector3(pos.x - size, pos.y, 0), new Vector3(pos.x + size, pos.y, 0));
        Gizmos.DrawLine(new Vector3(pos.x, pos.y - size, 0), new Vector3(pos.x, pos.y + size, 0));
    }

    [System.Serializable]
    public class Damage
    {
        public Damage()
        {
            blunt     = 0;
            piercing  = 0;
            slashing  = 0;
            fire      = 0;
            ice       = 0;
            water     = 0;
            explosive = 0;
            poison    = 0;
            magic     = 0;
            psychic   = 0;
            electric  = 0;
            misc      = 0;
        }

        public Damage(float defaultValue)
        {
            blunt     = defaultValue;
            piercing  = defaultValue;
            slashing  = defaultValue;
            fire      = defaultValue;
            ice       = defaultValue;
            water     = defaultValue;
            explosive = defaultValue;
            poison    = defaultValue;
            magic     = defaultValue;
            psychic   = defaultValue;
            electric  = defaultValue;
            misc      = defaultValue;
        }

        public float blunt;
        public float piercing;
        public float slashing;
        public float fire;
        public float ice;
        public float water;
        public float explosive;
        public float poison;
        public float magic;
        public float psychic;
        public float electric;
        public float misc;
    }
}