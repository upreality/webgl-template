using System.Collections.Generic;
using UnityEngine;

namespace Utils.Enviroment.Birds
{
    public class BirdAIPointProvider : MonoBehaviour
    {
        private List<Transform> targets = new();
        [SerializeField] private Transform player;
        [SerializeField, Range(0f, 1f)] private float playerPointChance = 0.05f;

        private void Awake()
        {
            foreach (Transform child in transform) targets.Add(child);
        }

        public Transform GetNextTarget()
        {
            var playerChance = Random.Range(0f, 1f);
            if (playerChance <= playerPointChance)
                return player;

            return targets[Random.Range(0, targets.Count)];
        }
    }
}