// UnityDtos.cs
// DTOs "allocation-free" (no arrays) designed for JSON serialization.
// Keep this file pure data (no methods) to avoid coupling and keep it stable.

using System;

namespace SaveSystem.SSJson.Utils
{
    // ---------- VECTORS (float) ----------
    [Serializable]
    public struct Vector2Dto
    {
        public float x, y;
    }

    [Serializable]
    public struct Vector3Dto
    {
        public float x, y, z;
    }

    [Serializable]
    public struct Vector4Dto
    {
        public float x, y, z, w;
    }

    // ---------- VECTORS (int) ----------
    [Serializable]
    public struct Vector2IntDto
    {
        public int x, y;
    }

    [Serializable]
    public struct Vector3IntDto
    {
        public int x, y, z;
    }

    // ---------- QUATERNION ----------
    [Serializable]
    public struct QuaternionDto
    {
        // Keep it explicit (same layout as UnityEngine.Quaternion)
        public float x, y, z, w;
    }

    // ---------- TRANSFORM (state only, not reference) ----------
    [Serializable]
    public struct TransformDto
    {
        public Vector3Dto position;   // world position
        public QuaternionDto rotation; // world rotation
        public Vector3Dto scale;      // local scale
    }

    // ---------- MATRIX ----------
    [Serializable]
    public struct Matrix4x4Dto
    {
        // Row-major layout (matches Unity's m00..m33 fields naming)
        public float m00, m01, m02, m03;
        public float m10, m11, m12, m13;
        public float m20, m21, m22, m23;
        public float m30, m31, m32, m33;
    }

    // ---------- COLORS ----------
    [Serializable]
    public struct ColorDto
    {
        public float r, g, b, a; // 0..1
    }

    [Serializable]
    public struct Color32Dto
    {
        public byte r, g, b, a; // 0..255
    }

    // ---------- RECT ----------
    [Serializable]
    public struct RectDto
    {
        // Unity Rect = x,y,width,height
        public float x, y, width, height;
    }

    [Serializable]
    public struct RectIntDto
    {
        public int x, y, width, height;
    }

    // ---------- BOUNDS ----------
    [Serializable]
    public struct BoundsDto
    {
        public Vector3Dto center;
        public Vector3Dto size;
    }

    [Serializable]
    public struct BoundsIntDto
    {
        // Unity BoundsInt = position (Vector3Int) + size (Vector3Int)
        public Vector3IntDto position;
        public Vector3IntDto size;
    }

    // ---------- PLANE ----------
    [Serializable]
    public struct PlaneDto
    {
        public Vector3Dto normal;
        public float distance;
    }

    // ---------- RAYS ----------
    [Serializable]
    public struct RayDto
    {
        public Vector3Dto origin;
        public Vector3Dto direction;
    }

    [Serializable]
    public struct Ray2DDto
    {
        public Vector2Dto origin;
        public Vector2Dto direction;
    }

    // --------- Gradient ---------
    [Serializable]
    public struct GradientColorKeyDto
    {
        public ColorDto color;  // r,g,b,a (même si Unity ignore parfois a côté color keys)
        public float time;      // 0..1
    }

    [Serializable]
    public struct GradientAlphaKeyDto
    {
        public float alpha;     // 0..1
        public float time;      // 0..1
    }

    /// <summary>
    /// DTO stable for UnityEngine.Gradient
    /// </summary>
    [Serializable]
    public struct GradientDto
    {
        // Unity has: GradientMode (Blend/Fixed) and in newer versions also color space settings.
        // Keep an int for forward compatibility.
        public int mode;

        public GradientColorKeyDto[] colorKeys;
        public GradientAlphaKeyDto[] alphaKeys;
    }

    // --------- AnimationCurve ---------

    [Serializable]
    public struct KeyframeDto
    {
        public float time;
        public float value;

        public float inTangent;
        public float outTangent;

        // Weighted tangents (Unity supports this on Keyframe)
        public float inWeight;
        public float outWeight;

        // Store as int for forward compatibility
        public int weightedMode;

        // Tangent mode flags (UnityEditor-only in some APIs, but we can store it as int)
        public int tangentMode;
    }

    [Serializable]
    public struct AnimationCurveDto
    {
        // Wrap modes control behavior outside key range
        public int preWrapMode;
        public int postWrapMode;

        public KeyframeDto[] keys;
    }
}
