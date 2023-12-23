using System.Web.Http;

namespace FrameworkDev.Web.Helpers
{
    public class CustomApiController<TRepository> : ApiController where TRepository : new()
    {
        public readonly TRepository repo = new TRepository();
    }
}
