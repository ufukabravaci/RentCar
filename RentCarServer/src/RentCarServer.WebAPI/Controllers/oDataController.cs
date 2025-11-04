using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace RentCarServer.WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
[EnableQuery] //OData sorgularını aktif eder
public class oDataController : ControllerBase
{
    public static IEdmModel GetEdmModel() //Edm => entity data model. Verileri işleyebilmek için Odata ister.
    {
        ODataConventionModelBuilder builder = new();
        builder.EnableLowerCamelCase();
        //builder.EntitySet<UserResponse>("users");
        return builder.GetEdmModel();
    }
}
