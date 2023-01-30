using Features.Cameras.domain.model;

namespace Features.Cameras.domain
{
    public interface ICameraTypeRepository
    {
        public CamType Get(string id);
    }
}