using System;

namespace Payroll_Manager.Entity.Abstract
{
    public abstract class BaseEntity: IBaseEntity
    {
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
