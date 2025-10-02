using AgendaUni.Models;
using AgendaUni.Repositories.Interfaces;
namespace AgendaUni.Repositories
{
    public class AbsenceRepository : IAbsenceRepository
    {
        private readonly AppDbContext _context;

        public AbsenceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Absence absence)
        {
            using var connection = _context.GetConnection();
            await connection.OpenAsync();

            var command = connection.CreateCommand(); 
            command.CommandText = @"
        INSERT INTO Absence (AbsenceDate, AbsenceReason, ClassId)
        VALUES ($absenceDate, $absenceReason, $classId);
        SELECT last_insert_rowid();";
            command.Parameters.AddWithValue("$absenceDate", absence.AbsenceDate);
            command.Parameters.AddWithValue("$absenceReason", absence.AbsenceReason);
            command.Parameters.AddWithValue("$classId", absence.ClassId);

            await command.ExecuteScalarAsync();
        }

        public async Task<IEnumerable<Absence>> GetAllAsync()
        {
            var absences = new List<Absence>();
            using var connection = _context.GetConnection();
            await connection.OpenAsync();
            var command = connection.CreateCommand();
            command.CommandText = @"SELECT Id, AbsenceDate, AbsenceReason, ClassId FROM Absence;";
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                absences.Add(new Absence
                {
                    Id = reader.GetInt32(0),
                    AbsenceDate = reader.GetDateTime(1),
                    AbsenceReason = reader.GetString(2),
                    ClassId = reader.GetInt32(3)
                });
            }
            return absences;
        }

        public async Task<Absence> GetByIdAsync(int id)
        {
            using var connection = _context.GetConnection();
            await connection.OpenAsync();
            var command = connection.CreateCommand();
            command.CommandText = @"SELECT Id, AbsenceDate, AbsenceReason, ClassId FROM Absence WHERE Id = $id;";
            command.Parameters.AddWithValue("$id", id);
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Absence
                {
                    Id = reader.GetInt32(0),
                    AbsenceDate = reader.GetDateTime(1),
                    AbsenceReason = reader.GetString(2),
                    ClassId = reader.GetInt32(3)
                };
            }
            return null;
        }

        public async Task UpdateAsync(Absence absence)
        {
            using var connection = _context.GetConnection();
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"
                UPDATE Absence SET
                    AbsenceDate = $absenceDate,
                    AbsenceReason = $absenceReason,
                    ClassId = $classId
                WHERE Id = $id;";
            command.Parameters.AddWithValue("$absenceDate", absence.AbsenceDate);
            command.Parameters.AddWithValue("$absenceReason", absence.AbsenceReason);
            command.Parameters.AddWithValue("$classId", absence.ClassId);
            command.Parameters.AddWithValue("$id", absence.Id);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _context.GetConnection();
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Absence WHERE Id = $id;";
            command.Parameters.AddWithValue("$id", id);
            await command.ExecuteNonQueryAsync();
        }
    }
}
