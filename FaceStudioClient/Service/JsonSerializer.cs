#define JSONNET
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FaceStudioClient.Service
{
    public class JsonSerializer<T>
    {
        public static String Serialize(T t)
        {
#if JSONNET
            //, new IsoDateTimeConverter()
            return JsonConvert.SerializeObject(t);
#else
            MemoryStream ms = new MemoryStream();
            DataContractJsonSerializer d = new DataContractJsonSerializer(typeof(T));
            d.WriteObject(ms, t);

            ms.Position = 0;
            StreamReader reader = new StreamReader(ms);
            return reader.ReadToEnd();
#endif
        }

        public static T Deserialize(Stream s)
        {
#if JSONNET
            StreamReader reader = new StreamReader(s);
            String str = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<T>(str);
#else
            //JsonReaderWriterFactory jrw = JsonReaderWriterFactory.CreateJsonReader(
            DataContractJsonSerializer dcjs = new DataContractJsonSerializer(typeof(T));
            T t = (T)dcjs.ReadObject(s);
            return t;
#endif
        }

        public static T Deserialize(string str)
        {
#if JSONNET
            return JsonConvert.DeserializeObject<T>(str);
#else
            ////JsonReaderWriterFactory jrw = JsonReaderWriterFactory.CreateJsonReader(
            //DataContractJsonSerializer dcjs = new DataContractJsonSerializer(typeof(T));
            //T t = (T)dcjs.ReadObject(s);
            //return t;
            throw new NotImplementedException();
#endif
        }
    }

    public class A
    {
        [JsonConverter(typeof(IsoDateTimeConverter))] 
        public DateTime Date
        {
            get;
            set;
        }
    }

    interface IJsonSerializer
    {
        String Serialize(Object obj);
        Object Deserialize(Stream s);
    }

    class JsonNetSerializer : IJsonSerializer
    {
        public String Serialize(Object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public Object Deserialize(Stream s)
        {
            StreamReader reader = new StreamReader(s);
            return JsonConvert.DeserializeObject(reader.ReadToEnd());
        }
    }
}
