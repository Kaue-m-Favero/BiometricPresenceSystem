using BusinessLogicalLayer.Interfaces;
using Common;
using DataAccessLayer;
using Metadata;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicalLayer
{
    public class TeacherBLL : BaseValidator<Teacher>, ITeacherService
    {
        public override Response Validate(Teacher teacher)
        {
            AddError(teacher.Teachername.IsValidName());
            AddError(teacher.Cpf.IsValidCPF());
            AddError(teacher.Email.IsValidEmail());
            AddError(teacher.Phonenumber.IsValidPhoneNumber());
            return null;
        }
        public async Task<Response> Insert(Teacher teacher)
        {
            Response response = Validate(teacher);
            if (response.Success)
            {
                teacher.Cpf.RemoveMaskCPF();
                teacher.Phonenumber.RemoveMaskPhoneNumber();

                try
                {
                    using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                    {
                        await dataBase.Teachers.AddAsync(teacher);
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
        public async Task<Response> Update(Teacher teacher)
        {
            Response validationResponse = Validate(teacher);
            if (!validationResponse.Success)
            {
                return validationResponse;
            }
            try
            {
                using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                {
                    dataBase.Entry(teacher).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    await dataBase.SaveChangesAsync();
                }
                return ResponseMessage.CreateSuccessResponse();
            }
            catch (Exception ex)
            {
                return ResponseMessage.CreateErrorResponse(ex);
            }
        }
        public async Task<SingleResponse<Teacher>> GetByID(int id)
        {
            try
            {
                using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                {
                    Teacher teacher = await dataBase.Teachers.FirstOrDefaultAsync(p => p.ID == id);
                    if (teacher == null)
                    {
                        return ResponseMessage.CreateNotFoundData<Teacher>();
                    }
                    return ResponseMessage.CreateSingleSuccessResponse<Teacher>(teacher);
                }
            }
            catch (Exception ex)
            {
                SingleResponse<Teacher> teacher = (SingleResponse<Teacher>)ResponseMessage.CreateErrorResponse(ex);
                return teacher;
            }
        }
        public async Task<QueryResponse<Teacher>> GetAll()
        {
            try
            {
                using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                {
                    List<Teacher> teachers = await dataBase.Teachers.ToListAsync();
                    return ResponseMessage.CreateQuerySuccessResponse<Teacher>(teachers);
                }
            }
            catch (Exception ex)
            {
                QueryResponse<Teacher> teacher = (QueryResponse<Teacher>)ResponseMessage.CreateErrorResponse(ex);
                return teacher;
            }
        }
        public async Task<SingleResponse<Teacher>> ChangeStatus(int id)
        {
            try
            {
                using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                {
                    Teacher teacher = dataBase.Teachers.Find(id);
                    if (teacher.Active == false)
                    {
                        teacher.Active = true;
                    }
                    else
                    {
                        teacher.Active = false;
                    }
                    await dataBase.SaveChangesAsync();
                    return ResponseMessage.CreateSingleSuccessResponse<Teacher>(teacher);
                }
            }
            catch (Exception ex)
            {
                return ResponseMessage.CreateNotFoundData<Teacher>();
            }
        }
    }
}