using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TMT
{
    public abstract class Entity : MonoBehaviour
    {
        public Transform Transform => transform;
        public Rigidbody2D Rigidbody => rigidbody;

        protected new Transform transform;
        protected new Rigidbody2D rigidbody;

        public virtual void Death()
        {
            Destroy(gameObject);
        }
        public virtual void Initialize()
        {
            transform = GetComponent<Transform>();
            rigidbody = GetComponent<Rigidbody2D>();
        }
        private void Awake() => Initialize();
    }
}
