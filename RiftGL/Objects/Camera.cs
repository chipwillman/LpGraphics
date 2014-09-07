namespace RiftGL.Objects
{
    using System;

    using RiftGL.View;

    public class Camera : ViewPort
    {
        public Camera()
        {
            position = new Vector(0, 0, 0);
            lookAt = new Vector(0, 0, 1);
            forward = new Vector(0, 0, 1);
            up = new Vector(0, 1, 0);
            right = new Vector(1, 0, 0);
            velocity = new Vector(0, 0, 0);
            acceleration = new Vector(0, 0, 0);

            yaw = 0.0f;
            pitch = 0f;
        }

        public Vector position;			// position of camera
        public Vector velocity;			// velocity of camera
        public Vector acceleration;		// acceleration of camera
        public Vector lookAt;			// lookat vector

        // up, forward, right vectors
        public Vector up;
        public Vector forward;
        public Vector right;

        // yaw and pitch angles
        public float yaw;
        public float pitch;

        public int screenWidth, screenHeight;
        public int centerX, centerY;

        public void LookAt(GlObject obj)
        {
            velocity = obj.Position - lookAt;
            initLookAt = lookAt;
            finalLookAt = obj.Position;

            lookAtAccel = new Vector() - lookAt * 0.25f;

            this.UpdateLookAt();
        }

        public void LookAtNow(GlObject obj)
        {
            lookAt = obj.Position;
        }

        public void MoveTo(GlObject obj)
        {
            velocity = obj.Position - position;
            initPosition = position;
            finalPosition = obj.Position;

            acceleration = new Vector() - position * 0.25f;

            this.UpdateMoveTo();
        }

        public void MoveToNow(GlObject obj)
        {
            position = obj.Position;
        }

        public void MoveToNow(float x, float y, float z)
        {
            position.X = x;
            position.Y = y;
            position.Z = z;
        }

    	// right rotation along y-axis (yaw)
        public void RotateYaw(float radians)
        {

            float sine = (float)Math.Sin(radians);
            float cosine = (float)Math.Cos(radians);

            right.X = cosine * right.Length();
            right.Z = sine * right.Length();

            forward.X = -sine * forward.Length();
            forward.Z = cosine * forward.Length();

            /*	   x      y    z      p
                 |  cos(A)  0  -sin(A)  0 |
             M = |  0       1   0       0 |
                 |  sin(A)  0   cos(A)  0 |
                 |  0       0   0       1 |
            */
        }

        public void RotatePitch(float radians)
        {
            float sine = (float)Math.Sin(radians);
            float cosine = (float)Math.Cos(radians);

            up.Y = cosine * up.Length();
            up.Z = sine * up.Length();

            forward.Y = -sine * forward.Length();
            forward.Z = cosine * forward.Length();
            /*     x   y      z       p
                 |  1  0       0       0 |
             M = |  0  cos(A) -sin(A)  0 |
                 |  0  sin(A)  cos(A)  0 |
                 |  0  0       0       1 |
            */
        }

        public void RotateRoll(float radians)
        {
            float sine = (float)Math.Sin(radians);
            float cosine = (float)Math.Cos(radians);

            right.X = cosine * right.Length();
            right.Y = sine * right.Length();

            up.X = -sine * forward.Length();
            up.Y = cosine * forward.Length();
            /*
                 |  cos(A)  -sin(A)   0   0 |
             M = |  sin(A)   cos(A)   0   0 |
                 |  0        0        1   0 |
                 |  0        0        0   1 |
            */
        }

        public float Deg2Rad(float a)
        {
            return (float)Math.PI / 180 * (a);
        }

        // do physics calculations
        public void Animate(float deltaTime)
        {
            if ((yaw >= 360.0f) || (yaw <= -360.0f))
                  yaw = 0.0f;

            if (pitch > 60.0f)
                 pitch = 60.0f;
            if (pitch < -60.0f)
                 pitch = -60.0f;
            float cosYaw = (float)Math.Cos(this.Deg2Rad(yaw));
            float sinYaw = (float)Math.Sin(this.Deg2Rad(yaw));
            float sinPitch = (float)Math.Sin(this.Deg2Rad(pitch));

            // added line
            float cosPitch = (float)Math.Cos(this.Deg2Rad(pitch));

            float speed = velocity.Z * deltaTime;
            float strafeSpeed = velocity.X * deltaTime;

            if (speed > 15.0f)
                speed = 15.0f;
            if (strafeSpeed > 15.0f)
                strafeSpeed = 15.0f;
            if (speed < -15.0f)
                speed = -15.0f;
            if (strafeSpeed < -15.0f)
                strafeSpeed = -15.0f;

            if (velocity.Length() > 0.0)
                acceleration = new Vector() -velocity * 1.5f;

            velocity += acceleration*deltaTime;

            //position.X += (float)(Math.Cos(this.Deg2Rad(yaw + 90.0f))) * strafeSpeed;
            //position.Z += (float)(Math.Sin(this.Deg2Rad(yaw + 90.0f))) * strafeSpeed;
            //position.X += cosYaw * speed;
            //position.Z += sinYaw * speed;

            // added *cosPitch
            lookAt.X = position.X + (cosYaw*cosPitch);
            lookAt.Y = position.Y + sinPitch;
            lookAt.Z = position.Z + (sinYaw*cosPitch);
            
            var cameraMatrix = MakeLookAt(position, lookAt, up);

            // Make a view matrix from the camera matrix.
            Matrices.ModelView = MakeInverse(cameraMatrix);
        }

        #region Implementation

        private Vector initPosition;

        private Vector finalPosition;

        private Vector initLookAt;

        private Vector finalLookAt;

        private Vector lookAtVel;

        private Vector lookAtAccel;

        private void UpdateLookAt()
        {
            var look = finalLookAt -lookAt;
            lookAtVel = look * 0.5f;
        }

        private void UpdateMoveTo()
        {
            var pos = finalPosition - position;
            velocity = pos * 0.5f;
        }

        private float[] MakeLookAt(Vector cameraPosition, Vector target, Vector up)
        {
            var zAxis = (cameraPosition - target);
            zAxis.Normalize();
            var xAxis = up ^ zAxis;
            var yAxis = zAxis ^ xAxis;

            return new[]
                       {
                           xAxis.X, xAxis.Y, xAxis.Z, 0, 
                           yAxis.X, yAxis.Y, yAxis.Z, 0, 
                           zAxis.X, zAxis.Y, zAxis.Z, 0,
                           cameraPosition.X, 
                           cameraPosition.Y, 
                           cameraPosition.Z, 
                           1
                       };
        }

        private float[] MakeInverse(float[] m) 
        {
        var m00 = m[0 * 4 + 0];
        var m01 = m[0 * 4 + 1];
        var m02 = m[0 * 4 + 2];
        var m03 = m[0 * 4 + 3];
        var m10 = m[1 * 4 + 0];
        var m11 = m[1 * 4 + 1];
        var m12 = m[1 * 4 + 2];
        var m13 = m[1 * 4 + 3];
        var m20 = m[2 * 4 + 0];
        var m21 = m[2 * 4 + 1];
        var m22 = m[2 * 4 + 2];
        var m23 = m[2 * 4 + 3];
        var m30 = m[3 * 4 + 0];
        var m31 = m[3 * 4 + 1];
        var m32 = m[3 * 4 + 2];
        var m33 = m[3 * 4 + 3];
        var tmp_0  = m22 * m33;
        var tmp_1  = m32 * m23;
        var tmp_2  = m12 * m33;
        var tmp_3  = m32 * m13;
        var tmp_4  = m12 * m23;
        var tmp_5  = m22 * m13;
        var tmp_6  = m02 * m33;
        var tmp_7  = m32 * m03;
        var tmp_8  = m02 * m23;
        var tmp_9  = m22 * m03;
        var tmp_10 = m02 * m13;
        var tmp_11 = m12 * m03;
        var tmp_12 = m20 * m31;
        var tmp_13 = m30 * m21;
        var tmp_14 = m10 * m31;
        var tmp_15 = m30 * m11;
        var tmp_16 = m10 * m21;
        var tmp_17 = m20 * m11;
        var tmp_18 = m00 * m31;
        var tmp_19 = m30 * m01;
        var tmp_20 = m00 * m21;
        var tmp_21 = m20 * m01;
        var tmp_22 = m00 * m11;
        var tmp_23 = m10 * m01;

        var t0 = (tmp_0 * m11 + tmp_3 * m21 + tmp_4 * m31) -
            (tmp_1 * m11 + tmp_2 * m21 + tmp_5 * m31);
        var t1 = (tmp_1 * m01 + tmp_6 * m21 + tmp_9 * m31) -
            (tmp_0 * m01 + tmp_7 * m21 + tmp_8 * m31);
        var t2 = (tmp_2 * m01 + tmp_7 * m11 + tmp_10 * m31) -
            (tmp_3 * m01 + tmp_6 * m11 + tmp_11 * m31);
        var t3 = (tmp_5 * m01 + tmp_8 * m11 + tmp_11 * m21) -
            (tmp_4 * m01 + tmp_9 * m11 + tmp_10 * m21);

        var d = 1.0f / (m00 * t0 + m10 * t1 + m20 * t2 + m30 * t3);

            return new float[]
                       {
                           d * t0, d * t1, d * t2, d * t3,
                           d * ((tmp_1 * m10 + tmp_2 * m20 + tmp_5 * m30) - (tmp_0 * m10 + tmp_3 * m20 + tmp_4 * m30)),
                           d * ((tmp_0 * m00 + tmp_7 * m20 + tmp_8 * m30) - (tmp_1 * m00 + tmp_6 * m20 + tmp_9 * m30)),
                           d * ((tmp_3 * m00 + tmp_6 * m10 + tmp_11 * m30) - (tmp_2 * m00 + tmp_7 * m10 + tmp_10 * m30)),
                           d * ((tmp_4 * m00 + tmp_9 * m10 + tmp_10 * m20) - (tmp_5 * m00 + tmp_8 * m10 + tmp_11 * m20)),
                           d
                           * ((tmp_12 * m13 + tmp_15 * m23 + tmp_16 * m33) - (tmp_13 * m13 + tmp_14 * m23 + tmp_17 * m33))
                           ,
                           d
                           * ((tmp_13 * m03 + tmp_18 * m23 + tmp_21 * m33) - (tmp_12 * m03 + tmp_19 * m23 + tmp_20 * m33))
                           ,
                           d
                           * ((tmp_14 * m03 + tmp_19 * m13 + tmp_22 * m33) - (tmp_15 * m03 + tmp_18 * m13 + tmp_23 * m33))
                           ,
                           d
                           * ((tmp_17 * m03 + tmp_20 * m13 + tmp_23 * m23) - (tmp_16 * m03 + tmp_21 * m13 + tmp_22 * m23))
                           ,
                           d
                           * ((tmp_14 * m22 + tmp_17 * m32 + tmp_13 * m12) - (tmp_16 * m32 + tmp_12 * m12 + tmp_15 * m22))
                           ,
                           d
                           * ((tmp_20 * m32 + tmp_12 * m02 + tmp_19 * m22) - (tmp_18 * m22 + tmp_21 * m32 + tmp_13 * m02))
                           ,
                           d
                           * ((tmp_18 * m12 + tmp_23 * m32 + tmp_15 * m02) - (tmp_22 * m32 + tmp_14 * m02 + tmp_19 * m12))
                           ,
                           d
                           * ((tmp_22 * m22 + tmp_16 * m02 + tmp_21 * m12) - (tmp_20 * m12 + tmp_23 * m22 + tmp_17 * m02))
                       };
        }
        #endregion
    }
}
