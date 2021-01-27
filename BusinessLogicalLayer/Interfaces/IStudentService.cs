using Common;
using Metadata;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicalLayer.Interfaces
{
    public interface IStudentService : IMetadataCRUD<Student>
    {
        Task<SingleResponse<Student>> IsUniqueRegister(string register);
        Task<SingleResponse<Student>> ChangeStatus(int id);
    }
}