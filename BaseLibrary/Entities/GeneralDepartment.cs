
namespace BaseLibrary.Entities
{
    public class GeneralDepartment : BaseEntity
    {
        // one to many

        public List<Department>? Departments { get; set; }
    }
}
