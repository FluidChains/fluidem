using System.Threading.Tasks;
using Fluidem.Core.Models;

namespace Fluidem.Core
{
    public interface IProvider
    {
        public void BootstrapProvider();
        public Task SaveExceptionAsync(DetailError e);
        public Task<DetailError> GetExceptionAsync(string uid);
    }
}