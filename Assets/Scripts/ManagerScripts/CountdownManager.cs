using TMPro;
using UnityEngine;

    public class CountdownManager: MonoBehaviour
    {
        public float countdown = 5f;
        public TMP_Text countdownText;
        public GameObject countdownUI;
        
        private void Update()
        {
            if (countdown <= 0)
            {
                countdownUI.SetActive(false);
            }
            else
            {
                countdown -= Time.deltaTime;
                countdownText.text = (countdown).ToString("0");
            }
        }

        public void SetCountdown(float time)
        {
            countdown = time;
            countdownUI.SetActive(true);
        }
    }
