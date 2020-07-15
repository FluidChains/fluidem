using System;
using System.Linq;
using Xunit;
using static Xunit.Assert;

namespace Fluidem.Core.Test
{
    public class TestOne
    {
        public static int GenerateException(string txt)
        {
            /*var clsObject = new ExceptionCoreFluidem();
            try
            {
                return int.Parse(txt);
            }
            catch (Exception e)
            {
                clsObject.ReceiptException(e);
                Console.WriteLine($"ok {e}");
                throw;
            }*/
            return 1;

        }

    }

    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Throws<FormatException>(() => TestOne.GenerateException("TEXT"));
        }
        
        [Fact]
        public void GenerateExceptionOne()
        {
                
        }
    }
}