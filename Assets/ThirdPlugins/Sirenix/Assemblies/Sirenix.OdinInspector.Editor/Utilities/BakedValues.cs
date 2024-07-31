//-----------------------------------------------------------------------
// <copyright file="BakedValues.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor
{
    using UnityEngine;
    using System;
    using Sirenix.Serialization;
    using System.Collections.Generic;
    using Sirenix.Serialization.Utilities;
    using System.IO;
    using Sirenix.Utilities;

    public static class BakedValues
    {
        private static Dictionary<string, object> loadedValues;

#if SIRENIX_INTERNAL

        public static void BuildBakedValues()
        {
            var bytes = WriteValues(ValueDefinitions);
            SaveBakedValueBytes(bytes);
            //Debug.Log("Saved hash: " + MathUtilities.ComputeByteArrayHash(bytes));
        }

        private static void SaveBakedValueBytes(byte[] bytes)
        {
            var path = SirenixAssetPaths.OdinPath + "/Assets/Editor/ConfigData.bytes";
            File.WriteAllBytes(path, bytes);
        }

        public static void DeleteBakedValues()
        {
            var path = SirenixAssetPaths.OdinPath + "/Assets/Editor/ConfigData.bytes";
            File.Delete(path);
        }

        private static byte[] WriteValues(ValueDefinition[] values)
        {
            using (var stream = new MemoryStream())
            using (var writerCache = Cache<BinaryDataWriter>.Claim())
            {
                var writer = writerCache.Value;
                writer.PrepareNewSerializationSession();
                writer.Stream = stream;
                writer.CompressStringsTo8BitWhenPossible = false;

                writer.BeginArrayNode(values.Length);
                foreach (var value in values)
                {
                    writer.BeginStructNode(value.Name, null);
                    writer.WriteInt32(null, (int)value.Type);
                    writer.WriteInt32(null, value.StartPattern.Length);
                    writer.WriteInt32(null, value.Data.Length);
                    writer.WriteString(null, value.StartPattern + value.Data + value.EndPattern);
                    writer.EndNode(value.Name);
                }
                writer.EndArrayNode();

                writer.FlushToStream();
                writer.Dispose();

                return stream.ToArray();
            }
        }
#endif

        private static byte[] LoadBakedValueBytes()
        {
            var path = SirenixAssetPaths.OdinPath + "/Assets/Editor/ConfigData.bytes";
            if (!File.Exists(path)) return null;
            var bytes = File.ReadAllBytes(path);
            //Debug.Log("Loaded hash: " + MathUtilities.ComputeByteArrayHash(bytes));
            return bytes;
        }

        private static Dictionary<string, object> ReadValues(byte[] bytes)
        {
            var values = new Dictionary<string, object>();

            using (var stream = new MemoryStream(bytes))
            using (var readerCache = Cache<BinaryDataReader>.Claim())
            {
                var reader = readerCache.Value;
                reader.PrepareNewSerializationSession();
                reader.Stream = stream;
                stream.Position = 0;

                Type discard;
                long count;

                if (!reader.EnterArray(out count)) throw new ArgumentException();

                for (int i = 0; i < count; i++)
                {
                    if (!reader.EnterNode(out discard)) throw new ArgumentException();

                    string name = reader.CurrentNodeName;
                    string data;
                    int type, startLength, dataLength;

                    if (string.IsNullOrEmpty(name)) throw new ArgumentException();
                    if (!reader.ReadInt32(out type)) throw new ArgumentException();
                    if (!reader.ReadInt32(out startLength)) throw new ArgumentException();
                    if (!reader.ReadInt32(out dataLength)) throw new ArgumentException();
                    if (!reader.ReadString(out data)) throw new ArgumentException();
                    if (!reader.ExitNode()) throw new ArgumentException();

                    var value = GetValueFromData(data.Substring(startLength, dataLength), (BakedValueType)type);

                    values.Add(name, value);
                }

                if (!reader.ExitArray()) throw new ArgumentException();
                return values;
            }
        }

        private static unsafe object GetValueFromData(string data, BakedValueType type)
        {
            switch (type)
            {
                case BakedValueType.String:
                    return data.Trim();
                case BakedValueType.Int:
                    if (data.Length != 2) throw new ArgumentException();

                    fixed (char* ptr = data)
                    {
                        return *(int*)ptr;
                    }
                case BakedValueType.DateTime:
                    if (data.Length != sizeof(long) / 2) throw new ArgumentException();

                    fixed (char* ptr = data)
                    {
                        return DateTime.FromBinary(*(long*)ptr);
                    }
                case BakedValueType.Bool:
                    if (data.Length != 1) throw new ArgumentException();
                    return data[0] != 0;
                case BakedValueType.Invalid:
                default:
                    throw new ArgumentException();
            }
        }


        public static bool TryGetBakedValue<T>(string name, out T value)
        {
            if (loadedValues == null)
            {
                try
                {
                    var bytes = LoadBakedValueBytes();
                    if (bytes != null)
                    {
                        loadedValues = ReadValues(bytes);
                    }
                    else
                    {
                        // There are no baked values
                        loadedValues = new Dictionary<string, object>();
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogException(new Exception("Could not load baked config values from file.", ex));
                    loadedValues = new Dictionary<string, object>();
                }
            }

            object weakValue;

            if (!loadedValues.TryGetValue(name, out weakValue))
            {
                value = default(T);
                return false;
            }

            if (weakValue == null)
            {
                value = default(T);
                return typeof(T) == typeof(string);
            }

            if (!(weakValue is T))
            {
                value = default(T);
                return false;
            }

            value = (T)weakValue;
            return true;
        }

        public enum BakedValueType
        {
            Invalid = 0,
            String = 1,
            Int = 2,
            DateTime = 3,
            Bool = 4
        }

#if SIRENIX_INTERNAL
        public class ValueDefinition
        {
            public string Name;
            public string StartPattern;
            public string EndPattern;
            public string Data;
            public BakedValueType Type;
        }


        public static readonly ValueDefinition[] ValueDefinitions = new ValueDefinition[]
        {
            new ValueDefinition()
            {
                Name = "Licensee",
                StartPattern = "b20380160e9b4d219fceb2407a5f5c37",
                EndPattern = "8a37047477754636b2aff701621a6152",
                Data = "                                                                                                                                ",
                Type = BakedValueType.String,
            },
            // Actually, I think we should leave this one using the old string literal approach,
            // as that makes it a little more opaque and harder to mess with. And it does not need
            // to work with source mode at all.
            //new ValueDefinition()
            //{
            //    Name = "ExpirationTime",
            //    StartPattern = "f94d96de81d04ad5803f2fb70c45cb10",
            //    EndPattern = "685d",
            //    Data = "\0\0\0\0",
            //    Type = BakedValueType.DateTime,
            //},
            new ValueDefinition()
            {
                Name = "OrgClaimToken",
                StartPattern = "dc4897892113491a89e4698ea1a45631",
                EndPattern = "99a9d2a4c33b44078acb5a54bcdb4a5a",
                Data = "                                ",
                Type = BakedValueType.String,
            },
            new ValueDefinition()
            {
                Name = "LicensingIsEnabled",
                StartPattern = "3a35939e6c94456684c0f84fa3d11e4b",
                EndPattern = "cd23531f35264637ab7b18834d8857ee",
                Data = "\0",
                Type = BakedValueType.Bool,
            },
            new ValueDefinition()
            {
                Name = "OrgClaimIsEnabled",
                StartPattern = "eae4491a49604d90a859445c3b9cd449",
                EndPattern = "b946bdc052a849f8ac142ec2cedc1144",
                Data = "\0",
                Type = BakedValueType.Bool,
            },
            new ValueDefinition()
            {
                Name = "OrgID",
                StartPattern = "079e1998b0d24386932655cc63699d85",
                EndPattern = "8741f2a316aa46dcaa107f45a10e06cd",
                Data = "\0\0",
                Type = BakedValueType.Int,
            },
            new ValueDefinition()
            {
                Name = "OrgName",
                StartPattern = "24d32e62593f4be1972094dbca25316b",
                EndPattern = "6f677572d409488e8dfe129c9246b46a",
                Data = "                                                                                                    ",
                Type = BakedValueType.String,
            }
        };
#endif
    }
}
#endif