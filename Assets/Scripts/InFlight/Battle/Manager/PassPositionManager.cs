using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PassPositionManager
{
    private Dictionary<string, Vector3> passPosition = new Dictionary<string, Vector3>();

    public void Init()
    {
        passPosition["Passenger (1)"] = new Vector3(-9.43f, 3.04f, 0);
        passPosition["Passenger (2)"] = new Vector3(-7.65f, -6.66f, 0);
        passPosition["Passenger (3)"] = new Vector3(16.67f, -15.6f, 0);
        passPosition["Passenger (4)"] = new Vector3(4.33f, 6.42f, 0);
        passPosition["Passenger (5)"] = new Vector3(-10.19f, -17.34f, 0);
        passPosition["Passenger (6)"] = new Vector3(-21.76f, -12.5f, 0);
        passPosition["Passenger (7)"] = new Vector3(8.78f, 12.63f, 0);
        passPosition["Passenger (8)"] = new Vector3(-10.13f, 0.89f, 0);
        passPosition["Passenger (9)"] = new Vector3(-4.37f, 15.18f, 0);
        passPosition["Passenger (10)"] = new Vector3(-19.93f, -15.05f, 0);
        passPosition["Passenger (11)"] = new Vector3(-9.36f, -15.98f, 0);
        passPosition["Passenger (12)"] = new Vector3(-9.17f, -3.66f, 0);
        passPosition["Passenger (13)"] = new Vector3(10.51f, -17.35f, 0);
        passPosition["Passenger (14)"] = new Vector3(3.6f, 14.91f, 0);
        passPosition["Passenger (15)"] = new Vector3(-14.73f, 0.08f, 0);
        passPosition["Passenger (16)"] = new Vector3(-13.81f, -1.16f, 0);
        passPosition["Passenger (17)"] = new Vector3(15.01f, -17.25f, 0);
        passPosition["Passenger (18)"] = new Vector3(29.91f, -16.7f, 0);
        passPosition["Passenger (19)"] = new Vector3(28.06f, -14.45f, 0);
        passPosition["Passenger (20)"] = new Vector3(7.36f, -11.65f, 0);
        passPosition["Passenger (21)"] = new Vector3(2.21f, -11.4f, 0);
        passPosition["Passenger (22)"] = new Vector3(0.66f, -12.5f, 0);
        passPosition["Passenger (23)"] = new Vector3(8.57f, -6.41f, 0);
        passPosition["Passenger (24)"] = new Vector3(-9.69f, -7.15f, 0);
        passPosition["Passenger (25)"] = new Vector3(4.59f, -2.7f, 0);
        passPosition["Passenger (26)"] = new Vector3(-4.46f, -7.65f, 0);
        passPosition["Passenger (27)"] = new Vector3(-1.63f, 14.09f, 0);
        passPosition["Passenger (28)"] = new Vector3(-21.68f, -5.84f, 0);
        passPosition["Passenger (29)"] = new Vector3(-13.84f, -9.14f, 0);
        passPosition["Passenger (30)"] = new Vector3(5.92f, 9.45f, 0);
        passPosition["Passenger (31)"] = new Vector3(8.99f, 16.25f, 0);
        passPosition["Passenger (32)"] = new Vector3(8.99f, 3.97f, 0);
        passPosition["Passenger (33)"] = new Vector3(9.99f, -7.28f, 0);
        passPosition["Passenger (34)"] = new Vector3(-14.07f, -14.94f, 0);
        passPosition["Passenger (35)"] = new Vector3(-5.15f, 9.33f, 0);
        passPosition["Passenger (36)"] = new Vector3(-21.65f, -14.3f, 0);
        passPosition["Passenger (37)"] = new Vector3(-16.76f, 0.04f, 0);
        passPosition["Passenger (38)"] = new Vector3(9.92f, 0.76f, 0);
        passPosition["Passenger (39)"] = new Vector3(1.54f, 14.13f, 0);
        passPosition["Passenger (40)"] = new Vector3(-2.78f, 15.99f, 0);
        passPosition["Passenger (41)"] = new Vector3(10.16f, -14.5f, 0);
        passPosition["Passenger (42)"] = new Vector3(10.55f, -6.92f, 0);
        passPosition["Passenger (43)"] = new Vector3(-16.25f, -16.18f, 0);
        passPosition["Passenger (44)"] = new Vector3(-14.3f, -3.82f, 0);
        passPosition["Passenger (45)"] = new Vector3(10.62f, 15.47f, 0);
        passPosition["Passenger (46)"] = new Vector3(9.5f, 3.05f, 0);
        passPosition["Passenger (47)"] = new Vector3(-8.33f, -17.01f, 0);
        passPosition["Passenger (48)"] = new Vector3(-13.86f, -14.3f, 0);
        passPosition["Passenger (49)"] = new Vector3(-7.84f, 15.75f, 0);
        passPosition["Passenger (50)"] = new Vector3(-20.12f, 1.01f, 0);
        passPosition["Passenger (51)"] = new Vector3(9.73f, -16.19f, 0);
        passPosition["Passenger (52)"] = new Vector3(-21.81f, -0.82f, 0);
        passPosition["Passenger (53)"] = new Vector3(2.84f, 12.42f, 0);
        passPosition["Passenger (54)"] = new Vector3(-8.07f, 12.96f, 0);
        passPosition["Passenger (55)"] = new Vector3(-18.01f, -15.08f, 0);
        passPosition["Passenger (56)"] = new Vector3(2.06f, -5.02f, 0);
        passPosition["Passenger (57)"] = new Vector3(8.25f, -15.27f, 0);
        passPosition["Passenger (58)"] = new Vector3(-16.12f, -1.44f, 0);
        passPosition["Passenger (59)"] = new Vector3(29.98f, -14.3f, 0);
        passPosition["Passenger (60)"] = new Vector3(7.9f, 15.66f, 0);
        passPosition["Passenger (61)"] = new Vector3(-15.29f, -17.24f, 0);
        passPosition["Passenger (62)"] = new Vector3(7.95f, -6.07f, 0);
        passPosition["Passenger (63)"] = new Vector3(4.2f, 9.11f, 0);
        passPosition["Passenger (64)"] = new Vector3(16.99f, -15.55f, 0);
        passPosition["Passenger (65)"] = new Vector3(3.6f, 14.91f, 0);
        passPosition["Passenger (66)"] = new Vector3(-6.76f, -16.44f, 0);
        passPosition["Passenger (67)"] = new Vector3(-5.67f, -14.7f, 0);
        passPosition["Passenger (68)"] = new Vector3(9.38f, 13.09f, 0);
        passPosition["Passenger (69)"] = new Vector3(-9.09f, 0.89f, 0);
        passPosition["Passenger (70)"] = new Vector3(-9.38f, 13.02f, 0);
        passPosition["Passenger (71)"] = new Vector3(7.77f, -10.95f, 0);
        passPosition["Passenger (72)"] = new Vector3(-4.39f, -8.75f, 0);
        passPosition["Passenger (73)"] = new Vector3(7.32f, -16.24f, 0);
        passPosition["Passenger (74)"] = new Vector3(8.69f, -14.9f, 0);
        passPosition["Passenger (75)"] = new Vector3(-7.62f, 16.18f, 0);
        passPosition["Passenger (76)"] = new Vector3(4.48f, -7.92f, 0);
        passPosition["Passenger (77)"] = new Vector3(-13.82f, -10.29f, 0);
        passPosition["Passenger (78)"] = new Vector3(9.4f, -17.1f, 0);
        passPosition["Passenger (79)"] = new Vector3(9.6f, -11.17f, 0);
        passPosition["Passenger (80)"] = new Vector3(2.49f, -10.77f, 0);
        passPosition["Passenger (81)"] = new Vector3(-5.4f, -10.69f, 0);
        passPosition["Passenger (82)"] = new Vector3(-6.96f, -11.13f, 0);
        passPosition["Passenger (83)"] = new Vector3(9.41f, -4.9f, 0);
        passPosition["Passenger (84)"] = new Vector3(2.04f, -5.85f, 0);
        passPosition["Passenger (85)"] = new Vector3(-7.97f, -14.01f, 0);
        passPosition["Passenger (86)"] = new Vector3(-2.9f, 9.29f, 0);
        passPosition["Passenger (87)"] = new Vector3(-15.8f, -11.8f, 0);
        passPosition["Passenger (88)"] = new Vector3(-8.94f, -16.1f, 0);
        passPosition["Passenger (89)"] = new Vector3(4.54f, -9.18f, 0);
        passPosition["Passenger (90)"] = new Vector3(7.67f, 16.51f, 0);
        passPosition["Passenger (91)"] = new Vector3(8.99f, 3.97f, 0);
        passPosition["Passenger (92)"] = new Vector3(13.58f, -14.26f, 0);
        passPosition["Passenger (93)"] = new Vector3(-4.6f, 9.27f, 0);
        passPosition["Passenger (94)"] = new Vector3(-21.65f, -14.3f, 0);
        passPosition["Passenger (95)"] = new Vector3(-8.64f, -4.06f, 0);
        passPosition["Passenger (96)"] = new Vector3(9.92f, 0.76f, 0);
        passPosition["Passenger (97)"] = new Vector3(1.97f, 14.27f, 0);
    }

    public Vector3 GetPassPosition(string objectName)
    {
        if (passPosition.TryGetValue(objectName, out Vector3 position))
        {
            return position;
        }

        return new Vector3();
    }
}
