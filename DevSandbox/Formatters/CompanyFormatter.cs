using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using DevSandbox.Models;

namespace DevSandbox
{
    public class CompanyFormatter : IFormatter
    {
        public ISurrogateSelector SurrogateSelector { get; set; }
        public SerializationBinder Binder { get; set; }
        public StreamingContext Context { get; set; }

        public CompanyFormatter()
        {
            Context = new StreamingContext(StreamingContextStates.All);
        }

        public object Deserialize(Stream serializationStream)
        {
            StreamReader sr = new StreamReader(serializationStream);

            string line = sr.ReadLine();
            char[] delim = new char[] { ':' };

            // Get Type from serialized data.
            //string className = $"DevSandbox.Models.{sarr[0].Replace("[", "").Replace("]", "")}";
            //Type t = Type.GetType(className, false, true);

            //// Create object of just found type name.
            //var obj = FormatterServices.GetUninitializedObject(t);

            var company = new Company();
            var properties = company.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.GetCustomAttribute(typeof(IgnoreMemberAttribute)) == null)
                .ToArray();

            object obj = company;

            while (sr.Peek() >= 0)
            {
                line = sr.ReadLine().Replace("\t", "");
                if (line.StartsWith("["))
                {
                    //string nestedClassName = $"DevSandbox.Models.{line.Replace("[", "").Replace("]", "")}";
                    //Type nestedType = Type.GetType(nestedClassName, false, true);

                    //// Create object of just found type name.
                    //var nestedObj = FormatterServices.GetUninitializedObject(typeof(List<>));

                    //var outerProperty = properties.FirstOrDefault(p => p.Name.ToUpper().Contains(line.Replace("[", "").Replace("]", "")));
                    //outerProperty.SetValue(obj, nestedObj, null);

                    switch (line)
                    {
                        case "[SUBGROUP]":
                            if (company.SubGroups == null) company.SubGroups = new List<SubGroup>();
                            var group = new SubGroup();
                            company.SubGroups.Add(group);

                            obj = group;
                            break;
                        case "[TRANSLATION]":
                            var currentGroup = (SubGroup)obj;
                            var translation = new Translation();
                            currentGroup.Translation = translation;

                            obj = translation;
                            break;

                        default:
                            continue;
                    }

                    properties = obj.GetType()
                        .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                        .Where(p => p.GetCustomAttribute(typeof(IgnoreMemberAttribute)) == null)
                        .ToArray();

                    continue;
                }

                var nameValuePair = line.Split(delim);
                var name = nameValuePair.First();
                var value = nameValuePair.Last();

                var property = properties.FirstOrDefault(p => p.Name.ToLower().Contains(name));
                if (property != null)
                {
                    var propertyValue = property.PropertyType == typeof(bool) ? value == "1"
                                            : property.PropertyType == typeof(string) && string.IsNullOrWhiteSpace(value) ? null
                                            : Convert.ChangeType(value, property.PropertyType);

                    property.SetValue(obj, propertyValue, null);
                }
            }

            sr.Close();

            return company;
        }

        public void Serialize(Stream serializationStream, object graph)
        {
            // Write class name and all properties & values to file
            StreamWriter sw = new StreamWriter(serializationStream);

            WriteProperties(graph, sw, 0);

            sw.Close();
        }

        private void WriteProperties(object graph, StreamWriter sw, int numTabs)
        {
            sw.WriteLine($"{Indent(numTabs)}[{graph.GetType().Name.ToUpper()}]");
            var properties = graph.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.GetCustomAttribute(typeof(IgnoreMemberAttribute)) == null)
                .ToArray();

            WritePropertyInfo(sw, properties, graph, numTabs + 1);
        }

        private void WritePropertyInfo(StreamWriter sw, PropertyInfo[] properties, object obj, int numTabs)
        {
            foreach (var property in properties)
            {
                if (property.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                {
                    var items = (IEnumerable)property.GetValue(obj, null);
                    if (items == null) continue;

                    foreach (var item in items)
                    {
                        WriteProperties(item, sw, numTabs);
                    }
                }
                else
                {
                    if (property.PropertyType != typeof(string) && property.PropertyType.IsClass)
                    {
                        var item = property.GetValue(obj, null);
                        if (item == null) continue;

                        WriteProperties(item, sw, numTabs);
                    }
                    else
                    {
                        var propertyValue = property.PropertyType != typeof(bool)
                            ? property.GetValue(obj)
                            : Convert.ToBoolean(property.GetValue(obj)) == false ? 0 : 1;

                        sw.WriteLine($"{Indent(numTabs)}{property.Name}:{propertyValue}");
                    }
                }
            }
        }

        private static string Indent(int numTabs)
        {
            return string.Empty.PadLeft(numTabs, '\t');
        }
    }
}
