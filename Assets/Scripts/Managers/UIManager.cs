using Game.GridSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TMP_InputField inputText;
        [SerializeField] private GridInitSettings gridInitSettings;
        private int cellSize = 3;
        void OnEnable()
        {
            EventManager.StartListening(GameEvents.UPDATE_SCORE_UI, OnScoreUpdate);
        }
        void OnDisable()
        {
            EventManager.StopListening(GameEvents.UPDATE_SCORE_UI, OnScoreUpdate);
        }
        private void OnScoreUpdate(object[] obj)
        {
            int score = (int)obj[0];
            scoreText.text = score.ToString();
        }
        public void OnSizeInput(string input)
        {
            int.TryParse(input, out cellSize);
            if (cellSize < gridInitSettings.GridMinSize)
            {
                cellSize = gridInitSettings.GridMinSize;
                inputText.text = cellSize.ToString();
            }
        }
        public void OnRebuild()
        {
            EventManager.TriggerEvent(GameEvents.REBUILD, new object[] { cellSize});
        }
    }
}