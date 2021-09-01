using System;

namespace Infinite_story
{
    public interface ITriggerColliderEnter<T>
    {
            event Action<T> OnTriggerColliderEnter;
    }
}
