using System;

namespace Infinite_story
{
    public interface IScoreChange<T>
    {
        event Action<T> OnScoreChange;
    }

}
