using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fluidem.Core.Models;

namespace Fluidem.Core
{
    public interface IProvider
    {
        public void BootstrapProvider();
        public Task SaveExceptionAsync(ErrorDetail e);
        public Task<IEnumerable<Error>> GetExceptionsAsync();
        public Task<ErrorDetail> GetExceptionAsync(Guid id);
    }
}