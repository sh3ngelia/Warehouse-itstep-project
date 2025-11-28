using System;

namespace Warehouse.Domain
{
    public abstract class BaseEntity
    {
        private int _id;
        private bool _isActive;
        private DateTime _createdAt;

        public int Id
        {
            get => _id;
            set
            {
                if (value < 0)
                    throw new ArgumentException("ID cannot be negative");
                _id = value;
            }
        }

        public bool IsActive
        {
            get => _isActive;
            protected set => _isActive = value;
        }

        public DateTime CreatedAt
        {
            get => _createdAt;
            protected set => _createdAt = value;
        }

        protected BaseEntity()
        {
            _isActive = true;
            _createdAt = DateTime.Now;
        }

        public virtual void Deactivate()
        {
            _isActive = false;
            Console.WriteLine($"Entity {Id} has been deactivated at {DateTime.Now}");
        }

        public virtual void Activate()
        {
            _isActive = true;
            Console.WriteLine($"Entity {Id} has been activated at {DateTime.Now}");
        }

        public virtual void HardDelete()
        {
            Console.WriteLine($"Entity {Id} has been permanently deleted at {DateTime.Now}");
        }

        public abstract string GetEntityInfo();
    }
}