using HNS.Model;

namespace HNS.domain
{
    public interface ISleepPlacesRepository
    {
        TransformSnapshot Get(long id);
        void SetOccupied(long id, bool occupied);
    }
}