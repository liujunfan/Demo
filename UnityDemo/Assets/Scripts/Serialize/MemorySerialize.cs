using System;
using System.IO;
using UnityEngine;

public class CMemorySerialize
{
	public CMemorySerialize()
	{
		m_Ms = new MemoryStream();
	}

	public CMemorySerialize(byte[] buf)
	{
		m_bytes = buf;
		m_iOffset = 0;
	}

	int m_iOffset = 0;
	byte[] m_bytes = null;
	MemoryStream m_Ms = null;

	public char asChar
	{
		get
        {
            return BitConverter.ToChar (m_bytes, m_iOffset++);
        }
		set
		{
			m_Ms.Write (BitConverter.GetBytes (value), 0, sizeof(char));
            m_iOffset += sizeof(char);
		}
	}
	
	public byte asByte
	{
		get
        {
            return m_bytes[m_iOffset++];
        }
		set
		{
			m_Ms.Write (BitConverter.GetBytes (value), 0, sizeof(byte));
            m_iOffset += sizeof(byte);
		}
	}
    public short SetFloat(float value)
    {
        return asShort = (short)(value * 100);
    }
    public float GetFloat
    {
        get
        {
            return asShort / 100.0f;
        }
    }
	public short asShort
	{
		get
		{
			short r = BitConverter.ToInt16 (m_bytes, m_iOffset);
			m_iOffset += sizeof(short);
			return r;
		}

		set
		{
			m_Ms.Write (BitConverter.GetBytes (value), 0, sizeof(short));
            m_iOffset += sizeof(short);
		}
	}
	
	public ushort asUShort
	{
		get
		{
			ushort r = BitConverter.ToUInt16 (m_bytes, m_iOffset);
			m_iOffset += sizeof(ushort);
			return r;
		}

		set
		{
			m_Ms.Write (BitConverter.GetBytes (value), 0, sizeof(ushort));
            m_iOffset += sizeof(ushort);

		}
	}
	
	public int asInt
	{
		get
		{
			int r = BitConverter.ToInt32 (m_bytes, m_iOffset);
			m_iOffset += sizeof(int);
			return r;
		}

		set
		{
			m_Ms.Write (BitConverter.GetBytes (value), 0, sizeof(int));
            m_iOffset += sizeof(int);
		}
	}
	
	public uint asUInt
	{
		get
		{
			uint r = BitConverter.ToUInt32 (m_bytes, m_iOffset);
			m_iOffset += sizeof(uint);
			return r;
		}
		set
		{
			m_Ms.Write (BitConverter.GetBytes (value), 0, sizeof(uint));
            m_iOffset += sizeof(uint);
		}
	}
	
	public string asString
	{
		get
		{
			int len = (int)asUShort;
			string r = System.Text.Encoding.UTF8.GetString (m_bytes, m_iOffset, len);
			m_iOffset += len;
			return r;
		}
		set
		{
			byte[] data = System.Text.Encoding.UTF8.GetBytes(value);
			asUShort = (ushort)data.Length;
			m_Ms.Write(data, 0, data.Length);
            m_iOffset += data.Length;
		}
	}

	public virtual byte[] GetBuffer()
	{
        byte[] result = new byte[m_iOffset];
        Buffer.BlockCopy(m_Ms.GetBuffer(), 0, result, 0, m_iOffset);
        m_Ms.Close();
        return result;
	}
}
