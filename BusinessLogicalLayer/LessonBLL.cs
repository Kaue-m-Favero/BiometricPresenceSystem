using BusinessLogicalLayer.Interfaces;
using Common;
using DataAccessLayer;
using Metadata;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace BusinessLogicalLayer
{
    public class LessonBLL : BaseValidator<Lesson>, ILessonService
    {
        private readonly ITeacherService _teacherService;
        private readonly IClassService _classService;

        public LessonBLL(ITeacherService teacherService, IClassService classService)
        {
            _teacherService = teacherService;
            _classService = classService;
        }


        public override Response Validate(Lesson lesson)
        {
            return base.Validate(lesson);
        }
        public async Task<Response> Insert(Lesson lesson)
        {
            List<HorarioAula> horariosAula = new List<HorarioAula>();

            QueryResponse<Teacher> responseTeacher = await _teacherService.GetAll();
            QueryResponse<Class> responseClasses = await _classService.GetAll();

            List<Teacher> temp = responseTeacher.Data;
            List<Teacher> professores = new List<Teacher>(temp.Count);
            List<Class> turmas = responseClasses.Data;

            for (int i = 1; i <= 5; i++)
            {
                temp = professores.OrderBy(item => Guid.NewGuid()).ToList();
                HorarioAula horarioAula = new HorarioAula((DayOfWeek)i);
                for (int x = 0; x < 5; x++)
                {
                    horarioAula.AdicionarAula(new Lesson(temp[x], turmas[x]));

                }
                horariosAula.Add(horarioAula);
            }



        }
        public async Task<QueryResponse<Lesson>> GetAll()
        {
            try
            {
                using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                {
                    List<Lesson> lessons = await dataBase.Lessons.ToListAsync();
                    return ResponseMessage.CreateQuerySuccessResponse<Lesson>(lessons);
                }
            }
            catch (Exception ex)
            {
                QueryResponse<Lesson> lesson = (QueryResponse<Lesson>)ResponseMessage.CreateErrorResponse(ex);
                return lesson;
            }

        }
        public async Task<SingleResponse<Lesson>> GetByID(int id)
        {
            try
            {
                using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                {
                    Lesson lesson = await dataBase.Lessons.FirstOrDefaultAsync(p => p.ID == id);
                    if (lesson == null)
                    {
                        return ResponseMessage.CreateNotFoundData<Lesson>();
                    }
                    return ResponseMessage.CreateSingleSuccessResponse<Lesson>(lesson);
                }
            }
            catch (Exception ex)
            {
                SingleResponse<Lesson> lesson = (SingleResponse<Lesson>)ResponseMessage.CreateErrorResponse(ex);
                return lesson;
            }
        }
        public async Task<Response> Update(Lesson lesson)
        {

            Response validationResponse = Validate(lesson);
            if (!validationResponse.Success)
            {
                return validationResponse;
            }
            try
            {
                using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                {
                    dataBase.Entry(lesson).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    await dataBase.SaveChangesAsync();
                }
                return ResponseMessage.CreateSuccessResponse();
            }
            catch (Exception ex)
            {
                return ResponseMessage.CreateErrorResponse(ex);
            }

        }
        private string DateValidate(Lesson date)
        {
            if (date.Lessondate.DayOfWeek == DayOfWeek.Sunday || date.Lessondate.DayOfWeek == DayOfWeek.Saturday)
            {
                return "Dia da semana inválido";
            }
            else if (date.Lessondate < DateTime.Now)
            {
                return "Preencha com uma data valida";
            }
            return "";
        }

    }

    public class HorarioAula
    {
        private readonly List<Lesson> _aulas = new List<Lesson>();
        public DayOfWeek DiaDaSemana { get; private set; }
        public IReadOnlyList<Lesson> Aulas => _aulas.AsReadOnly();

        public HorarioAula(DayOfWeek diaDaSemana)
        {
            DiaDaSemana = diaDaSemana;
            _aulas = new List<Lesson>();
        }
        public void AdicionarAula(Lesson aula) => _aulas.Add(aula);
    }
}
