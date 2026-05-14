using System.Net.Http.Json;
using DigitalCollege.UI.Models;

namespace DigitalCollege.UI.Services
{
    public class ApiClient
    {
        private readonly HttpClient _http;
        public ApiClient(HttpClient http) => _http = http;

        public async Task UpdateTeacherAsync(TeacherModel m) => await _http.PutAsJsonAsync("api/management/teacher", m);
        public async Task DeleteTeacherAsync(int id) => await _http.DeleteAsync($"api/management/teacher/{id}");
        
        public async Task UpdateStudentAsync(StudentModel m) => await _http.PutAsJsonAsync("api/management/student", m);
        public async Task DeleteStudentAsync(int id) => await _http.DeleteAsync($"api/management/student/{id}");

        public async Task UpdateDisciplineAsync(DisciplineModel m) => await _http.PutAsJsonAsync("api/management/discipline", m);
        public async Task DeleteDisciplineAsync(int id) => await _http.DeleteAsync($"api/management/discipline/{id}");

        public async Task<List<TeacherModel>> GetTeachersAsync(string s = null, string o = null) => 
            await _http.GetFromJsonAsync<List<TeacherModel>>($"api/management/teachers?searchTerm={s}&sortBy={o}");

        public async Task<List<StudentModel>> GetStudentsAsync(string s = null, int? g = null, string o = null) => 
            await _http.GetFromJsonAsync<List<StudentModel>>($"api/management/students?searchTerm={s}&groupId={g}&sortBy={o}");

        public async Task<List<DisciplineModel>> GetDisciplinesAsync(string s = null, string o = null) => 
            await _http.GetFromJsonAsync<List<DisciplineModel>>($"api/management/disciplines?searchTerm={s}&sortBy={o}");

        public async Task AddTeacherAsync(TeacherModel m) => await _http.PostAsJsonAsync("api/management/teacher", m);
        public async Task AddStudentAsync(StudentModel m) => await _http.PostAsJsonAsync("api/management/student", m);
        public async Task AddDisciplineAsync(DisciplineModel m) => await _http.PostAsJsonAsync("api/management/discipline", m);

        public async Task AssignGradeAsync(CreateGradeModel grade) => 
            await _http.PostAsJsonAsync("api/academic/grade", grade);

        public async Task<List<GradeModel>> GetStudentGradesAsync(int studentId) => 
            await _http.GetFromJsonAsync<List<GradeModel>>($"api/academic/student/{studentId}/grades");
    }
}