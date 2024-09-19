using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class UnityUtils
{
    // --- Maths --- \\
    public static Vector3 GetVector3Between(float min = -1f, float max = 1f)
    {
        float x = GetFloatBetween(min, max);
        float y = GetFloatBetween(min, max);
        float z = GetFloatBetween(min, max);

        return new Vector3(x, y, z);
    }

    public static Vector2 GetVector2Between(float min = -1f, float max = 1f)
    {
        float x = GetFloatBetween(min, max);
        float y = GetFloatBetween(min, max);

        return new Vector2(x, y);
    }

    public static Vector3 GetRandomUnitVector()
    {
        return GetVector3Between(-1, 1).normalized;
    }

    public static int GetIntBetween(int min = -1, int max = 1)
    {
        return Random.Range(min, max);
    }

    public static float GetFloatBetween(float min = -1f, float max = 1f)
    {
        return Random.Range(min * 100f, max * 100f) / 100;
    }

    public static bool DirectionsApproximatlyEqual(Vector3 dir1, Vector3 dir2, float epsilon = 0.8f)
    {
        float d = Vector3.Dot(dir1.normalized, dir2.normalized);
        return d > epsilon;
    }

    public static float AngleBetweenTwoPositions(Vector2 p0, Vector2 p1)
    {
        Vector2 dir = (p1 - p0).normalized;
        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;

        return angle;
    }

    public static Vector2 RemoveY(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.z);
    }

    public static Vector3 SetXToZero(this Vector3 vector)
    {
        return new Vector3(0, vector.y, vector.z);
    }

    public static Vector3 SetYToZero(this Vector3 vector)
    {
        return new Vector3(vector.x, 0, vector.z);
    }

    public static Vector3 SetZToZero(this Vector3 vector)
    {
        return new Vector3(vector.x, vector.y, 0);
    }

    public static Vector3 GetLinearVelocityAtPoint(Rigidbody aRigidbody, Vector3 aLocalPoint)
    {
        var p = aLocalPoint - aRigidbody.centerOfMass;
        var v = Vector3.Cross(aRigidbody.angularVelocity, p);
        v = aRigidbody.transform.TransformDirection(v);
        v += aRigidbody.velocity;
        return v;
    }

    public static float Length(this Vector3 vector)
    {
        float x = Mathf.Pow(vector.x, 2f);
        float y = Mathf.Pow(vector.y, 2f);
        float z = Mathf.Pow(vector.z, 2f);
        return Mathf.Sqrt(x + y + z);
    }

    public static float Length(this Vector2 vector)
    {
        float x = Mathf.Pow(vector.x, 2f);
        float y = Mathf.Pow(vector.y, 2f);
        return Mathf.Sqrt(x + y);
    }

    public static Vector2 GetPointOnCircle(Vector2 circleCentre, float radius, float angle)
    {
        float xPos = radius * Mathf.Cos(angle) + circleCentre.x;
        float yPos = radius * Mathf.Sin(angle) + circleCentre.y;

        return new Vector2(xPos, yPos);
    }

    public static Vector2 FindClosestPointOnCircle(Vector2 startPos, Vector2 circleCentre, float radius)
    {
        Vector2 v = startPos - circleCentre;
        return circleCentre + v / v.Length() * radius;
    }

    public static Vector3 FindNearestPointOnLine(Vector3 origin, Vector3 end, Vector3 point)
    {
        // Get heading
        Vector3 heading = (end - origin);
        float magnitudeMax = heading.magnitude;
        heading.Normalize();

        // Do projection from the point
        Vector3 lhs = point - origin;
        float dotP = Vector3.Dot(lhs, heading);

        // Clamp the result
        dotP = Mathf.Clamp(dotP, 0f, magnitudeMax);
        return origin + heading * dotP;
    }

    public static Vector3 FindNearestPointOnInfiniteLine(Vector3 linePoint, Vector3 lineDir, Vector3 point)
    {
        lineDir.Normalize();
        var v = point - linePoint;
        var d = Vector3.Dot(v, lineDir);

        return linePoint + lineDir * d;
    }

    public static float NormalizedDistance(float min, float max, float t)
    {
        return Mathf.Clamp01((t - min) / (max - min));
    }

    public static float MagnitudeInDirection(Vector3 vector, Vector3 direction, bool normalizeParameters = true)
    {
        if (normalizeParameters)
        {
            direction.Normalize();
        }

        return Vector3.Dot(vector, direction);
    }

    public static float ClampMax(float value, float max)
    {
        if (value > max)
        {
            return max;
        }

        return value;
    }

    public static float ClampMin(float value, float min)
    {
        if (value < min)
        {
            return min;
        }

        return value;
    }

    public static float RandomSign(float num)
    {
        if (Random.Range(0, 2) == 0)
        {
            num *= -1f;
        }

        return num;
    }

    public static float Round(float value, int digits = 3)
    {
        float mult = Mathf.Pow(10.0f, (float)digits);
        return Mathf.Round(value * mult) / mult;
    }

    // --- Easing Functions --- \\
    public static float EaseInCubic(float t)
    {
        return t * t * t;
    }

    public static float EaseOutCubic(float t)
    {
        return 1f - Mathf.Pow(1 - t, 3);
    }

    public static float EaseInQuad(float t)
    {
        return t * t;
    }

    public static float EaseOutQuad(float t)
    {
        return 1f - (1f - t) * (1f - t);
    }

    // --- Formatting --- \\
    public static string FormatTimeSeconds(float time)
    {
        int minutes = (int)time / 60;
        int seconds = (int)time % 60;

        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public static string FormatTimeMiliseconds(float time)
    {
        int minutes = (int)time / 60;
        int seconds = (int)time % 60;
        int miliseconds = (int)(time * 1000) % 1000;

        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, miliseconds);
    }

    // --- Misc --- \\
    public static RaycastHit[] OrderRaycastHitsByDistance(RaycastHit[] hits, Vector3 startPos)
    {
        return hits.OrderBy(x => Vector2.Distance(startPos, x.point)).ToArray();
    }

    public static AudioClip GetRandomAudioClip(AudioClip[] clips)
    {
        int index = Random.Range(0, clips.Length);
        return clips[index];
    }

    public static List<T> CopyList<T>(List<T> list)
    {
        List<T> listCopy = new List<T>();
        foreach (T listElement in list)
        {
            listCopy.Add(listElement);
        }

        return listCopy;
    }

    public static List<T> ConcatonateLists<T>(List<T> list1, List<T> list2)
    {
        List<T> listCopy = new List<T>();
        foreach (T listElement in list1)
        {
            listCopy.Add(listElement);
        }

        foreach (T listElement in list2)
        {
            listCopy.Add(listElement);
        }

        return listCopy;
    }

    public static bool Contains(this LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    public static IEnumerable<T> FindInterfacesOfType<T>()
    {
        return MonoBehaviour.FindObjectsOfType<MonoBehaviour>().OfType<T>();
    }
}
