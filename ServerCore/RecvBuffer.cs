using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public class RecvBuffer
    {
        ArraySegment<byte> _buffer;
        int _readPos;
        int _writePos;

        public RecvBuffer(int bufferSize)
        {
            _buffer = new ArraySegment<byte>(new byte[bufferSize], 0, bufferSize);
        }

        public int DataSize { get { return _writePos - _readPos; } }
        public int FreeSize { get { return _buffer.Count - _writePos; } }

        public ArraySegment<byte> ReadSegment
        {
            get { return new ArraySegment<byte>(_buffer.Array, _buffer.Offset + _readPos, DataSize); }
        }

        public ArraySegment<byte> WriteSegment
        {
            get { return new ArraySegment<byte>(_buffer.Array, _buffer.Offset + _writePos, FreeSize); }
        }

        public void Clean()
        {
            int dataSize = DataSize;
            if (dataSize == 0)
            {
                //남은 데이터가 없을 때 위치만 리셋
                _readPos = _writePos = 0;
            }
            else
            {
                //남은 데이터가 있을때 시작위치로 복사
                Array.Copy(_buffer.Array, _buffer.Offset + _readPos, _buffer.Array, _buffer.Offset, dataSize);
                //Array.Copy((원본배열), srcIndex(원본배열에서 복사시작할위치), destArray(데이터를 붙여넣을 대상배열),
                //destIndex(대상배열에서 붙여넣기 시작할 위치), length(복사할 요소수)
                _readPos = 0;
                _writePos = dataSize;
            }
        }

        public bool OnRead(int numOfByte)
        {
            if (numOfByte > DataSize)
                return false;

            _readPos += numOfByte;
            return true;
        }

        public bool OnWrite(int numOfByte)
        {
            if (numOfByte > FreeSize)
                return false;

            _writePos += numOfByte;
            return true;
        }
    }
}
