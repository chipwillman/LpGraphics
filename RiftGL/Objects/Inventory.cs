namespace RiftGL.Objects
{
    public class Inventory
    {
        public Inventory()
        {
            this.Previous = this;
            this.Next = this;
        }

        public Inventory(Inventory parent) : this()
        {
            this.AttachTo(parent);
        }

        public Inventory Parent { get; set; }

        public Inventory Child { get; set; }

        public Inventory Previous { get; set; }

        public Inventory Next { get; set; }

        public bool HasParent
        {
            get
            {
                return this.Parent != null;
            }
        }

        public bool HasChild
        {
            get
            {
                return this.Child != null;
            }
        }

        public bool IsFirstChild()
        {
            if (this.Parent != null)
            {
                return this.Parent.Child == this;
            }
            return false;
        }

        public bool IsLastChild()
        {
            if (this.Parent != null)
            {
                return this.Parent.Child.Previous == this;
            }
            return false;
        }

        public void AttachTo(Inventory newParent)
        {
            if (this.Parent != null)
            {
                this.Detach();
            }

            this.Parent = newParent;

            if (this.Parent.Child != null)
            {
                this.Previous = this.Parent.Child.Previous;
                this.Next = this.Parent.Child;
                this.Parent.Child.Previous.Next = this;
                this.Parent.Child.Previous = this;
            }
            else
            {
                this.Parent.Child = this;
            }
        }

        public void Attach(Inventory newChild)
        {
            if (newChild.HasParent)
            {
                newChild.Detach();
            }

            newChild.Parent = this;

            if (this.Child != null)
            {
                newChild.Previous = this.Child.Previous;
                newChild.Next = this.Child;
                this.Child.Previous.Next = newChild;
                this.Child.Previous = newChild;
            }
            else
            {
                this.Child = newChild;
            }
        }

        public void Detach()
        {
            if (this.Parent != null && this.Parent.Child == this)
            {
                if (this.Next != this)
                {
                    this.Parent.Child = this.Next;
                }
                else
                {
                    this.Parent.Child = null;
                }
            }

            this.Previous.Next = this.Next;
            Next.Previous = this.Previous;

            this.Previous = this;
            this.Next = this;
        }

        public int CountNodes()
        {
            if (this.Child != null)
            {
                return this.Child.CountNodes() + 1;
            }
            return 1;
        }
    }
}
