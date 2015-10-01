using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

namespace UnityRose
{
    class CacheItem : IComparer
    {
        public int hits { get; set; }
        public object item { get; set;  }

        public CacheItem( object obj )
        {
            item = obj;
            hits = 1;
        }

        /// <summary>
        /// IComparer Compare override
        /// </summary>
        /// <param name="x"> left object </param>
        /// <param name="y"> right object </param>
        /// <returns></returns>
        public int Compare(object x, object y)
        {
            CacheItem left = (CacheItem)x;
            CacheItem right = (CacheItem)y;

            if (left.hits > right.hits)
                return 1;
            if (left.hits < right.hits)
                return -1;
            else
                return 0;
        }
    }


    /// <summary>
    /// This class is used to cache resources that are used most commonly in the current map
    /// </summary>
    class Cache
    {
        private ResourceManager _resourceManager;
        private int _maxSize;
        Dictionary<string, CacheItem> _cacheDictionary;

        /// <summary>
        /// Constructor for a Cache with a max size
        /// </summary>
        /// <param name="rm"> A reference to the resource manager for loading/unloading resources</param>
        /// <param name="maxSize"> The max size for this cache </param>
        public Cache(ResourceManager rm, int maxSize)
        {
            _resourceManager = rm;
            _maxSize = maxSize;

            _cacheDictionary = new Dictionary<string, CacheItem>(_maxSize);
        }


        public object request(string resource)
        {
            object obj;

            // Check if this resource exists
            if (_cacheDictionary.ContainsKey(resource))  // Cache hit
            {
                _cacheDictionary[resource].hits++;
                obj = _cacheDictionary[resource].item;
            }
            else    // Cache miss
            {
                // Load resource from file
                obj = _resourceManager.loadResource(resource);

                // If there is no free space in cache, remove the least used resource from it
                if ( _cacheDictionary.Count >= _maxSize )
                {
                    // Remove the least used resource from memory and cache
                    var keyValuePair = _cacheDictionary.Min();
                    _cacheDictionary.Remove(keyValuePair.Key);
                    DirectoryInfo dir = new DirectoryInfo(keyValuePair.Key);
                    if( (dir.Extension == ".png") || (dir.Extension == ".PNG"))
                        _resourceManager.unloadResource((UnityEngine.Object)keyValuePair.Value.item);
                }

                // Add the new resource to cache
                _cacheDictionary.Add(resource, new CacheItem(obj));

            }

            return obj;
                
        }
    }
}
