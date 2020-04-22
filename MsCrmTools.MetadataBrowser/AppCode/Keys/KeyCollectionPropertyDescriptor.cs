using System;
using System.ComponentModel;

namespace MsCrmTools.MetadataBrowser.AppCode.Keys
{
    /// <summary>
    ///     Summary description for CollectionPropertyDescriptor.
    /// </summary>
    public class KeyCollectionPropertyDescriptor : PropertyDescriptor
    {
        private readonly KeyCollection collection;
        private readonly int index = -1;

        public KeyCollectionPropertyDescriptor(KeyCollection coll, int idx) :
            base("#" + idx, null)
        {
            collection = coll;
            index = idx;
        }

        public override AttributeCollection Attributes => new AttributeCollection(null);

        public override Type ComponentType => collection.GetType();

        public override string Description
        {
            get
            {
                KeyMetadataInfo kmi = collection[index];
                return kmi.SchemaName;
            }
        }

        public override string DisplayName
        {
            get
            {
                KeyMetadataInfo kmi = collection[index];
                return kmi.SchemaName;
            }
        }

        public override bool IsReadOnly => true;

        public override string Name => "#" + index;

        public override Type PropertyType => collection[index].GetType();

        public override bool CanResetValue(object component)
        {
            return true;
        }

        public override object GetValue(object component)
        {
            return collection[index];
        }

        public override void ResetValue(object component)
        {
        }

        public override void SetValue(object component, object value)
        {
            // collection[index] = value;
        }

        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }
    }
}