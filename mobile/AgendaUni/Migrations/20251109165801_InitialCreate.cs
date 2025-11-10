using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgendaUni.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Tabela Classes
            migrationBuilder.Sql(@"
        CREATE TABLE IF NOT EXISTS Classes (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            ClassName TEXT NOT NULL,
            MaximumAbsences INTEGER NOT NULL
        )");

            // Tabela ClassSchedules
            migrationBuilder.Sql(@"
        CREATE TABLE IF NOT EXISTS ClassSchedules (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            DayOfWeek INTEGER NOT NULL,
            ClassTime TEXT NOT NULL,
            ClassId INTEGER NOT NULL,
            NotificationId INTEGER NULL,
            FOREIGN KEY (ClassId) REFERENCES Classes(Id) ON DELETE CASCADE
        )");

            // Adiciona o índice para ClassSchedules
            migrationBuilder.Sql(@"
        CREATE INDEX IF NOT EXISTS IX_ClassSchedules_ClassId ON ClassSchedules (ClassId)");

            // Tabela Absences
            migrationBuilder.Sql(@"
        CREATE TABLE IF NOT EXISTS Absences (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            AbsenceDate TEXT NOT NULL,
            AbsenceReason TEXT NOT NULL,
            ClassId INTEGER NOT NULL,
            FOREIGN KEY (ClassId) REFERENCES Classes(Id) ON DELETE CASCADE
        )");

            // Adiciona o índice para Absences
            migrationBuilder.Sql(@"
        CREATE INDEX IF NOT EXISTS IX_Absences_ClassId ON Absences (ClassId)");

            // Tabela Events
            migrationBuilder.Sql(@"
        CREATE TABLE IF NOT EXISTS Events (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            EventDate TEXT NOT NULL,
            Description TEXT NOT NULL,
            ClassId INTEGER NOT NULL,
            FOREIGN KEY (ClassId) REFERENCES Classes(Id) ON DELETE CASCADE
        )");

            // Adiciona o índice para Events
            migrationBuilder.Sql(@"
        CREATE INDEX IF NOT EXISTS IX_Events_ClassId ON Events (ClassId)");

            // Tabela EventNotifications
            migrationBuilder.Sql(@"
        CREATE TABLE IF NOT EXISTS EventNotifications (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            NotificationId INTEGER NOT NULL,
            EventId INTEGER NOT NULL,
            FOREIGN KEY (EventId) REFERENCES Events(Id) ON DELETE CASCADE
        )");

            // Adiciona o índice para EventNotifications
            migrationBuilder.Sql(@"
        CREATE INDEX IF NOT EXISTS IX_EventNotifications_EventId ON EventNotifications (EventId)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
