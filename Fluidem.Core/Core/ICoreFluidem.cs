using System;
using System.Threading.Tasks;
using Fluidem.Core.Models;

namespace Fluidem.Core
{
    public interface ICoreFluidem
    {
        public Task SaveExceptionAsync(DetailError e);
        public DetailError GetException(string uid);
        public void CreateTable();
    }
}