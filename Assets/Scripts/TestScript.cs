using System;

namespace Infinite_story
{
    public class TestScript
    {
        public int Score;

        public void Listen(ITestInterface<int> i)
        {
            i.MyTestEvent += SetScore;
        }

        public void SetScore(int newScore)
        {
            Score = newScore;
            UnityEngine.Debug.Log(Score);
            
        }
    }
}