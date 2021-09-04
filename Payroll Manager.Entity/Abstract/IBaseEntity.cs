using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll_Manager.Entity.Abstract
{
    interface IBaseEntity
    {
        DateTime? CreatedDate { set; get; }
        string CreatedBy { set; get; }
        DateTime? UpdatedDate { set; get; }
        string UpdatedBy { set; get; }
    }
}
