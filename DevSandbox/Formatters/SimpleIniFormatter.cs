using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DevSandbox
{
    // This only works with simple objects (does not work with nested objects)
    public class SimpleIniFormatter : IFormatter
    {
        public ISurrogateSelector SurrogateSelector { get; set; }
        public SerializationBinder Binder { get; set; }
        public StreamingContext Context { get; set; }

        public SimpleIniFormatter()
        {
            Context = new StreamingContext(StreamingContextStates.All);
        }

        public object Deserialize(Stream serializationStream)
        {
            StreamReader sr = new StreamReader(serializationStream);

            // Get Type from serialized data.
            string line = sr.ReadLine();
            char[] delim = new char[] { '=' };
            string[] sarr = line.Split(delim);
            string className = $"DevSandbox.Models.{sarr[0].Replace("[", "").Replace("]", "")}";
            Type t = Type.GetType(className, false, true);

            // Create object of just found type name.
            Object obj = FormatterServices.GetUninitializedObject(t);

            // Get type members.
            MemberInfo[] members = FormatterServices.GetSerializableMembers(obj.GetType(), Context);  // this uses private fields for serialization and doesn't work with public properties

            // Create data array for each member.
            object[] data = new object[members.Length];

            // Store serialized variable name -> value pairs.
            StringDictionary sdict = new StringDictionary();
            while (sr.Peek() >= 0)
            {
                line = sr.ReadLine();
                sarr = line.Split(delim);

                // key = variable name, value = variable value.
                sdict[sarr[0].Trim()] = sarr[1].Trim();
            }
            sr.Close();

            // Store for each member its value, converted from string to its type.
            for (int i = 0; i < members.Length; ++i)
            {
                FieldInfo fi = ((FieldInfo)members[i]);
                if (!sdict.ContainsKey(fi.Name))
                    throw new SerializationException("Missing field value : " + fi.Name);
                data[i] = System.Convert.ChangeType(sdict[fi.Name], fi.FieldType);
            }

            // Populate object members with theri values and return object.
            return FormatterServices.PopulateObjectMembers(obj, members, data);
        }

        public void Serialize(Stream serializationStream, object graph)
        {
            // Get fields that are to be serialized.
            MemberInfo[] members = FormatterServices.GetSerializableMembers(graph.GetType(), Context).OrderBy(f => f.MetadataToken).ToArray();

            // Get fields data.
            object[] objs = FormatterServices.GetObjectData(graph, members);

            // Write class name and all fields & values to file
            StreamWriter sw = new StreamWriter(serializationStream);
            sw.WriteLine($"[{graph.GetType().Name.ToUpper()}]");

            for (int i = 0; i < objs.Length; ++i)
            {
                sw.WriteLine("{0}={1}", members[i].Name.Replace("<", "").Replace(">", ""), objs[i]?.ToString() ?? string.Empty);
            }

            sw.Close();
        }
    }
}
