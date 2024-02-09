using Inmobiliaria.Models;
using Inmobiliaria.services;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiController : ControllerBase
    {
        public QczbbchrContext _context;

        public ApiController(QczbbchrContext context)
        {
            _context = context;
        }
    }
}
