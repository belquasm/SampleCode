using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace Taclef.Authentication.LoginProviders
{
	public static class Serializer
	{

		public static T DeserializeFromQuery<T>(string query) where T : class, new()
		{
			if (query == null) return null;
			var obj = new T();
			var type = typeof (T);
			foreach (var pair in query.Split('&'))
			{
				var values = pair.Split('=');
				if (values.Length != 2) continue;
				var property = type.GetProperty(values[0], BindingFlags.Instance | BindingFlags.Public);
				if (property == null || !property.CanWrite) continue;
				var value = Convert.ChangeType(values[1], property.PropertyType);
				property.SetValue(obj, value);

			}
			return obj;
		}

		public static T DeserializeBase64<T>(string s) where T : class, new()
		{
			return s == null ? null : DeserializeFromQuery<T>(Encoding.UTF8.GetString(Convert.FromBase64String(s)));
		}

		public static string SerializeToQuery(object obj)
		{
			if (obj == null) return null;
			var sb = new StringBuilder();
			foreach (var property in obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.CanWrite && p.CanRead))
			{
				var value = property.GetValue(obj);
				if (value == null) continue;
				if (sb.Length > 0)
				{
					sb.Append("&");
				}
				sb.Append(HttpUtility.UrlEncode(property.Name, Encoding.UTF8));
				sb.Append('=');
				sb.Append(HttpUtility.UrlEncode(value.ToString(), Encoding.UTF8));
			}
			return sb.ToString();
		}

		public static string SerializeToBase64(object obj)
		{
			var query = SerializeToQuery(obj);
			return obj == null ? null : Convert.ToBase64String(Encoding.UTF8.GetBytes(query));
		}

	}
}