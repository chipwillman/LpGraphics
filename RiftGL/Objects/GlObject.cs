﻿namespace RiftGL.Objects
{
    using System;
    using System.Collections.Generic;

    using RiftGL.View;

    public class GlObject : Inventory
    {
        protected GlObject()
        {
            Position = new Vector();
            Velocity = new Vector();
            Acceleration = new Vector();
        }

        protected GlObject(Inventory parent) : base(parent)
        {
            Position = new Vector();
            Velocity = new Vector();
            Acceleration = new Vector();
        }

        public Vector Acceleration { get; set; }

        public Vector Position { get; set; }

        public Vector Velocity { get; set; }

        public float Size { get; set; }

        private Stack<float[]> glViewMatrix = new Stack<float[]>();

        public void Draw(Camera camera)
        {
            this.PushMatrix();
            this.OnDraw(camera);
            this.PopMatrix();
        }

        public void PushMatrix()
        {
            var modelView = new float[16];
            Array.Copy(ViewPort.Matrices.ModelView, modelView, modelView.Length);
            glViewMatrix.Push(modelView);
        }

        public void PopMatrix()
        {
            ViewPort.Matrices.ModelView = glViewMatrix.Pop();
        }

        public void Animate(float deltaTime)
        {
            this.OnAnimate(deltaTime);

            if (this.HasChild)
            {
                ((GlObject)Child).Animate(deltaTime);
            }

            if (this.HasParent && !this.IsLastChild())
            {
                ((GlObject)Next).Animate(deltaTime);
            }
        }

        public void ProcessCollisions(GlObject obj)
        {
            if ((obj.Position - this.Position).Length() <= (obj.Size + this.Size))
            {
                this.OnCollision(obj);
                if (this.HasChild)
                {
                    ((GlObject)Child).ProcessCollisions(obj);
                }

                if (this.HasParent && !this.IsLastChild())
                {
                    ((GlObject)Next).ProcessCollisions(obj);
                }
            }

            if (obj.HasChild)
            {
                this.ProcessCollisions(((GlObject)obj.Child));
            }

            if (obj.HasParent && !obj.IsLastChild())
            {
                this.ProcessCollisions(((GlObject)obj.Next));
            }
        }

        public void Prepare()
        {
            this.OnPrepare();

            if (HasChild)
            {
                ((GlObject)Child).Prepare();
            }

            if (HasParent && !this.IsLastChild())
            {
                ((GlObject)Next).Prepare();
            }
        }

        #region Implementation

        protected virtual void OnAnimate(float deltaTime)
        {
            this.Position += this.Velocity * deltaTime;
            this.Velocity += this.Acceleration * deltaTime;
        }

        protected virtual void OnDraw(Camera camera)
        {
        }

        protected virtual void OnCollision(GlObject collisionObject)
        {
            
        }

        protected virtual void OnPrepare()
        {
            this.ProcessCollisions(this.FindRoot());
        }

        protected virtual void Load()
        {
            
        }

        protected virtual void Unload()
        {
            
        }

        protected GlObject FindRoot()
        {
            if (this.Parent != null)
            {
                return ((GlObject)this.Parent).FindRoot();
            }
            return this;
        }

        #endregion
    }
}
