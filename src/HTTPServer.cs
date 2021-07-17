using System;
using System.IO;
using System.Text;
using System.Net;
using System.Threading.Tasks;

namespace SimpleHTTPServer
{
	class HTTPServer
	{
		public HTTPServer(string sConnectionString) {this.sConnectionString = sConnectionString;}

		private bool bLogging = false;
		private bool bServerRunning = false;

		private string sConnectionString;

		private HttpListener listener;

		private async Task listen()
		{
			while (this.bServerRunning)
			{
				HttpListenerContext context = await this.listener.GetContextAsync();

				HttpListenerRequest request = context.Request;

				if (this.bLogging)
				{
					Console.WriteLine("\nUrl: " + request.Url.ToString());
					Console.WriteLine("Method: " + request.HttpMethod);
					Console.WriteLine("Host name: " + request.UserHostName);
				}

				HttpListenerResponse response = context.Response;
				response.ContentEncoding = Encoding.UTF8;

				byte[] data = new byte[0];

				switch (request.HttpMethod)
				{
					case ("GET"):
						if (request.Url.AbsolutePath == "/")
						{
							response.ContentType = "text/html";
							string sFileContent = System.IO.File.ReadAllText("./static/index.html");
							data = Encoding.UTF8.GetBytes(sFileContent);
						}
						else
						{
							if (request.Url.AbsolutePath.Contains(".js"))
							{
								response.ContentType = "text/javascript";
								string sFileContent = System.IO.File.ReadAllText("./" + request.Url.AbsolutePath);
								data = Encoding.UTF8.GetBytes(sFileContent);
							}
							else if (request.Url.AbsolutePath.Contains(".ico"))
							{
								response.ContentType = "image/x-icon";
								data = System.IO.File.ReadAllBytes("./" + request.Url.AbsolutePath);
							}
							else if (request.Url.AbsolutePath.Contains(".css"))
							{
								response.ContentType = "text/css";
								string sFileContent = System.IO.File.ReadAllText("./" + request.Url.AbsolutePath);
								data = Encoding.UTF8.GetBytes(sFileContent);
							}
						}
						break;

					case ("POST"):
						break;

					case ("PUT"):
						break;

					case ("DELETE"):
						break;
				}

				response.ContentLength64 = data.LongLength;

				await response.OutputStream.WriteAsync(data, 0, data.Length);
				
				response.Close();
			}
		}

		public void run()
		{
			this.listener = new HttpListener();
			this.listener.Prefixes.Add(this.sConnectionString);
			this.listener.Start();

			this.bServerRunning = true;

			Console.WriteLine("\nServer has been started: " + this.sConnectionString);

			Task listenTask = this.listen();
			listenTask.GetAwaiter().GetResult();

			this.listener.Close();
		}

		public void enableLogging() {this.bLogging = true;}
	}
}