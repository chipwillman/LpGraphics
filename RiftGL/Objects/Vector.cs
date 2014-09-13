namespace RiftGL.Objects
{
    using System;

    public class Vector
    {
        public Vector()
        {
            
        }

        public Vector(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Vector(Vector vector)
        {
            this.X = vector.X;
            this.Y = vector.Y;
            this.Z = vector.Z;
        }

        public float X;

        public float Y;

        public float Z;

        public static bool operator ==(Vector x, Vector y)
        {
            return (x != null && y != null && Math.Abs(x.X - y.X) < 0.001f && (Math.Abs(x.Y - y.Y) < 0.001f) && (Math.Abs(x.Z - y.Z) < 0.001f));
        }

        public static bool operator !=(Vector x, Vector y)
        {
            return !(x == y);
        }

        public static Vector operator +(Vector x, Vector y)
        {
            return new Vector(x.X + y.X, x.Y + y.Y, x.Z + y.Z);
        }

        public static Vector operator -(Vector x, Vector y)
        {
            return new Vector(x.X - y.X, x.Y - y.Y, x.Z - y.Z);
        }

        public static Vector operator *(Vector x, Vector y)
        {
            var result = new Vector(x);
            result.X *= y.X;
            result.Y *= y.Y;
            result.Z *= y.Z;
            return result;
        }

        public static Vector operator /(Vector x, float y)
        {
            var reciprical = 1 / y;
            var result = new Vector(x);
            result.X *= reciprical;
            result.Y *= reciprical;
            result.Z *= reciprical;
            return result;
        }

        public static Vector operator *(Vector x, float y)
        {
            var result = new Vector(x);
            result.X *= y;
            result.Y *= y;
            result.Z *= y;
            return result;
        }

        public Vector CrossProduct(Vector vector)
        {
            return new Vector(Y * vector.Z - Z * vector.Y, Z * vector.X - X * vector.Z, X * vector.Y - Y * vector.X);
        }

        public static Vector operator ^(Vector x, Vector y)
        {
            return new Vector(x.Y * y.Z - x.Z * y.Y, x.Z * y.X - x.X * y.Z, x.X * y.Y - x.Y * y.X);
        }

        public float DotProduct(Vector vector)
        {
            return X * vector.X + Y * vector.Y + Z * vector.Z;
        }

        public static float operator %(Vector x, Vector y)
        {
            return x.X * y.X + x.Y * y.Y + x.Z * y.Z;
        }

        public float Length()
        {
            return (float)Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public Vector UnitVector()
        {
            return this / this.Length();
        }

        public void Normalize()
        {
            var result = this / this.Length();
            X = result.X;
            Y = result.Y;
            Z = result.Z;
        }

        public static float operator !(Vector x)
        {
            return x.Length();
        }

        public static Vector operator |(Vector x, float length)
        {
            var result = new Vector(x);
            result *= (length / x.Length());
            return result;
        }

        public float Angle(Vector normal)
        {
            return (float)Math.Acos(this % normal);
        }

        public Vector Reflection(Vector normal)
        {
            var unitVector = this | 1;
            return (unitVector - normal * 2.0f * (unitVector % normal)) * this.Length();
        }

        public float[] ToFloatVector()
        {
            return new[] { X, Y, Z };
        }
    }
}
