using System;
using System.Configuration;

namespace Sparc.KPK2.Config.Settings
{
    public class ConfigurationElementCollection<TElementType> : ConfigurationElementCollection 
        where TElementType : ConfigurationElement, IUniqueConfigurationElement, new()
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new TElementType();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((TElementType)element).Name;
        }

        public TElementType this[int index]
        {
            get
            {
                return (TElementType)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public new TElementType this[string name]
        {
            get
            {
                return (TElementType)BaseGet(name);
            }
        }

        public int IndexOf(TElementType element)
        {
            return BaseIndexOf(element);
        }

        public void Add(TElementType element)
        {
            BaseAdd(element);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(TElementType element)
        {
            if (BaseIndexOf(element) >= 0)
                BaseRemove(element.Name);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
        }
    }
}
