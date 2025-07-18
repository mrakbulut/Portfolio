using System;
using UnityEngine;

namespace Portfolio.Utility
{
    [Serializable]
    public struct SerializableGuid : IEquatable<SerializableGuid>
    {
        [SerializeField] [HideInInspector] public uint Part1;
        [SerializeField] [HideInInspector] public uint Part2;
        [SerializeField] [HideInInspector] public uint Part3;
        [SerializeField] [HideInInspector] public uint Part4;

        public static SerializableGuid Empty => new SerializableGuid(0, 0, 0, 0);

        public SerializableGuid(uint val1, uint val2, uint val3, uint val4)
        {
            Part1 = val1;
            Part2 = val2;
            Part3 = val3;
            Part4 = val4;
        }

        public SerializableGuid(Guid guid)
        {
            var bytes = guid.ToByteArray();
            Part1 = BitConverter.ToUInt32(bytes, 0);
            Part2 = BitConverter.ToUInt32(bytes, 4);
            Part3 = BitConverter.ToUInt32(bytes, 8);
            Part4 = BitConverter.ToUInt32(bytes, 12);
        }

        public static SerializableGuid NewGuid()
        {
            return Guid.NewGuid().ToSerializableGuid();
        }

        public static SerializableGuid FromHexString(string hexString)
        {
            if (hexString.Length != 32)
            {
                return Empty;
            }

            return new SerializableGuid
            (
                Convert.ToUInt32(hexString.Substring(0, 8), 16),
                Convert.ToUInt32(hexString.Substring(8, 8), 16),
                Convert.ToUInt32(hexString.Substring(16, 8), 16),
                Convert.ToUInt32(hexString.Substring(24, 8), 16)
            );
        }

        public string ToHexString()
        {
            return $"{Part1:X8}{Part2:X8}{Part3:X8}{Part4:X8}";
        }

        public Guid ToGuid()
        {
            var bytes = new byte[16];
            BitConverter.GetBytes(Part1).CopyTo(bytes, 0);
            BitConverter.GetBytes(Part2).CopyTo(bytes, 4);
            BitConverter.GetBytes(Part3).CopyTo(bytes, 8);
            BitConverter.GetBytes(Part4).CopyTo(bytes, 12);
            return new Guid(bytes);
        }

        public static implicit operator Guid(SerializableGuid serializableGuid)
        {
            return serializableGuid.ToGuid();
        }

        public static implicit operator SerializableGuid(Guid guid)
        {
            return new SerializableGuid(guid);
        }

        public override bool Equals(object obj)
        {
            return obj is SerializableGuid guid && Equals(guid);
        }

        public bool Equals(SerializableGuid other)
        {
            return Part1 == other.Part1 && Part2 == other.Part2 && Part3 == other.Part3 && Part4 == other.Part4;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Part1, Part2, Part3, Part4);
        }

        public static bool operator ==(SerializableGuid left, SerializableGuid right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SerializableGuid left, SerializableGuid right)
        {
            return !(left == right);
        }
    }
}