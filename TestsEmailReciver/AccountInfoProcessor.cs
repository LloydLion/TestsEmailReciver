using LloydLion.Serialization.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsEmailReciver
{
	class AccountInfoProcessor : ITypeSerializationProcessor
	{
		public bool CanProcess(Type type)
		{
			return type == typeof(AccountInfo);
		}


		public void Prepare(object origin, ObjectsDataSetBuilder builder, SerializationContext ctx, Serializator invoker, ObjectsDataSetReference root)
		{
			var info = (AccountInfo)origin;

			builder.AddObjectToRoot(info.Email, null, "email");
			builder.AddObjectToRoot(info.ImapServer, null, "server");
			builder.AddObjectToRoot(info.Password, null, "password");
			builder.AddObjectToRoot(info.ServerPort, null, "port");
		}

		public void Restore(IObjectBuilder builder, ObjectsDataSetReader reader, DeserializationContext ctx, Serializator invoker, ObjectsDataSetReference root)
		{
			builder.WithValue("Email", reader.Root.ReadPrimitive("email"));
			builder.WithValue("ImapServer", reader.Root.ReadPrimitive("server"));
			builder.WithValue("Password", reader.Root.ReadPrimitive("password"));
			builder.WithValue("ServerPort", reader.Root.ReadPrimitive("port"));
		}
	}
}
