using System;
using UnityEngine;

namespace Infinite_story
{
    /// <summary>
    /// Define how much bonuses add to score when player get it 
    /// </summary>
    public class BonusAction : MonoBehaviour, IScoreChange<int>
    {

        [Tooltip("Positive value for good bonus, negative value for bad bonus"), Range(-50,50)] public int ChangeScoreTo = 0;

        public event Action<int> OnScoreChange = delegate (int s) { };
        public event Action<GameObject> SetCaller = delegate (GameObject obj) { };
        
        // здесь реализация интерфейса
        public void ScoreChange(int score)
        {
            OnScoreChange?.Invoke(score);
            SetCaller?.Invoke(transform.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                ScoreChange(ChangeScoreTo);
                //Destroy(gameObject);
                //gameObject.SetActive(false);
            }
        }

    }

}
