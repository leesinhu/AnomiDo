using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PatrolManager
{
    private Dictionary<string, Dictionary<string, List<Vector3>>> scenePatrols = new Dictionary<string, Dictionary<string, List<Vector3>>>();

    // E    NE  N   NW  W   SW  S   SE  실행x
    // 0    1   2   3   4   5   6   7   -1

    // new Vector3(12.0f,  20.0f,  3)
    // 목적지 (12.0f, 20.0f)
    // 도착 후 애니메이션 3 = NW


    public void Init()
    {
        scenePatrols["Stage4"] = new Dictionary<string, List<Vector3>>();
        scenePatrols["Stage4"]["Enemy (1)"] = new List<Vector3>()
        {
            new Vector3(-6.2f,  -13.37f,  0),
        };
        scenePatrols["Stage4"]["Enemy (2)"] = new List<Vector3>()
        {
            new Vector3(-0.97f,  -16.16f,   0)
        };
        scenePatrols["Stage4"]["Enemy (3)"] = new List<Vector3>()
        {
            new Vector3(4.82f,  -15.98f,  0),
        };
        scenePatrols["Stage4"]["Enemy (4)"] = new List<Vector3>()
        {
            new Vector3(-20.48f,  -4.35f,  0),
        };
        scenePatrols["Stage4"]["Enemy (5)"] = new List<Vector3>()
        {
            new Vector3(-19.29f,  -7.31f,  0),
        };
        scenePatrols["Stage4"]["Enemy (6)"] = new List<Vector3>()
        {
            new Vector3(-13.99f,  -3.91f,  0),
        };
        scenePatrols["Stage4"]["Enemy (7)"] = new List<Vector3>()
        {
            new Vector3(16.82f,  -5.78f,  0),
        };
        scenePatrols["Stage4"]["Enemy (8)"] = new List<Vector3>()
        {
            new Vector3(15.32f,  -2.8f,  0),
        };
        scenePatrols["Stage4"]["Enemy (9)"] = new List<Vector3>()
        {
            new Vector3(19.3f,  -5.95f,  0),
        };
        scenePatrols["Stage4"]["Enemy (10)"] = new List<Vector3>()
        {
            new Vector3(13.86f,  -0.37f,  0),
        };
        scenePatrols["Stage4"]["Enemy (11)"] = new List<Vector3>()
        {
            new Vector3(17.35f,  -3.75f,  0),
        };
        scenePatrols["Stage4"]["Enemy (12)"] = new List<Vector3>()
        {
            new Vector3(15.89f,  -0.27f,  0),
        };

        scenePatrols["Stage6"] = new Dictionary<string, List<Vector3>>();
        scenePatrols["Stage6"]["Enemy (1)"] = new List<Vector3>()
        {
            new Vector3(17.31f,  -12.56f,  0),
        };
        scenePatrols["Stage6"]["Enemy (2)"] = new List<Vector3>()
        {
            new Vector3(16.01f,  -14.41f,   0)
        };
        scenePatrols["Stage6"]["Enemy (3)"] = new List<Vector3>()
        {
            new Vector3(19.22f,  -13.5f,  0),
        };
        scenePatrols["Stage6"]["Enemy (4)"] = new List<Vector3>()
        {
            new Vector3(23.31f,  -12.4f,  0),
        };
        scenePatrols["Stage6"]["Enemy (5)"] = new List<Vector3>()
        {
            new Vector3(21.12f,  -13.39f,  0),
        };
        scenePatrols["Stage6"]["Enemy (6)"] = new List<Vector3>()
        {
            new Vector3(9.46f,  -7.02f,  0),
        };
        scenePatrols["Stage6"]["Enemy (7)"] = new List<Vector3>()
        {
            new Vector3(8.34f,  -5.68f,  0),
        };
        scenePatrols["Stage6"]["Enemy (8)"] = new List<Vector3>()
        {
            new Vector3(-7.93f,  11.62f,  0),
        };
        scenePatrols["Stage6"]["Enemy (9)"] = new List<Vector3>()
        {
            new Vector3(-7.89f,  13.9f,  0),
        };
        scenePatrols["Stage6"]["Enemy (10)"] = new List<Vector3>()
        {
            new Vector3(-9.17f,  14.88f,  0),
        };
        scenePatrols["Stage6"]["Enemy (11)"] = new List<Vector3>()
        {
            new Vector3(8.44f,  14.33f,  0),
        };
        scenePatrols["Stage6"]["Enemy (12)"] = new List<Vector3>()
        {
            new Vector3(9.51f,  15.25f,  0),
        };
        scenePatrols["Stage6"]["Enemy (13)"] = new List<Vector3>()
        {
            new Vector3(7.95f,  12.66f,  0),
        };
        scenePatrols["Stage6"]["Enemy (14)"] = new List<Vector3>()
        {
            new Vector3(6.34f,  -5.31f,  0),
        };
        scenePatrols["Stage6"]["Enemy (15)"] = new List<Vector3>()
        {
            new Vector3(-29.82f,  4.92f,  0),
        };
        scenePatrols["Stage6"]["Enemy (16)"] = new List<Vector3>()
        {
            new Vector3(-28.77f,  5.52f,  0),
        };
        scenePatrols["Stage6"]["Enemy (17)"] = new List<Vector3>()
        {
            new Vector3(-27.62f, 6.26f,  0),
        };
        scenePatrols["Stage6"]["Enemy (18)"] = new List<Vector3>()
        {
            new Vector3(-26.45f,  7.23f,  0),
        };
        scenePatrols["Stage6"]["Enemy (19)"] = new List<Vector3>()
        {
            new Vector3(-27.08f,  8.59f,  0),
        };
        scenePatrols["Stage6"]["Enemy (20)"] = new List<Vector3>()
        {
            new Vector3(-26.27f,  10.24f,  0),
        };

        scenePatrols["Stage8"] = new Dictionary<string, List<Vector3>>();
        scenePatrols["Stage8"]["Enemy (1)"] = new List<Vector3>()
        {
            new Vector3(-14.29f,  -1.38f,  0),
        };
        scenePatrols["Stage8"]["Enemy (2)"] = new List<Vector3>()
        {
            new Vector3(-14.24f,  -3.02f,   0)
        };
        scenePatrols["Stage8"]["Enemy (3)"] = new List<Vector3>()
        {
            new Vector3(-14.08f,  -4.66f,  0),
        };
        scenePatrols["Stage8"]["Enemy (4)"] = new List<Vector3>()
        {
            new Vector3(-14.09f,  -6.11f,  0),
        };
        scenePatrols["Stage8"]["Enemy (5)"] = new List<Vector3>()
        {
            new Vector3(-14.57f,  -7.29f,  0),
        };
        scenePatrols["Stage8"]["Enemy (6)"] = new List<Vector3>()
        {
            new Vector3(-15.46f,  -8.51f,  0),
        };
        scenePatrols["Stage8"]["Enemy (7)"] = new List<Vector3>()
        {
            new Vector3(-14.88f,  -10.39f,  0),
        };
        scenePatrols["Stage8"]["Enemy (8)"] = new List<Vector3>()
        {
            new Vector3(-2.27f,  14.3f,  0),
        };
        scenePatrols["Stage8"]["Enemy (9)"] = new List<Vector3>()
        {
            new Vector3(-8.09f,  -5.28f,  0),
        };
        scenePatrols["Stage8"]["Enemy (10)"] = new List<Vector3>()
        {
            new Vector3(3.21f,  15.25f,  0),
        };
        scenePatrols["Stage8"]["Enemy (11)"] = new List<Vector3>()
        {
            new Vector3(2.63f,  13.68f,  0),
        };
        scenePatrols["Stage8"]["Enemy (12)"] = new List<Vector3>()
        {
            new Vector3(7.9f,  2.74f,  0),
        };
        scenePatrols["Stage8"]["Enemy (13)"] = new List<Vector3>()
        {
            new Vector3(9.06f,  1.69f,  0),
        };
        scenePatrols["Stage8"]["Enemy (14)"] = new List<Vector3>()
        {
            new Vector3(-6.58f,  2.65f,  0),
        };
        scenePatrols["Stage8"]["Enemy (15)"] = new List<Vector3>()
        {
            new Vector3(-8.03f,  2.35f,  0),
        };
        scenePatrols["Stage8"]["Enemy (16)"] = new List<Vector3>()
        {
            new Vector3(-0.54f,  8.85f,  0),
        };
        scenePatrols["Stage8"]["Enemy (17)"] = new List<Vector3>()
        {
            new Vector3(-7.02f,  -5.44f,  0),
        };
        scenePatrols["Stage8"]["Enemy (18)"] = new List<Vector3>()
        {
            new Vector3(-15.59f,  -11.79f,  0),
        };
        scenePatrols["Stage8"]["Enemy (19)"] = new List<Vector3>()
        {
            new Vector3(-4.03f,  8.46f,  0),
        };
    }

    public List<Vector3> GetPatrolsForScene(string sceneName, string objectName)
    {
        if (scenePatrols.TryGetValue(sceneName, out Dictionary<string, List<Vector3>> patrols))
        {
            if (patrols.TryGetValue(objectName, out List<Vector3> points))
            {
                return points;
            }
        }
        return new List<Vector3>(); // Scene이 없으면 빈 리스트 반환
    }
}
