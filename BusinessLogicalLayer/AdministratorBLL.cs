using BusinessLogicalLayer.Interfaces;
using Common;
using DataAccessLayer;
using Metadata;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace BusinessLogicalLayer
{
    public class AdministratorBLL : BaseValidator<Administrator>, IAdministratorService
    {
        public override Response Validate(Administrator administrator)
        {
            AddError(administrator.Admname.IsValidName());
            AddError(administrator.Cpf.IsValidCPF());
            AddError(administrator.Email.IsValidEmail());
            AddError(administrator.Phonenumber.IsValidPhoneNumber());
            AddError(administrator.Passcode.IsValidPasscode());
            return base.Validate(administrator);
        }
        public async Task<Response> Insert(Administrator administrator)
        {
            Response response = Validate(administrator);
            if (response.Success)
            {
                administrator.Cpf.RemoveMaskCPF();
                administrator.Phonenumber.RemoveMaskPhoneNumber();
                administrator.Passcode = EncryptPassword(administrator.Passcode);
                try
                {
                    using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                    {
                        await dataBase.Administrators.AddAsync(administrator);
                        await dataBase.SaveChangesAsync();
                        ResponseMessage.CreateSuccessResponse();
                    }
                }
                catch (Exception ex)
                {
                    return ResponseMessage.CreateErrorResponse(ex);
                }
            }
            return response;
        }
        public async Task<Response> Update(Administrator administrator)
        {

            Response validationResponse = Validate(administrator);
            if (!validationResponse.Success)
            {
                return validationResponse;
            }
            try
            {
                using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                {
                    dataBase.Entry(administrator).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    await dataBase.SaveChangesAsync();
                }
                return ResponseMessage.CreateSuccessResponse();
            }
            catch (Exception ex)
            {
                return ResponseMessage.CreateErrorResponse(ex);
            }

        }
        public async Task<SingleResponse<Administrator>> ChangeStatus(int id)
        {
            try
            {
                using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                {
                    Administrator administrator = dataBase.Administrators.Find(id);
                    if (administrator.Active == false)
                    {
                        administrator.Active = true;
                    }
                    else 
                    {
                        administrator.Active = false;
                    }
                    await dataBase.SaveChangesAsync();
                    return ResponseMessage.CreateSingleSuccessResponse<Administrator>(administrator);
                }
            }
            catch (Exception ex)
            {
                return ResponseMessage.CreateNotFoundData<Administrator>();
            }

        }
        public async Task<SingleResponse<Administrator>> GetByID(int id)
        {
            try
            {
                using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                {
                    Administrator administrator = await dataBase.Administrators.FirstOrDefaultAsync(p => p.Id == id);
                    if (administrator == null)
                    {
                        return ResponseMessage.CreateNotFoundData<Administrator>();
                    }
                    return ResponseMessage.CreateSingleSuccessResponse<Administrator>(administrator);
                }
            }
            catch (Exception ex)
            {
                SingleResponse<Administrator> administrator = (SingleResponse<Administrator>)ResponseMessage.CreateErrorResponse(ex);
                return administrator;
            }

        }
        public async Task<QueryResponse<Administrator>> GetAll()
        {
            try
            {
                using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                {
                    List<Administrator> administrators = await dataBase.Administrators.ToListAsync();
                    return ResponseMessage.CreateQuerySuccessResponse<Administrator>(administrators);
                }
            }
            catch (Exception ex)
            {
                QueryResponse<Administrator> administrator = (QueryResponse<Administrator>)ResponseMessage.CreateErrorResponse(ex);
                return administrator;
            }

        }
        public string EncryptPassword(string passcode)
        {
            var encodedValue = Encoding.UTF8.GetBytes(passcode);
            var encryptedPassword = MD5.Create().ComputeHash(encodedValue);

            var sb = new StringBuilder();
            foreach (var caracter in encryptedPassword)
            {
                sb.Append(caracter.ToString("X2"));
            }
            return sb.ToString();
        }
        public async Task<SingleResponse<Administrator>> GetAdmByEmail(string email, string passcode)
        {
            try
            {
                passcode = EncryptPassword(passcode);

                using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                {
                    Administrator administrator = await dataBase.Administrators.FirstOrDefaultAsync(c => c.Email == email && c.Passcode == passcode);
                    if (administrator == null)
                    {
                        return ResponseMessage.CreateNotFoundData<Administrator>();
                    }
                    //Precisa de cookies
                    //SystemParameters.EmployeeName = response.Data.Name;
                    //SystemParameters.EmployeeADM = response.Data.IsAdm;
                    return ResponseMessage.CreateSingleSuccessResponse<Administrator>(administrator);
                }
            }
            catch (Exception ex)
            {
                SingleResponse<Administrator> administrator = (SingleResponse<Administrator>)ResponseMessage.CreateErrorResponse(ex);
                return administrator;
            }
        }

    }
}