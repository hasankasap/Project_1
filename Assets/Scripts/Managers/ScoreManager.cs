using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ScoreManager : MonoBehaviour
    {
        private int score = 0;
        private void OnEnable()
        {
            EventManager.StartListening(GameEvents.INCREASE_SCORE, OnScoreIncrease);
            EventManager.StartListening(GameEvents.REBUILD, ResetScore);
            EventManager.StartListening(GameEvents.GENERATE_GRID, ResetScore);
        }
        private void OnDisable()
        {
            EventManager.StopListening(GameEvents.INCREASE_SCORE, OnScoreIncrease);
            EventManager.StopListening(GameEvents.REBUILD, ResetScore);
            EventManager.StopListening(GameEvents.GENERATE_GRID, ResetScore);
        }
        private void Start()
        {
            EventManager.TriggerEvent(GameEvents.UPDATE_SCORE_UI, new object[] { score });
        }
        private void OnScoreIncrease(object[] obj)
        {
            score++;
            EventManager.TriggerEvent(GameEvents.UPDATE_SCORE_UI, new object[] { score });
        }
        private void ResetScore(object[] obj)
        {
            score = 0;
            EventManager.TriggerEvent(GameEvents.UPDATE_SCORE_UI, new object[] { score });
        }
    }
}