
namespace BaseLibrary.Entities
{
    public class Country : BaseEntity
    {
        // one to many

        public List<City>? Cities { get; set; }
    }
}
