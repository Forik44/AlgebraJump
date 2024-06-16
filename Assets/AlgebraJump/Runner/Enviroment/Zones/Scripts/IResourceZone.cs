namespace AlgebraJump.Runner
{
    public abstract class IResourceZone : IZone
    {
        public abstract string ZoneID { get; }
        public abstract void RestartZone();
        public abstract void SetActive(bool active);
    }
}