using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Aramex.Models
{
    internal class ExceptionConverter : JavaScriptConverter
    {
        public override IEnumerable<Type> SupportedTypes
        {
            //Add a list of the type you want to convert here
            get { return new ReadOnlyCollection<Type>(new List<Type>(new[] { typeof(Exception) })); }
        }

        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            if (dictionary == null) throw new ArgumentNullException("dictionary");

            //check that the passed object is of the correct type
            if (type == typeof(Exception))
            {
                //Since we've lost most of the data of the original exception
                //we'll deserialize to a plain Exception object
                var message = dictionary[@"Message"].ToString();
                return new Exception(message);
            }

            return null;
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            var exception = obj as Exception;
            if (exception != null)
            {
                var result = new Dictionary<string, object>
                    {
                        //Make sure anything added here is serializable!
                        {@"Type", exception.GetType().ToString()},
                        {@"Message", exception.Message},
                        {@"Source", exception.Source}
                        //add whatever other properties you're interested in seeing on the client
                    };

                return result;
            }
            return new Dictionary<string, object>();
        }
    }
}