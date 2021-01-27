using Common;
using Metadata;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicalLayer.Interfaces
{
    public interface ITeacherService : IMetadataCRUD<Teacher>
    {
        Task<SingleResponse<Teacher>> ChangeStatus(int id);

    }
}
