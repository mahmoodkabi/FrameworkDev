using AutoMapper;
using FrameworkDev.Web.Areas.Management.Models;
using FrameworkDev.Web.Models;

namespace FrameworkDev.Web.Helpers
{
    public sealed class DomainMapper
    {
        private static volatile DomainMapper instance;
        private static readonly object syncRoot = new object();

        private DomainMapper()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.ValidateInlineMaps = false;

                //cfg.CreateMap<Role, VM_Role>().ConvertUsing<M2VM_Role_Converter>();
                //cfg.CreateMap<VM_Role, Role>().ConvertUsing<VM2M_Role_Converter>();
            });

            Mapper.AssertConfigurationIsValid();
        }

        public static DomainMapper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new DomainMapper();
                        }
                    }
                }

                return instance;
            }
        }
    }
}
