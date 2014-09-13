namespace RiftGL.Objects
{
    using System;
    using System.Collections.Generic;

    using RiftGL.View;

    public class Camera : ViewPort
    {
        public Camera()
        {
            MatrixTranslation = GLMatrix4.create();
            MatrixRotation = GLMatrix4.create();
            GLMatrix4.identity(MatrixTranslation);
            GLMatrix4.identity(MatrixRotation);
            //GLMatrix4.rotateX(MatrixTranslation, 0.0f, MatrixRotation);
            Location = new Vector();
            Rotation = new Vector();
            Velocity = new Vector();
            fAcceleration = new Vector();
            LookAt = new Vector(0, 0, 1);
            fLookAtAcceleration = new Vector();
            fLookAtVelocity = new Vector();
            fUp = new Vector(0, 1, 0);
        }

        public dynamic BillboardMatrix
        {
            get { return fBillboardMatrix; }
            set { fBillboardMatrix = value; }
        }
        
        public Vector GlobalDirection
        {
            get { return fGlobalDirection; }
            set { fGlobalDirection = value; }
        }
        
        public event EventHandler EyePartChanged;
        public Vector EyePart
        {
            get { return Location; }
            set
            {
                if (Location != value)
                {
                    Location = value;
                    if (EyePartChanged != null)
                    {
                        EyePartChanged(this, EventArgs.Empty);
                    }
                }
                Location = value;
            }
        }
        
        public Vector StartPos;
        public Vector StartRot;

        public Vector EndPos;
        public Vector EndRot;
        
        private dynamic fMatrixWorld;
        public dynamic MatrixWorld
        {
            get
            {
                return fMatrixWorld;
            }
            set
            {
                fMatrixWorld = value;
            }
        }
        
        public dynamic MatrixTranslation;
        public dynamic MatrixRotation;
        
        public event EventHandler LookAtChanged;
        public Vector LookAt
        {
            get
            {
                return fLookAt;
            }
            set
            {
                fLookAt = value;
                if (LookAtChanged != null)
                {
                    LookAtChanged(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler VelocityChanged;
        public Vector Velocity
        {
            get { return fVelocity; }
            set
            {
                if (value.Length() > 15)
                {
                    value.Normalize();
                    value *= 15f;
                }

                if (value.Length() < -15)
                {
                    value.Normalize();
                    value *= -15f;
                }

                fVelocity = value;
                if (VelocityChanged != null)
                {
                    VelocityChanged(this, EventArgs.Empty);
                }
            }
        }

        public Vector Location
        {
            get { return fLocation; }
            set
            {
                fLocation = value;
            }
        }

        private Vector fRotation;
        public event EventHandler RotationChanged;
        public Vector Rotation
        {
            get { return fRotation; }
            set
            {
                fRotation = value;
                if (RotationChanged != null)
                {
                    RotationChanged(this, EventArgs.Empty);
                }
            }
        }

        public float Yaw
        {
            get { return RAD2DEG(Rotation.Y); }
            set
            {
                if ((value >= 360.0f) || (value <= -360.0f))
                {
                    fRotation.Y = 0.0f;
                }
                else
                {
                    fRotation.Y = DEG2RAD(value);
                }
            }
        }

        public float Pitch
        {
            get { return RAD2DEG(fRotation.X); }
            set
            {
                if (value > 60.0f)
                {
                    value = 60.0f;
                }
                if (value < -60.0f)
                {
                    value = -60.0f;
                }
                fRotation.X = DEG2RAD(value);
            }
        }

        public void Center()
        {
            fRotation.Y = 0.0f;
        }

        public void IncrementCameraYaw(float Angle)
        {
            fRotation.X += DEG2RAD(Angle);
        }

        public void IncrementCameraPitch(float Angle)
        {
            fRotation.Y += DEG2RAD(Angle);
        }
        
        public void SetBillBoardMatrix(Vector position)
        {
            fBillboardMatrix.M41 = position.X;
            fBillboardMatrix.M42 = position.Y;
            fBillboardMatrix.M43 = position.Z;
            Matrices.Billboard = fBillboardMatrix;
        }
        
        public dynamic GetMatrix()
        {
            Update();
            return MatrixWorld;
        }
        
        #region Implementation

        
        protected dynamic fBillboardMatrix;
        protected Vector fGlobalDirection;
        
        protected Vector fInitPosition;
        protected Vector fFinalPosition;
        protected Vector fInitLookAt;
        protected Vector fFinalLookAt;

        protected Vector fLookAtVelocity;
        protected Vector fLookAtAcceleration;
        
        protected Vector fUp;
        protected Vector fForward;
        protected Vector fRight;

        protected Vector fLocation;
        protected Vector fLookAt;
        protected Vector fVelocity;
        protected Vector fAcceleration;
        
        protected const float PI = 3.14159265359f;
        
        protected float DEG2RAD(float a)
        {
            return PI / 180 * (a);
        }
        
        protected float RAD2DEG(float a)
        {
            return 180 / PI * (a);
        }
        
        public bool Point(float XEye, float YEye, float ZEye, float XAt, float YAt, float ZAt)
        {
            float XRot, YRot, XDiff, YDiff, ZDiff;

            // Calculate angles between points
            XDiff = XAt - XEye;
            YDiff = YAt - YEye;
            ZDiff = ZAt - ZEye;
            XRot = (float)Math.Atan2(-YDiff, Math.Sqrt(XDiff * XDiff + ZDiff * ZDiff));
            YRot = (float)Math.Atan2(XDiff, ZDiff);

            Location = new Vector(XEye, YEye, ZEye);
            Rotation = new Vector(XRot, YRot, 0.0f);

            return true;
        }
        
        public bool SetStartTrack()
        {
            StartPos.X = Location.X;
            StartPos.Y = Location.Y;
            StartPos.Z = Location.Z;
            StartRot.X = Rotation.X;
            StartRot.Y = Rotation.Y;
            StartRot.Z = Rotation.Z;
            return true;
        }
        
        public bool SetEndTrack()
        {
            EndPos.X = Location.X;
            EndPos.Y = Location.Y;
            EndPos.Z = Location.Z;
            EndRot.X = Rotation.X;
            EndRot.Y = Rotation.Y;
            EndRot.Z = Rotation.Z;
            return true;
        }
        
        //---------------------------------------------------------------
        /// <summary>
        /// Track uses the predefined Start and End Track Positions and Rotation and Positions the Camera
        /// </summary>
        /// <param name="Time">Percentage of Ellapsed Time</param>
        /// <param name="Length">Total length of time in milliseconds</param>
        /// <returns></returns>
        //---------------------------------------------------------------
        public bool Track(float Time, float Length)
        {
            float x, y, z;
            float TimeOffset;

            TimeOffset = Length * Time;

            x = (EndPos.X - StartPos.X) / Length * TimeOffset;
            y = (EndPos.Y - StartPos.Y) / Length * TimeOffset;
            z = (EndPos.Z - StartPos.Z) / Length * TimeOffset;
            Location = new Vector(StartPos.X + x, StartPos.Y + y, StartPos.Z + z);

            x = (EndRot.X - StartRot.X) / Length * TimeOffset;
            y = (EndRot.Y - StartRot.Y) / Length * TimeOffset;
            z = (EndRot.Z - StartRot.Z) / Length * TimeOffset;
            Rotation = new Vector(StartRot.X + x, StartRot.Y + y, StartRot.Z + z);

            return true;
        }
        
        public bool Update()
        {
            //var matrixLookAt = GLMatrix4.lookAt(Location, LookAt, fUp);

            GLMatrix4.identity(MatrixTranslation);

            GLMatrix4.identity(MatrixRotation);
            GLMatrix4.translate(MatrixTranslation, (new Vector() - fLocation).ToFloatVector(), MatrixTranslation);
            GLMatrix4.rotateX(MatrixRotation, Rotation.X, MatrixRotation);
            GLMatrix4.rotateY(MatrixRotation, Rotation.Y, MatrixRotation);
            GLMatrix4.rotateZ(MatrixRotation, Rotation.Z, MatrixRotation);

            GLMatrix4.multiply(MatrixRotation, MatrixTranslation, Matrices.ModelView);
            return true;
        }
        
        public void Animate(float DeltaTime)
        {
            float CosineYaw = (float)Math.Cos(-Rotation.Y);
            float SinYaw = (float)Math.Sin(-Rotation.Y);
            float SinPitch = (float)Math.Sin(Rotation.X);

            float Speed = Velocity.Z * DeltaTime;
            float StrafeSpeed = Velocity.X * DeltaTime;

            if (Speed > 15.0f)
            {
                Speed = 15.0f;
            }
            if (StrafeSpeed > 15.0f)
            {
                StrafeSpeed = 15.0f;
            }
            if (Speed < -15.0f)
            {
                Speed = -15.0f;
            }
            if (StrafeSpeed < -15.0f)
            {
                StrafeSpeed = -15.0f;
            }

            if (Velocity.Length() > 0.0)
            {
                fAcceleration = (Velocity * -1.5f);
            }

            fAcceleration.Y = -9.8f;
            Velocity += fAcceleration * DeltaTime;

            //fLocation.X += (float)(Math.Cos(Rotation.X + PI / 4)) * StrafeSpeed;
            //fLocation.Z += (float)(Math.Sin(Rotation.X + PI / 4)) * StrafeSpeed;
            var delta = new Vector();

            delta.Y += Velocity.Y * DeltaTime;
            delta.X += SinYaw * Speed;
            delta.Z += CosineYaw * Speed;

            fLocation += delta;

            //fLookAt.X = Location.X + CosineYaw;
            //fLookAt.Y = Location.Y + SinPitch;
            //fLookAt.Z = Location.Z + SinYaw;
        }
        
        private Stack<dynamic> fWorldMatrixStack;
        
        protected Stack<dynamic> WorldMatrixStack
        {
            get
            {
                if (fWorldMatrixStack == null)
                {
                    fWorldMatrixStack = new Stack<dynamic>();
                }
                return fWorldMatrixStack;
            }
        }
        
        public dynamic PushMatrixState()
        {
            var Result = Matrices.ModelView;
            WorldMatrixStack.Push(Result);
            return Result;
        }
        
        public dynamic PopMatrixState()
        {
            var result = WorldMatrixStack.Pop();
            SetWorldMatrix(result);
            return result;
        }
        
        public void SetWorldMatrix(dynamic WorldMatrix)
        {
            Matrices.ModelView = WorldMatrix;
        }
        
        #endregion
    }
}


