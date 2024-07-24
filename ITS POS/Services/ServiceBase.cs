using ITS_POS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS_POS.Services
{
    public abstract class ServiceBase
    {
        #region Data Members

        protected static DataContextDb __context = null;

        #endregion

        #region Functions

        #region Get Context

        public static DataContextDb GetContext()
        {
            return __context;
        }

        #endregion

        #region Initialize

        public static void Initialize(DataContextDb context)
        {
            __context = context;
        }

        #endregion

        #endregion

    }
}
