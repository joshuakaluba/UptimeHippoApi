using UptimeHippoApi.Data.DataContext;

namespace UptimeHippoApi.Data.DataAccessLayer
{
    public abstract class BaseRepository
    {
        protected UptimeHippoDataContext DataContext;
    }
}