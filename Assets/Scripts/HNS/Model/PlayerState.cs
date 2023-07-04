namespace HNS.Model
{
    public struct SeekerState
    {
        public TransformSnapshot Transform;
        public Character Character;
        public SeekerBehavior Behavior;
    }
    
    public struct HiderState
    {
        public TransformSnapshot Transform;
        public Character Character;
        public HiderBehavior Behavior;
    }
    
    public enum SeekerBehavior
    {
        Pending,
        Seeking,
        Holding
    }
        
    public enum HiderBehavior
    {
        Pending,
        Hiding,
        Catched,
        Layed
    }
}