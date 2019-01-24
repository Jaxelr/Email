using Api.Model.Operations;
using NotDeadYet;
using NotDeadYet.Results;
using ServiceStack.Common.Web;
using ServiceStack.ServiceInterface;
using System.Net;

namespace Api.ServiceInterface
{
    public class HealthCheckService : Service
    {
        private readonly IHealthChecker _healthChecker;

        public HealthCheckService(IHealthChecker healthChecker)
        {
            _healthChecker = healthChecker;
        }

        public object Get(HealthCheck request)
        {
            var result = _healthChecker.Check();
            var statusCode = (result.Status == HealthCheckStatus.NotOkay)
                ? HttpStatusCode.ServiceUnavailable
                : HttpStatusCode.OK;

            return new HttpResult(result, statusCode);
        }
    }
}