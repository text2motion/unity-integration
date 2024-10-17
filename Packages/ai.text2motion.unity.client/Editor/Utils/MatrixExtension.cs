using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Text2Motion.Utils
{
    // Referenced from https://gist.github.com/ogxd/c48b5413994ebcf224ae765a7f07ee8e
    public static class MatrixExtensions
    {
        private static void GetTRS(this Matrix4x4 matrix, out Vector3 translation, out Quaternion rotation, out Vector3 scale)
        {
            float det = matrix.GetDeterminant();

            // Scale
            scale.x = matrix.MultiplyVector(new Vector3(1, 0, 0)).magnitude;
            scale.y = matrix.MultiplyVector(new Vector3(0, 1, 0)).magnitude;
            scale.z = matrix.MultiplyVector(new Vector3(0, 0, 1)).magnitude;
            scale = (det < 0) ? -scale : scale;

            // Rotation
            Matrix4x4 rotationMatrix = matrix;
            rotationMatrix.m03 = rotationMatrix.m13 = rotationMatrix.m23 = 0f;
            rotationMatrix *= new Matrix4x4 { m00 = 1f / scale.x, m11 = 1f / scale.y, m22 = 1f / scale.z, m33 = 1 };
            rotation = Quaternion.LookRotation(rotationMatrix.GetColumn(2), rotationMatrix.GetColumn(1));

            // Position
            translation = matrix.GetColumn(3);
        }

        private static float GetDeterminant(this Matrix4x4 matrix)
        {
            return matrix.m00 * (matrix.m11 * matrix.m22 - matrix.m12 * matrix.m21) -
                    matrix.m10 * (matrix.m01 * matrix.m22 - matrix.m02 * matrix.m21) +
                    matrix.m20 * (matrix.m01 * matrix.m12 - matrix.m02 * matrix.m11);
        }

        public static List<decimal> ToDecimalList(this Matrix4x4 matrix)
        {
            List<decimal> result = new(new decimal[16]);

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    result[i + j * 4] = (decimal)matrix[i, j];
                }
            }
            return result;
        }

        public static Matrix4x4 ToMatrix4x4(this List<decimal> list)
        {
            if (list.Count != 16)
            {
                throw new ArgumentException("List must have exactly 16 elements.");
            }

            var matrix = new Matrix4x4();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    matrix[i, j] = (float)list[i + j * 4];
                }
            }
            return matrix;
        }

        public static void SetFromLocalMatrix(this Transform transform, Matrix4x4 matrix, bool isLeftHanded, bool isZup, float scaleFactor)
        {

            matrix.GetTRS(out Vector3 t, out Quaternion r, out Vector3 s);

            // Apply scale factor
            t *= scaleFactor;

            // If matrix is from Z-up system, we convert the TRS to match Unity's Y-up system
            if (isZup)
            {
                t = new Vector3(t.x, t.z, t.y);
                r = new Quaternion(r.x, r.z, r.y, -r.w);
                s = new Vector3(s.x, s.z, s.y);
            }

            // If matrix is from Left-Handed system, we convert the TRS to match Unity's Right-Handed system
            if (!isLeftHanded)
            {
                t = new Vector3(-t.x, t.y, t.z);
                r = new Quaternion(-r.x, r.y, r.z, -r.w);
            }

            // Assign local TRS to transform
            transform.SetLocalPositionAndRotation(t, r);
            transform.localScale = s;
        }

        public static Matrix4x4 GetLocalMatrix(this Transform transform, bool isLeftHanded, bool isZup, float scaleFactor)
        {
            return GetLocalMatrix(transform.localPosition, transform.localRotation, transform.localScale, isLeftHanded, isZup, scaleFactor);
        }


        public static Matrix4x4 GetLocalMatrix(Vector3 t, Quaternion r, Vector3 s, bool isLeftHanded, bool isZup, float scaleFactor)
        {
            t *= scaleFactor;

            // If matrix should is for Z-up system, we convert the TRS from Unity's Y-up system
            if (isZup)
            {
                t = new Vector3(t.x, t.z, t.y);
                r = new Quaternion(r.x, r.z, r.y, -r.w);
                s = new Vector3(s.x, s.z, s.y);
            }

            // If matrix should is for Left-Handed system, we convert the TRS from Unity's Right-Handed system
            if (!isLeftHanded)
            {
                t = new Vector3(-t.x, t.y, t.z);
                r = new Quaternion(-r.x, r.y, r.z, -r.w);
            }

            Matrix4x4 matrix = Matrix4x4.identity;
            matrix.SetTRS(t, r, s);

            return matrix;
        }

        public static Matrix4x4 GetWorldMatrix(this Transform transform, bool isLeftHanded, bool isZup, float scaleFactor)
        {
            transform.GetPositionAndRotation(out Vector3 t, out Quaternion r);
            Vector3 s = transform.lossyScale;

            t *= scaleFactor;

            // If matrix should is for Z-up system, we convert the TRS from Unity's Y-up system
            if (isZup)
            {
                t = new Vector3(t.x, t.z, t.y);
                r = new Quaternion(r.x, r.z, r.y, -r.w);
                s = new Vector3(s.x, s.z, s.y);
            }

            // If matrix should is for Left-Handed system, we convert the TRS from Unity's Right-Handed system
            if (!isLeftHanded)
            {
                t = new Vector3(-t.x, t.y, t.z);
                r = new Quaternion(-r.x, r.y, r.z, -r.w);
            }

            Matrix4x4 matrix = Matrix4x4.identity;
            matrix.SetTRS(t, r, s);

            return matrix;
        }
    }
}