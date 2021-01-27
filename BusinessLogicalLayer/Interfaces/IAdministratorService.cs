using Common;
using Metadata;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicalLayer.Interfaces
{
    interface IAdministratorService: IMetadataCRUD<Administrator>
    {
        Task<SingleResponse<Administrator>> GetAdmByEmail(string email, string passcode);
        Task<SingleResponse<Administrator>> ChangeStatus(int id);


    }
}
