// UnityDtoConverters.cs
// Homogeneous naming + null-safety + consistent Transform semantics (LOCAL).
// Conventions:
// - ToDto(X) converts Unity -> DTO
// - FromDto(DTO) converts DTO -> Unity

using System;
using UnityEngine;

namespace SaveSystem.SSJson.Utils
{
    public static class UnityDtoConverters
    {
        // -------------------------
        // VECTORS (float)
        // -------------------------
        public static Vector2Dto ToDto(Vector2 v) => new() { x = v.x, y = v.y };
        public static Vector2 FromDto(Vector2Dto dto) => new(dto.x, dto.y);

        public static Vector3Dto ToDto(Vector3 v) => new() { x = v.x, y = v.y, z = v.z };
        public static Vector3 FromDto(Vector3Dto dto) => new(dto.x, dto.y, dto.z);

        public static Vector4Dto ToDto(Vector4 v) => new() { x = v.x, y = v.y, z = v.z, w = v.w };
        public static Vector4 FromDto(Vector4Dto dto) => new(dto.x, dto.y, dto.z, dto.w);

        // -------------------------
        // VECTORS (int)
        // -------------------------
        public static Vector2IntDto ToDto(Vector2Int v) => new() { x = v.x, y = v.y };
        public static Vector2Int FromDto(Vector2IntDto dto) => new(dto.x, dto.y);

        public static Vector3IntDto ToDto(Vector3Int v) => new() { x = v.x, y = v.y, z = v.z };
        public static Vector3Int FromDto(Vector3IntDto dto) => new(dto.x, dto.y, dto.z);

        // -------------------------
        // QUATERNION
        // -------------------------
        public static QuaternionDto ToDto(Quaternion q) => new() { x = q.x, y = q.y, z = q.z, w = q.w };
        public static Quaternion FromDto(QuaternionDto dto) => new(dto.x, dto.y, dto.z, dto.w);

        // -------------------------
        // TRANSFORM (STATE ONLY)
        // Default = LOCAL space for consistency.
        // -------------------------
        public static TransformDto ToDto(Transform t) => new()
        {
            position = ToDto(t.localPosition),
            rotation = ToDto(t.localRotation),
            scale = ToDto(t.localScale),
        };

        public static void FromDto(Transform t, TransformDto dto)
        {
            t.localPosition = FromDto(dto.position);
            t.localRotation = FromDto(dto.rotation);
            t.localScale = FromDto(dto.scale);
        }

        // -------------------------
        // MATRIX4x4
        // -------------------------
        public static Matrix4x4Dto ToDto(Matrix4x4 m) => new()
        {
            m00 = m.m00,
            m01 = m.m01,
            m02 = m.m02,
            m03 = m.m03,
            m10 = m.m10,
            m11 = m.m11,
            m12 = m.m12,
            m13 = m.m13,
            m20 = m.m20,
            m21 = m.m21,
            m22 = m.m22,
            m23 = m.m23,
            m30 = m.m30,
            m31 = m.m31,
            m32 = m.m32,
            m33 = m.m33
        };

        public static Matrix4x4 FromDto(Matrix4x4Dto dto)
        {
            var m = new Matrix4x4
            {
                m00 = dto.m00,
                m01 = dto.m01,
                m02 = dto.m02,
                m03 = dto.m03,
                m10 = dto.m10,
                m11 = dto.m11,
                m12 = dto.m12,
                m13 = dto.m13,
                m20 = dto.m20,
                m21 = dto.m21,
                m22 = dto.m22,
                m23 = dto.m23,
                m30 = dto.m30,
                m31 = dto.m31,
                m32 = dto.m32,
                m33 = dto.m33
            };
            return m;
        }

        // -------------------------
        // COLORS
        // -------------------------
        public static ColorDto ToDto(Color c) => new() { r = c.r, g = c.g, b = c.b, a = c.a };
        public static Color FromDto(ColorDto dto) => new(dto.r, dto.g, dto.b, dto.a);

        public static Color32Dto ToDto(Color32 c) => new() { r = c.r, g = c.g, b = c.b, a = c.a };
        public static Color32 FromDto(Color32Dto dto) => new(dto.r, dto.g, dto.b, dto.a);

        // Optional bridges (useful when older saves used ColorDto only)
        public static Color32Dto ToColor32Dto(ColorDto dto) => new()
        {
            r = (byte)(Mathf.Clamp01(dto.r) * 255f),
            g = (byte)(Mathf.Clamp01(dto.g) * 255f),
            b = (byte)(Mathf.Clamp01(dto.b) * 255f),
            a = (byte)(Mathf.Clamp01(dto.a) * 255f),
        };

        public static ColorDto ToColorDto(Color32Dto dto) => new()
        {
            r = dto.r / 255f,
            g = dto.g / 255f,
            b = dto.b / 255f,
            a = dto.a / 255f,
        };

        // -------------------------
        // RECT
        // -------------------------
        public static RectDto ToDto(Rect r) => new() { x = r.x, y = r.y, width = r.width, height = r.height };
        public static Rect FromDto(RectDto dto) => new(dto.x, dto.y, dto.width, dto.height);

        public static RectIntDto ToDto(RectInt r) => new() { x = r.x, y = r.y, width = r.width, height = r.height };
        public static RectInt FromDto(RectIntDto dto) => new(dto.x, dto.y, dto.width, dto.height);

        // -------------------------
        // BOUNDS
        // -------------------------
        public static BoundsDto ToDto(Bounds b) => new()
        {
            center = ToDto(b.center),
            size = ToDto(b.size)
        };

        public static Bounds FromDto(BoundsDto dto)
            => new(FromDto(dto.center), FromDto(dto.size));

        public static BoundsIntDto ToDto(BoundsInt b) => new()
        {
            position = ToDto(b.position),
            size = ToDto(b.size)
        };

        public static BoundsInt FromDto(BoundsIntDto dto)
            => new(FromDto(dto.position), FromDto(dto.size));

        // -------------------------
        // PLANE
        // -------------------------
        public static PlaneDto ToDto(Plane p) => new()
        {
            normal = ToDto(p.normal),
            distance = p.distance
        };

        public static Plane FromDto(PlaneDto dto)
            => new(FromDto(dto.normal), dto.distance);

        // -------------------------
        // RAYS
        // -------------------------
        public static RayDto ToDto(Ray r) => new()
        {
            origin = ToDto(r.origin),
            direction = ToDto(r.direction)
        };

        public static Ray FromDto(RayDto dto)
            => new(FromDto(dto.origin), FromDto(dto.direction));

        public static Ray2DDto ToDto(Ray2D r) => new()
        {
            origin = ToDto(r.origin),
            direction = ToDto(r.direction)
        };

        public static Ray2D FromDto(Ray2DDto dto)
            => new(FromDto(dto.origin), FromDto(dto.direction));

        // -------------------------
        // GRADIENT
        // Null-safe + clamp times/alpha to avoid corrupted/old saves breaking.
        // -------------------------
        public static GradientDto ToDto(Gradient g)
        {
            var cks = g.colorKeys;
            var aks = g.alphaKeys;

            var dto = new GradientDto
            {
                mode = (int)g.mode,
                colorKeys = new GradientColorKeyDto[cks.Length],
                alphaKeys = new GradientAlphaKeyDto[aks.Length]
            };

            for (int i = 0; i < cks.Length; i++)
            {
                dto.colorKeys[i] = new GradientColorKeyDto
                {
                    color = ToDto(cks[i].color),
                    time = Mathf.Clamp01(cks[i].time)
                };
            }

            for (int i = 0; i < aks.Length; i++)
            {
                dto.alphaKeys[i] = new GradientAlphaKeyDto
                {
                    alpha = Mathf.Clamp01(aks[i].alpha),
                    time = Mathf.Clamp01(aks[i].time)
                };
            }

            return dto;
        }

        public static Gradient FromDto(GradientDto dto)
        {
            var g = new Gradient
            {
                mode = (GradientMode)dto.mode
            };

            var dtoC = dto.colorKeys ?? Array.Empty<GradientColorKeyDto>();
            var dtoA = dto.alphaKeys ?? Array.Empty<GradientAlphaKeyDto>();

            var cks = new GradientColorKey[dtoC.Length];
            for (int i = 0; i < dtoC.Length; i++)
            {
                cks[i] = new GradientColorKey(
                    FromDto(dtoC[i].color),
                    Mathf.Clamp01(dtoC[i].time)
                );
            }

            var aks = new GradientAlphaKey[dtoA.Length];
            for (int i = 0; i < dtoA.Length; i++)
            {
                aks[i] = new GradientAlphaKey(
                    Mathf.Clamp01(dtoA[i].alpha),
                    Mathf.Clamp01(dtoA[i].time)
                );
            }

            g.SetKeys(cks, aks);
            return g;
        }

        // -------------------------
        // ANIMATION CURVE
        // -------------------------
        public static AnimationCurveDto ToDto(AnimationCurve curve)
        {
            if (curve == null)
            {
                return new AnimationCurveDto
                {
                    preWrapMode = (int)WrapMode.Once,
                    postWrapMode = (int)WrapMode.Once,
                    keys = Array.Empty<KeyframeDto>()
                };
            }

            var keys = curve.keys;
            var dtoKeys = new KeyframeDto[keys.Length];

            for (int i = 0; i < keys.Length; i++)
            {
                var k = keys[i];

                dtoKeys[i] = new KeyframeDto
                {
                    time = k.time,
                    value = k.value,
                    inTangent = k.inTangent,
                    outTangent = k.outTangent,
                    inWeight = k.inWeight,
                    outWeight = k.outWeight,
                    weightedMode = (int)k.weightedMode,

                    // Runtime-safe: Unity doesn't expose tangentMode in runtime API consistently.
                    // We keep a field for compatibility but store 0 by default.
                    tangentMode = 0
                };
            }

            return new AnimationCurveDto
            {
                preWrapMode = (int)curve.preWrapMode,
                postWrapMode = (int)curve.postWrapMode,
                keys = dtoKeys
            };
        }

        public static AnimationCurve FromDto(AnimationCurveDto dto)
        {
            var dtoKeys = dto.keys ?? Array.Empty<KeyframeDto>();
            var keys = new Keyframe[dtoKeys.Length];

            for (int i = 0; i < dtoKeys.Length; i++)
            {
                var d = dtoKeys[i];

                // Use the most complete constructor available
                var k = new Keyframe(d.time, d.value, d.inTangent, d.outTangent)
                {
                    inWeight = d.inWeight,
                    outWeight = d.outWeight,
                    weightedMode = (WeightedMode)d.weightedMode
                };

                keys[i] = k;
            }

            var curve = new AnimationCurve(keys)
            {
                preWrapMode = (WrapMode)dto.preWrapMode,
                postWrapMode = (WrapMode)dto.postWrapMode
            };

            return curve;
        }
    }
}
