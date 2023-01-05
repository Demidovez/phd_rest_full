using Uniformance.PHD;

namespace phd_api
{
    public class PHD_Server
    {
        public PHDHistorian server = null;

        public PHD_Server()
        {
            server = new PHDHistorian();
            server.DefaultServer = new PHDServer("127.0.0.1", SERVERVERSION.RAPI200); // SERVERVERSION.API200, SERVERVERSION.RAPI200
            server.DefaultServer.UserName = "user";
            server.DefaultServer.Password = "pass";
            server.DefaultServer.WindowsUsername = "user";
            server.DefaultServer.WindowsPassword = "pass";
            server.DefaultServer.Port = 1234;
        }

        public void Dispose()
        {
            server.Dispose();
        }
    }
}
