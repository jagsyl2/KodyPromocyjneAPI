using KodyPromocyjneAPI.BusinessLayer;
using KodyPromocyjneAPI.BusinessLayer.Services;
using KodyPromocyjneAPI.DataLayer;
using Unity;
using Unity.Injection;

namespace KodyPromocyjneAPI
{
    public class UnityDiContainerProvider
    {
        public IUnityContainer GetContainer()
        {
            var container = new UnityContainer();

            container.RegisterType<IDatabaseManagementService, DatabaseManagementService>();
            container.RegisterType<IPromoCodeServices, PromoCodeServices>();
            container.RegisterType<IChangeLogServices, ChangeLogServices>();

            container.RegisterType<Func<IPromoCodesDbContext>>(new InjectionFactory(c => new Func<IPromoCodesDbContext>(() => new PromoCodesDbContext())));

            return container;
        }
    }
}
