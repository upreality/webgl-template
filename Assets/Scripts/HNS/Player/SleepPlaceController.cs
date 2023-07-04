using HNS.Model;
using UnityEngine;

namespace HNS.Player
{
    public class SleepPlaceController : MonoBehaviour
    {
        [SerializeField] private GameObject occupiedMark;
        [SerializeField] private Transform sleepPos;

        public TransformSnapshot SleepPos => sleepPos.GetSnapshot();

        public void SetOccupied(bool occupied) => occupiedMark.SetActive(occupied);
    }
}