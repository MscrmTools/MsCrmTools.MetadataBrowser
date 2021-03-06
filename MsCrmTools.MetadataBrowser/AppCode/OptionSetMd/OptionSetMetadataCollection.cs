﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace MsCrmTools.MetadataBrowser.AppCode.OptionSetMd
{
    public class OptionSetMetadataCollection : CollectionBase, ICustomTypeDescriptor
    {
        private readonly List<OptionSetMetadataInfo> list;

        public OptionSetMetadataCollection()
        {
            list = new List<OptionSetMetadataInfo>();
        }

        public new int Count => list.Count;

        public OptionSetMetadataInfo this[int index] => list[index];

        public void Add(OptionSetMetadataInfo info)
        {
            list.Add(info);
        }

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return GetProperties();
        }

        public PropertyDescriptorCollection GetProperties()
        {
            var pds = new PropertyDescriptorCollection(null);
            for (int i = 0; i < list.Count; i++)
            {
                var pd = new OptionSetMetadataCollectionPropertyDescriptor(this, i);
                pds.Add(pd);
            }
            return pds;
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        public void Remove(OptionSetMetadataInfo info)
        {
            list.Remove(info);
        }
    }
}