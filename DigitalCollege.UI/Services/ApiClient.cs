using System.Net.Http.Headers;
using System.Net.Http.Json;
using DigitalCollege.UI.Models;

namespace DigitalCollege.UI.Services
{
    public class ApiClient
    {
        private readonly HttpClient _http;

        public ApiClient(HttpClient http) => _http = http;

        public void SetToken(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                _http.DefaultRequestHeaders.Authorization = null;
            }
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", loginDto);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AuthResponseDto>();
            }
            return null;
        }

        public async Task<List<TeacherModel>> GetTeachersAsync(string s = null, int? disciplineId = null, string o = null)
        {
            string query = $"api/management/teachers?s={(s != null ? Uri.EscapeDataString(s) : "")}&disciplineId={disciplineId}&o={(o != null ? Uri.EscapeDataString(o) : "")}";
            return await _http.GetFromJsonAsync<List<TeacherModel>>(query);
        }

        public async Task AddTeacherAsync(TeacherModel m)
        {
            var response = await _http.PostAsJsonAsync("api/management/teacher", m);
            if (!response.IsSuccessStatusCode) throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task UpdateTeacherAsync(TeacherModel m)
        {
            var response = await _http.PutAsJsonAsync("api/management/teacher", m);
            if (!response.IsSuccessStatusCode) throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task DeleteTeacherAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/management/teacher/{id}");
            if (!response.IsSuccessStatusCode) throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task<List<StudentModel>> GetStudentsAsync(string s = null, string g = null, string o = null)
        {
            string query = $"api/management/students?s={(s != null ? Uri.EscapeDataString(s) : "")}&g={(g != null ? Uri.EscapeDataString(g) : "")}&o={(o != null ? Uri.EscapeDataString(o) : "")}";
            return await _http.GetFromJsonAsync<List<StudentModel>>(query);
        }

        public async Task AddStudentAsync(StudentModel m)
        {
            var response = await _http.PostAsJsonAsync("api/management/student", m);
            if (!response.IsSuccessStatusCode) throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task UpdateStudentAsync(StudentModel m)
        {
            var response = await _http.PutAsJsonAsync("api/management/student", m);
            if (!response.IsSuccessStatusCode) throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task DeleteStudentAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/management/student/{id}");
            if (!response.IsSuccessStatusCode) throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task<List<DisciplineModel>> GetDisciplinesAsync(string s = null, string o = null, int? teacherId = null)
        {
            string query = $"api/management/disciplines?s={(s != null ? Uri.EscapeDataString(s) : "")}&o={(o != null ? Uri.EscapeDataString(o) : "")}&teacherId={teacherId}";
            return await _http.GetFromJsonAsync<List<DisciplineModel>>(query);
        }

        public async Task AddDisciplineAsync(DisciplineModel m) =>
            await _http.PostAsJsonAsync("api/management/discipline", m);

        public async Task UpdateDisciplineAsync(DisciplineModel m) =>
            await _http.PutAsJsonAsync("api/management/discipline", m);

        public async Task DeleteDisciplineAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/management/discipline/{id}");
            if (!response.IsSuccessStatusCode) throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task<List<GroupModel>> GetGroupsAsync(string s = null)
        {
            string query = $"api/management/groups?s={(s != null ? Uri.EscapeDataString(s) : "")}";
            return await _http.GetFromJsonAsync<List<GroupModel>>(query);
        }

        public async Task AddGroupAsync(GroupModel group) =>
            await _http.PostAsJsonAsync("api/management/group", group);

        public async Task UpdateGroupAsync(GroupModel group) =>
            await _http.PutAsJsonAsync("api/management/group", group);

        public async Task DeleteGroupAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/management/group/{id}");
            if (!response.IsSuccessStatusCode) throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task AssignGradeAsync(CreateGradeModel grade)
        {
            var response = await _http.PostAsJsonAsync("api/academic/grade", grade);
            if (!response.IsSuccessStatusCode) throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task<List<GradeModel>> GetStudentGradesAsync(int studentId) =>
            await _http.GetFromJsonAsync<List<GradeModel>>($"api/academic/student/{studentId}/grades");

        public async Task<List<TestAccountModel>> GetTestAccountsAsync()
        {
            try { return await _http.GetFromJsonAsync<List<TestAccountModel>>("api/auth/test-accounts"); }
            catch { return new List<TestAccountModel>(); }
        }

        public async Task UpdateGradeAsync(CreateGradeModel grade)
        {
            var response = await _http.PutAsJsonAsync("api/academic/grade", grade);
            if (!response.IsSuccessStatusCode) throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task DeleteGradeAsync(int gradeId, int teacherId)
        {
            var response = await _http.DeleteAsync($"api/academic/grade/{gradeId}/{teacherId}");
            if (!response.IsSuccessStatusCode) throw new Exception(await response.Content.ReadAsStringAsync());
        }
    }
}
