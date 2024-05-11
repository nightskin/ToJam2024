
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point 
{
    public Color color;
    public bool active;
    public float value;
    public Vector3 position;
    public int x;
    public int y;
    public int z;

    public Point()
    {
        color = new Color();
        active = false;
        position = new Vector3();
    }
    public Point(Vector3 pos)
    {
        position = pos;
    }

    public static int GetState(Point[] points)
    {
        int state = 0;
        if (points[0].active) state |= 1;
        if (points[1].active) state |= 2;
        if (points[2].active) state |= 4;
        if (points[3].active) state |= 8;
        if (points[4].active) state |= 16;
        if (points[5].active) state |= 32;
        if (points[6].active) state |= 64;
        if (points[7].active) state |= 128;
        return state;
    }

    public static int GetState(Point[] points, float isoLevel)
    {
        int state = 0;
        if (points[0].value >= isoLevel) state |= 1;
        if (points[1].value >= isoLevel) state |= 2;
        if (points[2].value >= isoLevel) state |= 4;
        if (points[3].value >= isoLevel) state |= 8;
        if (points[4].value >= isoLevel) state |= 16;
        if (points[5].value >= isoLevel) state |= 32;
        if (points[6].value >= isoLevel) state |= 64;
        if (points[7].value >= isoLevel) state |= 128;
        return state;
    }

    public static Vector3 LerpPoint(Point point1 , Point point2, float isoLevel)
    {
        return Vector3.Lerp(point1.position, point2.position, (isoLevel - point1.value) / (point2.value - point1.value));  
    }

    public static Vector3 GetMidPoint(Point point1, Point point2)
    {
        return (point1.position + point2.position) / 2;
    }
}