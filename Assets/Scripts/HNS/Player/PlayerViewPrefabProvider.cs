using HNS.Model;
using UnityEngine;

namespace HNS.Player
{
    public class PlayerViewPrefabProvider: ScriptableObject
    {
        [SerializeField] private SerializableDictionary<Character, GameObject> characters;

        public GameObject GetCharacter(Character character) => characters[character];
    }
}