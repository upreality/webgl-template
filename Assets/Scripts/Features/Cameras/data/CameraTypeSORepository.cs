using System.Collections.Generic;
using System.Linq;
using Features.Cameras.domain;
using Features.Cameras.domain.model;
using UnityEngine;
using Utils.StringSelector;

namespace Features.Cameras.data
{
    [CreateAssetMenu(menuName = "Cameras/CameraTypeRepository", fileName = "SO Camera Type Repository", order = 0)]
    public class CameraTypeSORepository: ScriptableObject, ICameraTypeRepository, IStringSelectorSource
    {
        [SerializeField] private List<CamType> definedTypes = new();
        
        public CamType Get(string id) => definedTypes.Find(type => type.Id == id);
        public Dictionary<string, string> GetSelectorEntries() => definedTypes
            .ToDictionary(c => c.Id, c => c.Name);
    }
}