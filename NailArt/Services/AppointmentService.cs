using MongoDB.Driver;
using NailArtApp.Models;

namespace NailArtApp.Services
{
    public class AppointmentService
    {
        private readonly IMongoCollection<Appointment> _appointmentsCollection;

        public AppointmentService(IMongoDatabase database)
        {
            _appointmentsCollection = database.GetCollection<Appointment>("Appointments");
        }
    }

}
