using MongoDB.Driver;
using NailArt.Services;
using NailArtApp.Models;

namespace NailArtApp.Services
{
    public class AppointmentService
    {
        private readonly IMongoCollection<Appointment> _appointmentsCollection;
        private readonly UserService _userService;


        public AppointmentService(IMongoDatabase database, UserService userService)
        {
            _appointmentsCollection = database.GetCollection<Appointment>("Appointments");
            _userService = userService;
        }

        public (bool Success, string Message) CreateAppointment(Appointment appointment)
        {
            var clientExists = _userService.GetUserByUsername(appointment.ClientUsername) != null;
            if (!clientExists)
            {
                return (false, "Client does not exist.");
            }

            var artistExists = _userService.GetUserByUsername(appointment.ArtistUsername) != null;
            if (!artistExists)
            {
                return (false, "Artist does not exist.");
            }

            var duplicateAppointment = _appointmentsCollection.Find(a =>
                a.Date == appointment.Date &&
                a.Time == appointment.Time &&
                a.ArtistUsername == appointment.ArtistUsername).FirstOrDefault();

            if (duplicateAppointment != null)
            {
                return (false, "An appointment already exists for this date, time, and artist.");
            }

            _appointmentsCollection.InsertOne(appointment);
            return (true, "Appointment created successfully.");
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
