
namespace BaseLibrary.Entities
{
    public class Town : BaseEntity
    {

        public List<Employee>? Employees { get; set; }

        // many to one

        public City? City { get; set; }

        public int CityId { get; set; }
    }
}
