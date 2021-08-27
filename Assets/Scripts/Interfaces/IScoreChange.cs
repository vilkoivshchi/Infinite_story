
using System;

namespace Infinite_story
{
    public interface IScoreChange<T>
    {
        //void ScoreChange(int score);
        event Action<T> OnScoreChange;
    }

}
