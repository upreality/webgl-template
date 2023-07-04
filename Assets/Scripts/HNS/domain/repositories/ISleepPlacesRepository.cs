using HNS.domain.Model;

namespace HNS.domain.repositories
{
    public interface ISleepPlacesRepository
    {
        TransformSnapshot Get(long id);
        void SetOccupied(long id, bool occupied);
    }
}