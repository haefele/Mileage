using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Mileage.Server.Contracts.Commands;
using Mileage.Server.Infrastructure.Api.Filters;
using Mileage.Server.Infrastructure.Commands.Drivers;
using Mileage.Server.Infrastructure.Extensions;
using Mileage.Shared.Extensions;
using Mileage.Shared.Results;

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
        [HttpGet]
        [MileageAuthentication]
        public async Task<HttpResponseMessage> GetDrivers(string searchText = null, string driversLicense = null, bool? personCarriageLicense = null)
        {
            Result<SearchDriversResult> result = await this.CommandExecutor
                .Execute(new SearchDriversCommand(
                    searchText, 
                    driversLicense, 
                    personCarriageLicense, 
                    this.Paging.Skip, 
                    this.Paging.Take))
                .WithCurrentCulture();

            if (result.IsError)
                return this.Request.GetMessageWithError(HttpStatusCode.InternalServerError, result.Message);

            if (result.Data.Status == SearchDriversResultStatus.Found)
                return this.Request.GetMessageWithObject(HttpStatusCode.Found, result.Data.FoundDrivers);
            else if (result.Data.Status == SearchDriversResultStatus.Suggestions)
                return this.Request.GetMessageWithObject(HttpStatusCode.SeeOther, result.Data.Suggestions);
            else
                return this.Request.GetMessage(HttpStatusCode.NotFound);
        }
        #endregion
    }
}