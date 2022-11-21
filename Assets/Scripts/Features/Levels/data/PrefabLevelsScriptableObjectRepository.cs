using System.Collections.Generic;
using System.Linq;
using Features.Levels.domain.model;
using Features.Levels.domain.repositories;
using UnityEngine;

namespace Features.Levels.data
{
    [CreateAssetMenu(
        menuName = "Levels/PrefabLevelsScriptableObjectRepository",
        fileName = "PrefabLevelsScriptableObjectRepository")
    ]
    public class PrefabLevelsScriptableObjectRepository : ScriptableObject, ILevelsRepository,
        ILevelSceneObjectRepository
    {
        [SerializeField] private List<GameObject> scenePrefabs = new();

        public List<Level> GetLevels() => scenePrefabs.Select((_, i) => GetLevel(i)).ToList();

        public Level GetLevel(int levelId) => new(levelId, levelId);

        public GameObject GetLevelScene(int levelId) => scenePrefabs[levelId];
    }
}