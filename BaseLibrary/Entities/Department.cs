
namespace BaseLibrary.Entities
{
    public class Department : BaseEntity
    {
        // many to one

        public GeneralDepartment? GeneralDepartment { get; set; }

        public int GeneralDepartmentId { get; set; }

        // one to many

        public List<Branch>? Branches { get; set; }
    }
}
