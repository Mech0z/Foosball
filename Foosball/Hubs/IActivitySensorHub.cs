using System.Threading.Tasks;

namespace Foosball.Hubs
{
    public interface IActivitySensorHub
    {
        Task SendAsync(string title, bool activity);
    }
}