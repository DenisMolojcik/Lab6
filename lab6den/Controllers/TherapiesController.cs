using lab6den.Data;
using lab6den.Models;
using lab6den.ViewsModels;
using Microsoft.AspNetCore.Mvc;

namespace lab6den.Controllers
{
    [Route("api/{therapies}")]
    public class TherapiesController : ControllerBase
    {
        private readonly Context _context;

        public TherapiesController(Context context)
        {
            _context = context;
        }
        [HttpGet]
        [Produces("application/json")]
        public IActionResult GetALL()
        {
            var therapies = _context.Therapies.ToList();
            var therapiesVM = ToView(therapies);
            return Ok(therapiesVM);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Therapy therapy)
        {
            _context.Therapies.Add(therapy);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPut]
        public IActionResult Put([FromBody] Therapy therapy)
        {
            _context.Update(therapy);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var therapy = _context.Therapies.FirstOrDefault(x => x.Id == id);
            _context.Therapies.Remove(therapy);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpGet("diseases")]
        [Produces("application/json")]
        public IActionResult GetDiseases()
        {
            var diseases = _context.Diseases.ToList();
            return Ok(diseases);
        }

        [HttpGet("medicianes")]
        [Produces("application/json")]
        public IActionResult GetMedicianes()
        {
            var medicianes = _context.Medicianes.ToList();
            return Ok(medicianes);
        }

        [HttpGet("patients")]
        [Produces("application/json")]
        public IActionResult GetPatients()
        {
            var patients = _context.Patients.ToList();
            return Ok(patients);
        }

        [HttpGet("doctors")]
        [Produces("application/json")]
        public IActionResult GetDoctors()
        {
            var doctors = _context.Doctors.ToList();
            return Ok(doctors);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Therapy therapies = _context.Therapies.FirstOrDefault(x => x.Id == id);
            return new ObjectResult(therapies);
        }

        public IEnumerable<TherapyView> ToView(IEnumerable<Therapy> therapies)
        {
            IEnumerable<TherapyView> vm = from t in therapies
                                          join d in _context.Diseases
                                          on t.DiseaseId equals d.DiseaseId
                                          join m in _context.Medicianes
                                          on t.MedicianId equals m.MedicianId
                                          join p in _context.Patients
                                          on t.PatientId equals p.PatientId
                                          join doc in _context.Doctors
                                          on t.DoctorId equals doc.DoctorId
                                          select new TherapyView
                                          {
                                              Id = t.Id,
                                              DiseaseName = d.Name,
                                              MedicianName = m.Name,
                                              DoctorName = doc.Name,
                                              PatientName = p.Name,
                                              Date = t.Date
                                          };
            return vm;

        }
    }
}
