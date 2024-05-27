
namespace BaseLibrary.Entities
{
    public class City : BaseEntity
    {
        // many to one

        public Country? Country { get; set; }

        public int CountyId { get; set; }

        public List<Town>? Towns { get; set; }
    }
}
