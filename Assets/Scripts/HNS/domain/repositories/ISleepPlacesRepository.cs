using HNS.domain.model;

namespace HNS.domain.repositories
{
    public interface ISleepPlacesRepository
    {
        TransformSnapshot Get(long id);
        void SetOccupied(long id, bool occupied);
    }
}