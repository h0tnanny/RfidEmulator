using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using RfidEmulator.Repository;

namespace RfidEmulator.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class BaseController<T>(ILogger<T> logger, RepositoryContext context, IMapper mapper) : 
    ControllerBase where T : class;