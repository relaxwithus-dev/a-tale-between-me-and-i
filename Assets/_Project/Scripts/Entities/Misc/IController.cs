using UnityEngine;

namespace ATBMI.Entities
{
    public interface IController
    {
        public void ChangeState(EntitiesState state);
        public void LookAt(Vector2 direction);
        public void Flip();
    }
}