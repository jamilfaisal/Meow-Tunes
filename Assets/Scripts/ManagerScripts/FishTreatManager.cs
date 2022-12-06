using System;
using UnityEngine;

public sealed class FishTreatManager : MonoBehaviour
{
        public static FishTreatManager Current;
        public event Action FishUnhideEvent;
        private void Awake()
        {
                Current = this;
        }


        public void OnFishUnhideEvent()
        {
                FishUnhideEvent?.Invoke();
        }
}