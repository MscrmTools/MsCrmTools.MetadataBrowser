using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.ComponentModel;

namespace MsCrmTools.MetadataBrowser.AppCode.SecurityPrivilege
{
    [TypeConverter(typeof(ExpandableObjectConverter))] //SecurityPrivilegeInfoConverter
    public class SecurityPrivilegeInfo
    {
        private readonly SecurityPrivilegeMetadata p;

        public SecurityPrivilegeInfo(SecurityPrivilegeMetadata p)
        {
            this.p = p;
        }

        public bool CanBeBasic => p.CanBeBasic;

        public bool CanBeDeep => p.CanBeDeep;

        public bool CanBeGlobal => p.CanBeGlobal;

        public bool CanBeLocal => p.CanBeLocal;

        public string ExtensionData => p.ExtensionData != null ? p.ExtensionData.ToString() : "";

        public string Name => p.Name;

        public Guid PrivilegeId => p.PrivilegeId;

        public PrivilegeType PrivilegeType => p.PrivilegeType;

        public override string ToString()
        {
            return p.Name;
        }
    }
}