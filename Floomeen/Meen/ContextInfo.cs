using System.Collections.Generic;
using Floomeen.Exceptions;
using Newtonsoft.Json;

namespace Floomeen.Meen
{
    public class ContextInfo
    {
        private readonly Dictionary<string, object> _dictionary = new Dictionary<string, object>();

        public ContextInfo()
        {
            
        }

        public ContextInfo(Dictionary<string, object> dictionary)
        {
            _dictionary = dictionary;
        }

        public void Add<T>(string key, T data)
        {
            _dictionary.Add(key, data);
        }
       
        public TObject Get<TObject>(string key, TObject defalutValue) where TObject : class
        {
            if (!_dictionary.ContainsKey(key))
            {
                Save(key, defalutValue);

                return defalutValue;
            }

            return GetByKey<TObject>(key);
        }

        private TObject GetByKey<TObject>(string key) where TObject : class
        {
            var d = _dictionary[key] as TObject;

            if (d == null)
                d = JsonConvert.DeserializeObject<TObject>(_dictionary[key].ToString());

            return d;
        }

        public TObject Get<TObject>(string key) where TObject : class
        {
            if (!_dictionary.ContainsKey(key))
                throw new FloomineException($"MissingKey {key}");

            return GetByKey<TObject>(key);
        }

        public bool TryGet<TObject>(string key, out TObject obj) where TObject : class
        {
            obj = null;

            if (!_dictionary.ContainsKey(key))
                return false;

            obj = GetByKey<TObject>(key);

            return true;
        }


        public void Save<TObject>(string key, TObject value) where TObject : class
        {
            _dictionary[key] = value;
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(_dictionary);
        }

        public static ContextInfo Deserialize(string serialized)
        {
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(serialized);

            return new ContextInfo(dictionary);
        }
    }
}
