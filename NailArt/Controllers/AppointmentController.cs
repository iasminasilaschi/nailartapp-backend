using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NailArtApp.Models;
using NailArtApp.Services;
using System.Security.Claims;

namespace NailArtApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentService _appointmentService;

        // Inject the AppointmentService through the constructor
        public AppointmentController(AppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        // POST api/appointment/create
        [HttpPost("create")]
        public IActionResult CreateAppointment([FromBody] Appointment appointment)
        {
            if (appointment == null)
            {
                return BadRequest("Appointment is null.");
            }

            _appointmentService.CreateAppointment(appointment);
            return Ok("Appointment created successfully.");
        }

        [HttpGet("getAppointments")]
        [Authorize] // Ensure the user is authenticated
        public IActionResult GetAppointments()
        {
            // Get the user ID from the JWT token
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(userRole))
            {
                return BadRequest("Invalid user data.");
            }

            // Check if the user is an artist or a client
            bool isArtist = userRole.Equals("admin", StringComparison.OrdinalIgnoreCase);

            // Retrieve the appointments for the user
            var appointments = _appointmentService.GetAppointmentsByUsername(username, isArtist);

            return Ok(appointments);
        }
    }
}
