using System;

namespace Features.Cameras.domain.model
{
    [Serializable]
    public class CamType
    {
        public string Id;
        public string Name;

        public CamType(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}