using Habanero.BO;

namespace TestHabanero.BO.Tests.Util
{
    class TestUtils
    {
        public static void SetupFixture()
        {
            BOBroker.LoadClassDefs();
        }
     
    }
}
