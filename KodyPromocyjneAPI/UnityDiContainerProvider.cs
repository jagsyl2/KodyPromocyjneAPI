using KodyPromocyjneAPI.BusinessLayer;
using KodyPromocyjneAPI.BusinessLayer.Services;
using KodyPromocyjneAPI.DataLayer;
using Unity;

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

            container.RegisterFactory<Func<IPromoCodesDbContext>>(c => new Func<IPromoCodesDbContext>(() => new PromoCodesDbContext()));

            return container;
        }
    }
}
