using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MsCrmTools.MetadataBrowser.AppCode
{
    public static class Extensions
    {
        public static List<string> GetSearchParts(string searchTerm)
        {
            List<string> parts = new List<string>();

            if (searchTerm.IndexOf("\"") >= 0)
            {
                var quotedParts = searchTerm.Split(new[] { '\"' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToArray();
                if (quotedParts.Length == 2)
                {
                    if (quotedParts[0].StartsWith("\"")) quotedParts[0] = quotedParts[0].Remove(0, 1);
                    if (quotedParts[0].EndsWith("\"")) quotedParts[0] = quotedParts[0].Substring(0, quotedParts[0].Length - 1);
                    if (quotedParts[1].StartsWith("\"")) quotedParts[1] = quotedParts[1].Remove(0, 1);
                    if (quotedParts[1].EndsWith("\"")) quotedParts[1] = quotedParts[1].Substring(0, quotedParts[1].Length - 1);

                    quotedParts[0] = quotedParts[0].Trim();
                    quotedParts[1] = quotedParts[1].Trim();
                }

                parts = quotedParts.ToList();
            }

            if (parts.Count == 0)
                parts = searchTerm.Split(' ').Where(t => !string.IsNullOrEmpty(t)).ToList();

            return parts;
        }

        public static bool Matches(this string toTest, string searchTerm)
        {
            if (toTest.ToLower().IndexOf(searchTerm.ToLower()) < 0) return false;

            return true;
        }

        public static bool Matches(this Label labelToTest, string searchTerm)
        {
            return labelToTest?.LocalizedLabels.Any(l => l.Label.ToLower().IndexOf(searchTerm.ToLower()) >= 0) ?? false;
        }

        public static bool Matches(this RelationshipMetadataBase m, string searchTerm, EntityMetadata[] all)
        {
            List<string> parts = GetSearchParts(searchTerm);

            if (m is OneToManyRelationshipMetadata otmmd)
            {
                return parts.Count == 1 && (otmmd.SchemaName.Matches(parts[0])
                                            || otmmd.ReferencedEntity.Matches(parts[0])
                                            || otmmd.ReferencingEntity.Matches(parts[0])
                                            || otmmd.ReferencedAttribute.Matches(parts[0])
                                            || otmmd.ReferencingAttribute.Matches(parts[0])
                                            || otmmd.MetadataId.Value.ToString("B").Matches(parts[0])

                                            || all.First(e => e.LogicalName == otmmd.ReferencedEntity).DisplayName.Matches(parts[0])
                                            || all.First(e => e.LogicalName == otmmd.ReferencingEntity).DisplayName.Matches(parts[0])
                                            //|| all.SelectMany(e => e.Attributes).Any(a => a.LogicalName == otmmd.ReferencedAttribute && a.DisplayName.Matches(parts[0]))
                                            //|| all.SelectMany(e => e.Attributes).Any(a => a.LogicalName == otmmd.ReferencingAttribute && a.DisplayName.Matches(parts[0]))
                                            )
                      || parts.Count == 2 && (otmmd.SchemaName.Matches(parts[0])
                                            || otmmd.ReferencedEntity.Matches(parts[0])
                                            || otmmd.ReferencingEntity.Matches(parts[0])
                                            || otmmd.ReferencedAttribute.Matches(parts[0])
                                            || otmmd.ReferencingAttribute.Matches(parts[0])
                                            || otmmd.MetadataId.Value.ToString("B").Matches(parts[0])

                                            || all.First(e => e.LogicalName == otmmd.ReferencedEntity).DisplayName.Matches(parts[0])
                                            || all.First(e => e.LogicalName == otmmd.ReferencingEntity).DisplayName.Matches(parts[0])
                                            //|| all.SelectMany(e => e.Attributes).Any(a => a.LogicalName == otmmd.ReferencedAttribute && a.DisplayName.Matches(parts[0]))
                                            //|| all.SelectMany(e => e.Attributes).Any(a => a.LogicalName == otmmd.ReferencingAttribute && a.DisplayName.Matches(parts[0]))
                                            )
                                            &&
                                            (otmmd.SchemaName.Matches(parts[1])
                                            || otmmd.ReferencedEntity.Matches(parts[1])
                                            || otmmd.ReferencingEntity.Matches(parts[1])
                                            || otmmd.ReferencedAttribute.Matches(parts[1])
                                            || otmmd.ReferencingAttribute.Matches(parts[1])
                                            || otmmd.MetadataId.Value.ToString("B").Matches(parts[1])

                                            || all.First(e => e.LogicalName == otmmd.ReferencedEntity).DisplayName.Matches(parts[1])
                                            || all.First(e => e.LogicalName == otmmd.ReferencingEntity).DisplayName.Matches(parts[1])
                                            //|| all.SelectMany(e => e.Attributes).Any(a => a.LogicalName == otmmd.ReferencedAttribute && a.DisplayName.Matches(parts[1]))
                                            //|| all.SelectMany(e => e.Attributes).Any(a => a.LogicalName == otmmd.ReferencingAttribute && a.DisplayName.Matches(parts[1]))
                                            );
            }
            else if (m is ManyToManyRelationshipMetadata mtmmd)
            {
                return parts.Count == 1 && (mtmmd.SchemaName.Matches(parts[0])
                                           || mtmmd.IntersectEntityName.Matches(parts[0])
                                           || mtmmd.Entity1LogicalName.Matches(parts[0])
                                           || mtmmd.Entity2LogicalName.Matches(parts[0])
                                           || mtmmd.Entity1IntersectAttribute.Matches(parts[0])
                                           || mtmmd.Entity2IntersectAttribute.Matches(parts[0])
                                           || mtmmd.MetadataId.Value.ToString("B").Matches(parts[0])

                                           || all.First(e => e.LogicalName == mtmmd.Entity1LogicalName).DisplayName.Matches(parts[0])
                                           || all.First(e => e.LogicalName == mtmmd.Entity2LogicalName).DisplayName.Matches(parts[0])
                                           //|| all.SelectMany(e => e.Attributes).Any(a => a.LogicalName == mtmmd.Entity1IntersectAttribute && a.DisplayName.Matches(parts[0]))
                                           //|| all.SelectMany(e => e.Attributes).Any(a => a.LogicalName == mtmmd.Entity2IntersectAttribute && a.DisplayName.Matches(parts[0]))
                                           )
                     || parts.Count == 2 && (mtmmd.SchemaName.Matches(parts[0])
                                           || mtmmd.IntersectEntityName.Matches(parts[0])
                                           || mtmmd.Entity1LogicalName.Matches(parts[0])
                                           || mtmmd.Entity2LogicalName.Matches(parts[0])
                                           || mtmmd.Entity1IntersectAttribute.Matches(parts[0])
                                           || mtmmd.Entity2IntersectAttribute.Matches(parts[0])
                                           || mtmmd.MetadataId.Value.ToString("B").Matches(parts[0])

                                           || all.First(e => e.LogicalName == mtmmd.Entity1LogicalName).DisplayName.Matches(parts[0])
                                           || all.First(e => e.LogicalName == mtmmd.Entity2LogicalName).DisplayName.Matches(parts[0])
                                           //|| all.SelectMany(e => e.Attributes).Any(a => a.LogicalName == mtmmd.Entity1IntersectAttribute && a.DisplayName.Matches(parts[0]))
                                           //|| all.SelectMany(e => e.Attributes).Any(a => a.LogicalName == mtmmd.Entity2IntersectAttribute && a.DisplayName.Matches(parts[0]))
                                           )
                                           &&
                                           (mtmmd.SchemaName.Matches(parts[1])
                                            || mtmmd.IntersectEntityName.Matches(parts[1])
                                           || mtmmd.Entity1LogicalName.Matches(parts[1])
                                           || mtmmd.Entity2LogicalName.Matches(parts[1])
                                           || mtmmd.Entity1IntersectAttribute.Matches(parts[1])
                                           || mtmmd.Entity2IntersectAttribute.Matches(parts[1])
                                           || mtmmd.MetadataId.Value.ToString("B").Matches(parts[1])

                                           || all.First(e => e.LogicalName == mtmmd.Entity1LogicalName).DisplayName.Matches(parts[1])
                                           || all.First(e => e.LogicalName == mtmmd.Entity2LogicalName).DisplayName.Matches(parts[1])
                                           //|| all.SelectMany(e => e.Attributes).Any(a => a.LogicalName == mtmmd.Entity1IntersectAttribute && a.DisplayName.Matches(parts[1]))
                                           //|| all.SelectMany(e => e.Attributes).Any(a => a.LogicalName == mtmmd.Entity2IntersectAttribute && a.DisplayName.Matches(parts[1]))
                                           );
            }

            return false;
        }

        public static bool Matches(this MetadataBase m, string searchTerm, EntityMetadata[] all)
        {
            List<string> parts = GetSearchParts(searchTerm);

            if (m is EntityMetadata emd)
            {
                if (parts.Count == 2) return false;

                return emd.LogicalName.Matches(parts[0]) || emd.DisplayName.Matches(parts[0]) || emd.MetadataId.Value.ToString("B").Matches(parts[0]);
            }
            else if (m is AttributeMetadata amd)
            {
                return parts.Count == 1 && (amd.LogicalName.Matches(parts[0]) || amd.DisplayName.Matches(parts[0]) || amd.MetadataId.Value.ToString("B").Matches(parts[0]))
                       || (parts.Count == 2 && (amd.LogicalName.Matches(parts[0]) || amd.DisplayName.Matches(parts[0]) || amd.MetadataId.Value.ToString("B").Matches(parts[0]))
                       && (amd.EntityLogicalName.Matches(parts[1]) || all.First(e => e.LogicalName == amd.EntityLogicalName).DisplayName.Matches(parts[1])));
            }
            else if (m is EntityKeyMetadata ekmd)
            {
                return parts.Count == 1 && (ekmd.LogicalName.Matches(parts[0]) || ekmd.DisplayName.Matches(parts[0]) || ekmd.MetadataId.Value.ToString("B").Matches(parts[0]))
                    || (parts.Count == 2 && (ekmd.LogicalName.Matches(parts[0]) || ekmd.DisplayName.Matches(parts[0]) || ekmd.MetadataId.Value.ToString("B").Matches(parts[0]))
                     && (ekmd.EntityLogicalName.IndexOf(parts[1]) >= 0 || all.First(e => e.LogicalName == ekmd.EntityLogicalName).DisplayName.Matches(parts[1])));
            }

            return false;
        }
    }
}