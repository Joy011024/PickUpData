using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureMvcExt.Factory
{
    public class FacadeFactory : PureMVC.Patterns.Facade.Facade
    {
        private static FacadeFactory mInstance = null;

        public static FacadeFactory FacadeInstance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new FacadeFactory();
                }

                return mInstance;
            }
        }
        public FacadeFactory() :this("")
        {

        }
        public FacadeFactory(string key) : base(key)
        {

        }
        protected override void InitializeController()
        {
            base.InitializeController();


        }
        protected override void InitializeModel()
        {
            base.InitializeModel();
            try
            {

            }
            catch (Exception ex)
            {
            }
        }
    }
}
