namespace CIIP
{
    public interface IPersistentObject
    {
        void Delete();
        void Save();
        string Caption { get; set; }
    }
}