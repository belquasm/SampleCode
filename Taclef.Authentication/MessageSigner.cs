using System;
using System.CodeDom;
using System.Security.Cryptography;
using System.Text;

namespace Taclef.Authentication
{
	public static class MessageSigner
	{
		public static string CreateKey()
		{
			using (var csp = new RSACryptoServiceProvider())
			{
				return ToBase64(csp.ToXmlString(true));
			}
		}

		public static string GetPublicKey(string privateKey)
		{
			using (var csp = CreateProvider(privateKey))
			{
				return ToBase64(csp.ToXmlString(false));
			}
		}

		private static string ToBase64(string s)
		{
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
		}

		private static string FromBase64(string s)
		{
			return Encoding.UTF8.GetString(Convert.FromBase64String(s));
		}

		public static RSACryptoServiceProvider CreateProvider(string key)
		{
			var csp = new RSACryptoServiceProvider();
			csp.FromXmlString(FromBase64(key));
			return csp;
		}

		public static string SignMessage(string message, string key)
		{
			using (var csp = CreateProvider(key))
			using (var sha = new SHA1Managed())
			{
				var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(message));
				var formatter = new RSAPKCS1SignatureFormatter(csp);
				formatter.SetHashAlgorithm("SHA1");
				var signature = formatter.CreateSignature(hash);
				return Convert.ToBase64String(signature);
			}
		}

		public static bool VerifySignature(string message, string signature, string key)
		{
			using (var csp = CreateProvider(key))
			using (var sha = new SHA1Managed())
			{
				var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(message));
				var deformatter = new RSAPKCS1SignatureDeformatter(csp);
				deformatter.SetHashAlgorithm("SHA1");
				return deformatter.VerifySignature(hash, Convert.FromBase64String(signature));
			}
		}
	}
}