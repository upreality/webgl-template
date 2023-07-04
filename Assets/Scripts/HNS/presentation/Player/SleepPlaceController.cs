using HNS.domain.Model;
using UnityEngine;

namespace HNS.presentation.Player
{
    public class SleepPlaceController : MonoBehaviour
    {
        [SerializeField] private GameObject occupiedMark;
        [SerializeField] private Transform sleepPos;

        public TransformSnapshot SleepPos => sleepPos.GetSnapshot();

        public void SetOccupied(bool occupied) => occupiedMark.SetActive(occupied);
    }
}