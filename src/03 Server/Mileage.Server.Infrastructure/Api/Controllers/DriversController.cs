using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Mileage.Server.Contracts.Commands;
using Mileage.Server.Infrastructure.Api.Filters;

namespace Mileage.Server.Infrastructure.Api.Controllers
{
    [RoutePrefix("Drivers")]
    public class DriversController : BaseController
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DriversController"/> class.
        /// </summary>
        /// <param name="commandExecutor">The command executor.</param>
        public DriversController(ICommandExecutor commandExecutor)
            : base(commandExecutor)
        {
        }
        #endregion

        #region Methods
        //[HttpPost]
        //[MileageAuthentication]
        //public async Task<HttpResponseMessage> GetDrivers(string searchText = null, string driversLicense = null, bool? personCarriageLicense = null)
        //{
        //    return 
        //}
        #endregion
    }
}