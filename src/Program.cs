using System;

namespace SimpleHTTPServer
{
	class main
	{
		static void Main(string[] args)
		{
			HTTPServer server = new HTTPServer("http://localhost:8000/");
			server.enableLogging();
			server.run();
		}
	}
}