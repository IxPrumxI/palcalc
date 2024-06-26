﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalCalc.SaveReader.FArchive
{
    public interface IProperty
    {
        IPropertyMeta Meta { get; }
    }

    public interface IPropertyMeta
    {
        string Path { get; set; }
        Guid? Id { get; set; }
    }

    public class BasicPropertyMeta : IPropertyMeta
    {
        public string Path { get; set; }
        public Guid? Id { get; set; }
    }

    public class LiteralProperty : IProperty
    {
        public IPropertyMeta Meta => TypedMeta;
        public BasicPropertyMeta TypedMeta { get; set; }

        public object Value { get; set; }

        public override string ToString() => Value.ToString();

        public static LiteralProperty Create(string path, Guid? id, object value)
        {
            return new LiteralProperty
            {
                TypedMeta = new BasicPropertyMeta { Path = path, Id = id },
                Value = value
            };
        }

        public static LiteralProperty Create(string path, object value, Guid? guid) =>
            Create(path, guid, value);
    }

    public class EnumPropertyMeta : BasicPropertyMeta
    {
        public string EnumType { get; set; }
    }

    public class EnumProperty : IProperty
    {
        public IPropertyMeta Meta => TypedMeta;
        public EnumPropertyMeta TypedMeta { get; set; }

        public string EnumValue { get; set; }

        public override string ToString() => $"({TypedMeta.EnumType}){EnumValue}";
    }

    public class StructPropertyMeta : BasicPropertyMeta
    {
        public string StructType { get; set; }
        public Guid StructTypeId { get; set; }
    }

    public class StructProperty : IProperty
    {
        public IPropertyMeta Meta => TypedMeta;
        public StructPropertyMeta TypedMeta { get; set; }

        public object Value { get; set; }

        public override string ToString() => $"({TypedMeta.StructType}){Value}";
    }

    public class ArrayPropertyMeta : BasicPropertyMeta
    {
        public string ArrayType { get; set; }

        // nullable, for ArrayStruct properties
        public string PropName { get; set; }
        public string PropType { get; set; }
        public string TypeName { get; set; }
        public Guid? ContentId { get; set; }

        public bool IsArrayStruct => TypeName != null;
    }

    public class ArrayProperty : IProperty
    {
        public IPropertyMeta Meta => TypedMeta;
        public ArrayPropertyMeta TypedMeta { get; set; }

        public object Value { get; set; }

        public T[] Values<T>() => Value as T[];

        public string[] StringValues
        {
            get
            {
                if (TypedMeta.ArrayType == "NameProperty" || TypedMeta.ArrayType == "EnumProperty")
                    return Value as string[];
                else
                    throw new Exception($"ArrayProperty of type {TypedMeta.ArrayType} cannot be an array of strings");
            }
        }

        public byte[] ByteValues
        {
            get
            {
                if (TypedMeta.ArrayType == "ByteProperty")
                    return Value as byte[];
                else
                    throw new Exception($"ArrayProperty of type {TypedMeta.ArrayType} cannot be an array of bytes");
            }
        }
    }

    public class MapPropertyMeta : BasicPropertyMeta
    {
        public string KeyType { get; set; }
        public string ValueType { get; set; }
        public string KeyStructType { get; set; }
        public string ValueStructType { get; set; }
    }

    public class MapProperty : IProperty
    {
        public IPropertyMeta Meta => TypedMeta;
        public MapPropertyMeta TypedMeta { get; set; }

        public Dictionary<object, object> Value { get; set; }
    }
}
