using Microsoft.Extensions.DependencyInjection;
using Tron.Model.CreateGameServices.Services;
using Tron.Model.Services.CreateGameServices;
using Tron.Model.Services.GameMangerServices;

namespace Tron.Model
{
    public static class InitializeDependences
    {
        public static void Initialize(IServiceCollection services)
        {
            services.AddSingleton<ICreateGameService, CreateGameService>();
            services.AddTransient<IGameMangerService, GameMangerService>();
        }
    }
}
