
namespace BaseLibrary.Entities
{
    public class Branch : BaseEntity
    {
        // many to one

        public Department? Department { get; set; }

        public int DepartmentId { get; set; }

        // one to many

        public List<Employee>? Employees { get; set; }
    }
}
