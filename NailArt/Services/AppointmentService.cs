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

        public void CreateAppointment(Appointment appointment)
        {
            _appointmentsCollection.InsertOne(appointment);
        }

        public List<Appointment> GetAppointmentsByUsername(string username, bool isArtist)
        {
            if (isArtist)
            {
                return _appointmentsCollection.Find(appointment => appointment.ArtistUsername == username).ToList();
            }
            else
            {
                return _appointmentsCollection.Find(appointment => appointment.ClientUsername == username).ToList();
            }
        }

    }

}
