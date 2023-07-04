using HNS.domain;
using HNS.Model;
using HNS.Player;
using UnityEngine;

namespace HNS.data
{
    public class SleepPlacesSceneRepository : ISleepPlacesRepository
    {
        [SerializeField] private SerializableDictionary<long, SleepPlaceController> sleepPlaces;

        TransformSnapshot ISleepPlacesRepository.Get(long id) => sleepPlaces[id].SleepPos;

        void ISleepPlacesRepository.SetOccupied(long id, bool occupied) => sleepPlaces[id].SetOccupied(occupied);
    }
}