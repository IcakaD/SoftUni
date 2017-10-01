using System;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace urlcreate
{
	class MainClass
	{

		public static void Main (string[] args)
		{
			string action = "";
			string url = "";
			string id = "";
			string code = RandomString (6);
			string type = "";
			string newCode = "";
			string value = "";

			while (action != "list" && action != "add" && action != "remove" && action != "edit") {
				Console.Write ("Please select an ACTION ");
				Console.Write ("[list | add | remove | edit]: ");
				action = Console.ReadLine ();
			}

			switch (action) {
				
			case "list":
				
				listAction ();

				break;
			case "add":
				
				while (url.Length == 0) {
					Console.Write ("Enter a valid URL to add: ");
					url = Console.ReadLine ();
				}

				addAction (url, code);
				Console.WriteLine ("https://icaka.info/r/" + code);
				break;
			case "remove":
				
				while (id.Length == 0) {
					Console.Write ("Enter a valid ID to remove: ");
					type = Console.ReadLine ();
				}

				removeAction (id);

				break;
			case "edit":

				while (id.Length == 0) {
					Console.Write("Select an ID to edit: ");
					id = Console.ReadLine();
				}

				while (type.Length == 0 && type != "url" && type != "code") {
					Console.Write ("Chose what you want to edit [url | code]: ");
					type = Console.ReadLine ();
				}

				if (type == "url") {
					while (url.Length == 0) {
						Console.Write ("Enter a new valid URL: ");
						value = Console.ReadLine ();
					}
				}

				if(type == "code"){
					while (newCode.Length == 0 && newCode != "random" && newCode != "custom") {
						Console.Write ("What kind of code do you want [random | custom]: ");
						newCode = Console.ReadLine ();
					}

					if (newCode == "custom") {
						Console.Write ("Enter your new code: ");
						value = Console.ReadLine ();
					}

					if (newCode == "random") {
						value = RandomString (6);
					}
				}

				editAction (id, type, value);

				break;

			}

		}

		private static void editAction(string id, string type, string value){
			htmlResponse ("edit&id=" + id + "&type="+ type +"&value=" + value);

		}

		private static void removeAction(string id){
			htmlResponse ("remove&id=" + id);
		}

		private static void listAction(){
			Console.WriteLine ();
			htmlResponse ("list");

		}

		private static void addAction (string url, string code)
		{
			htmlResponse ("add&url=" + url + "&code=" + code);

		}

		private static void htmlResponse(string curl){
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://shorturl.icaka.info/?action=" + curl);
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();

			if (response.StatusCode == HttpStatusCode.OK)
			{
				Stream receiveStream = response.GetResponseStream();
				StreamReader readStream = null;

				if (response.CharacterSet == null)
				{
					readStream = new StreamReader(receiveStream);
				}
				else
				{
					readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
				}

				string data = readStream.ReadToEnd();
				response.Close();
				readStream.Close();
				Console.WriteLine (data.Replace("<br/>", "\n"));
			}
		}

		private static Random random = new Random ();

		public static string RandomString (int length)
		{
			const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			return new string (Enumerable.Repeat (chars, length)
				.Select (s => s [random.Next (s.Length)]).ToArray ());
		}

	}
}