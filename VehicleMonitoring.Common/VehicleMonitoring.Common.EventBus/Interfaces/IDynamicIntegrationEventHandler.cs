using System.Threading.Tasks;

namespace VehicleMonitoring.Common.EventBus.Interfaces
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}
