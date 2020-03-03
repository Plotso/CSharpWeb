namespace SIS.HTTP
{
    using System.Threading.Tasks;

    public interface IHttpServer
    {
        Task StartAsync();

        Task ResetAsync();

        void Stop();
    }
}