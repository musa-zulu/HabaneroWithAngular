using Habanero.Base.Exceptions;
using Habanero.BO;

namespace TestHabanero.BO
{
    public class BORegistry
    {
        private static IDataAccessor _dataAccessor;
        private static readonly object _lockObject = new object();

        public static IDataAccessor DataAccessor
        {
            get
            {
                if (_dataAccessor == null)
                    throw new HabaneroApplicationException("The DataAccessor has not been set up on BORegistry. Please initialise it before attempting to load or save Business Objects.");
                return _dataAccessor;
            }
            set { _dataAccessor = value; }
        }

        private static IBusinessObjectManager _businessObjectManager;

        public static IBusinessObjectManager BusinessObjectManager
        {
            get
            {
                lock (_lockObject)
                {
                    return _businessObjectManager ?? Habanero.BO.BusinessObjectManager.Instance;
                }
            }
            set { _businessObjectManager = value; }
        }
    }
}
