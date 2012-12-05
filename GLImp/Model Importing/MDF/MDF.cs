/**************************************************************
 *  
 * MDF.cs -- Contains the MDFChunkReader and MDFChunkWriter
 * classes, used for reading and writing chunks and data 
 * contained within them for many of the file formats created
 * for the Mila engine.
 * 
 * Part of the Mila Engine.
 * Added on Thursday, July 30th, 2009.
 * Copyright(C) Aleksey Okoneshnikov, all rights reserved.
 * 
 **************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace Mila
{
    namespace Utility
    {
        namespace MDF
        {
            /// <summary>
            /// Structure used to store the header information of an MDF chunk.
            /// </summary>
            public struct MDFChunkHeader
            {
                /// <summary>
                /// The unique ID tag of this header, defining which type of data resides in the chunk.
                /// </summary>
                public UInt32 id;

                /// <summary>
                /// The size of the entire chunk, including this header, in octet bytes.
                /// </summary>
                public UInt32 size;

                /// <summary>
                /// The byte offset in the file at which this header was retrieved.
                /// Ignored for header writing purposes.
                /// </summary>
                public long offset;
            }


            /// <summary>
            /// Represents a reader that can be used to read files stored in the Mila Data File format.
            /// The reader is not thread-safe! One MDFChunkReader per thread and vice versa please.
            /// </summary>
            public class MDFChunkReader
            {
                private BinaryReader reader = null;


                /// <summary>
                /// Creates a new MDFChunkReader wrapped around the existing BinaryReader object.
                /// </summary>
                /// <param name="reader">the existing opened BinaryReader to use.</param>
                public MDFChunkReader(BinaryReader reader)
                {
                    this.reader = reader;
                    if (this.reader == null)
                    {
                        throw new ArgumentNullException("reader", "new MDFReader: BinaryReader parameter cannot be null!");
                    }
                }

                /// <summary>
                /// Creates a new MDFChunkReader opened on the given file.  
                /// If the file cannot be opened for reading, throws an IOException.
                /// </summary>
                /// <param name="filepath">The filepath to the file to open.</param>
                public MDFChunkReader(string filepath)
                {
                    try
                    {
                        this.reader = new BinaryReader(new FileStream(filepath, FileMode.Open, FileAccess.Read), Encoding.Unicode);
                    }
                    catch (ArgumentException ex)
                    {
                        throw new IOException("new MDFReader: Could not open the path specified.", ex);
                    }
                }

                /// <summary>
                /// Searches for a top-level chunk with a given id inside of the given container chunk.
                /// This method assumes that starting with the offset given, there is nothing in the container chunk besides
                /// chunks written one after another.  Any stray data between chunks is unexpected.
                /// </summary>
                /// <param name="container">The chunk to search within.</param>
                /// <param name="id">The id of the chunk to look for.</param>
                /// <param name="start">The starting offset from the beginning of the container chunk data to apply.</param>
                /// <returns>The header of the chunk found, or a header with an offset of -1 if none is found.</returns>
                public MDFChunkHeader findNextChunk(MDFChunkHeader container, UInt32 id, long start)
                {
                    long origPos = reader.BaseStream.Position;
                    long curPos = start + container.offset + sizeof(UInt32) + sizeof(UInt32);
                    MDFChunkHeader temp;

                    while (curPos < container.offset + container.size)
                    {
                        reader.BaseStream.Seek(curPos, SeekOrigin.Begin);
                        if ((temp = peekChunkHeader()).id == id)
                        {
                            reader.BaseStream.Seek(origPos, SeekOrigin.Begin);
                            return temp;
                        }
                        else
                        {
                            curPos = temp.offset + temp.size;
                        }
                    }

                    reader.BaseStream.Seek(origPos, SeekOrigin.Begin);
                    temp.offset = -1;
                    temp.id = 0;
                    temp.size = 0;
                    return temp;
                }


                /// <summary>
                /// Retrieves and returns the chunk header at the current location in the file, but does not advance the file position.
                /// </summary>
                /// <returns>The chunk header read.</returns>
                public MDFChunkHeader peekChunkHeader()
                {
                    long curPos = reader.BaseStream.Position;
                    MDFChunkHeader header;

                    header.id = reader.ReadUInt32();
                    header.size = reader.ReadUInt32();
                    header.offset = curPos;

                    reader.BaseStream.Seek(curPos, SeekOrigin.Begin);
                    return header;
                }

                /// <summary>
                /// Returns true if the given chunk is empty.
                /// </summary>
                /// <param name="header">The header of the chunk to test.</param>
                /// <returns>true if the given chunk is completely empty (in which the size field only accounts for the header), false otherwise.</returns>
                public bool isEmpty(MDFChunkHeader header)
                {
                    if (header.size == sizeof(UInt32) + sizeof(UInt32))
                        return true;
                    else
                        return false;
                }

                /// <summary>
                /// Skips over the given chunk header, positioning the file position at the beginning of the chunk's data.
                /// </summary>
                /// <param name="header">The chunk header of the chunk to enter.</param>
                public void enterChunk(MDFChunkHeader header)
                {
                    reader.BaseStream.Seek(header.offset + sizeof(UInt32) + sizeof(UInt32), SeekOrigin.Begin);
                }

                /// <summary>
                /// Skips the current chunk by seeking past its size.
                /// </summary>
                /// <param name="chunk">The header of the chunk to skip over.</param>
                public void skipChunk(MDFChunkHeader chunk)
                {
                    reader.BaseStream.Seek(chunk.offset + chunk.size, SeekOrigin.Begin);
                }

                /// <summary>
                /// Reads in a UInt32 value.
                /// </summary>
                /// <returns>The value read in.</returns>
                public UInt32 readUInt32()
                {
                    return reader.ReadUInt32();
                }

                /// <summary>
                /// Reads in an Int32 value.
                /// </summary>
                /// <returns>the value read.</returns>
                public Int32 readInt32()
                {
                    return reader.ReadInt32();
                }

                /// <summary>
                /// Reads in an IEEE 32-bit floating point number.
                /// </summary>
                /// <returns>the value read.</returns>
                public float readFloat()
                {
                    return reader.ReadSingle();
                }

                /// <summary>
                /// Reads in a UInt32 packed in a special variable-length encoding.
                /// If the first byte is 0xFE, then reads in the next two bytes as a short.
                /// If the first byte is 0xFF, then reads in the next four bytes as an int.
                /// Otherwise, reads in the first byte as the value.
                /// </summary>
                /// <returns>The value read as a UInt32</returns>
                public UInt32 readUVInt32()
                {
                    byte firstByte = reader.ReadByte();
                    if (firstByte == 0xFF)
                    {
                        return reader.ReadUInt32();
                    }
                    else if (firstByte == 0xFE)
                    {
                        return (UInt32)reader.ReadUInt16();
                    }
                    else
                    {
                        return (UInt32)firstByte;
                    }
                }

                /// <summary>
                /// Reads in an Int32 packed in a special variable-length encoding.
                /// If the first byte is 0x81 (-127), then reads in the next two bytes as a short.
                /// If the first byte is 0x80 (-128), then reads in the next four bytes as an int.
                /// Otherwise, reads in the first byte as the value.
                /// </summary>
                /// <returns>The value read as an Int32</returns>
                public Int32 readVInt32()
                {
                    sbyte firstByte = reader.ReadSByte();
                    if (firstByte == -128)
                    {
                        return reader.ReadInt32();
                    }
                    else if (firstByte == -127)
                    {
                        return (Int32)reader.ReadInt16();
                    }
                    else
                    {
                        return (Int32)firstByte;
                    }
                }

                /// <summary>
                /// Reads in an array of UInts stored in a variable-length encoding.
                /// </summary>
                /// <returns>An array with the appropriate UInt32s.</returns>
                public UInt32[] readUVIntArray()
                {
                    UInt32 size = readUVInt32();
                    List<UInt32> intList = new List<UInt32>();

                    for (UInt32 i = 0; i < size; i++)
                        intList.Add(readUVInt32());
                    return intList.ToArray();
                }

                /// <summary>
                /// Reads in an array of Ints stored in a variable-length encoding.
                /// </summary>
                /// <returns>An array with the appropriate Int32s.</returns>
                public Int32[] readVIntArray()
                {
                    UInt32 size = readUVInt32();
                    List<Int32> intList = new List<Int32>();

                    for (UInt32 i = 0; i < size; i++)
                        intList.Add(readVInt32());
                    return intList.ToArray();
                }

                /// <summary>
                /// Reads in a size-prefixed string encoded in UTF-16 Unicode.
                /// </summary>
                /// <returns>the value read.</returns>
                public string readString()
                {
                    return reader.ReadString();
                }

                /// <summary>
                /// Closes this reader.
                /// </summary>
                public void close()
                {
                    reader.Close();
                }

                /// <summary>
                /// Returns the position in the stream.
                /// </summary>
                /// <returns>the position in the stream, in bytes from 0.</returns>
                public long tell()
                {
                    return reader.BaseStream.Position;
                }

                /// <summary>
                /// Seeks to the given position in the stream.
                /// </summary>
                /// <param name="position">The byte offset in the stream to seek to.</param>
                public void seek(long position)
                {
                    reader.BaseStream.Seek(position, SeekOrigin.Begin);
                }

                /// <summary>
                /// Seeks the stream to the end (where next write occurs, etc).
                /// </summary>
                public void seekEnd()
                {
                    reader.BaseStream.Seek(0L, SeekOrigin.End);
                }
            }

            /// <summary>
            /// Represents a writer that can be used to write files stored in the Mila Data File format.
            /// The writer is not thread-safe! One MDFChunkWriter per thread and vice versa please.
            /// </summary>
            public class MDFChunkWriter
            {
                private BinaryWriter writer = null;

                /// <summary>
                /// Creates a new MDFChunkWriter object wrapped around the given existing BinaryWriter.
                /// If the writer is null, throws an ArgumentNullException.
                /// </summary>
                /// <param name="writer">the BinaryWriter object to use.</param>
                public MDFChunkWriter(BinaryWriter writer)
                {
                    this.writer = writer;

                    if (this.writer == null)
                    {
                        throw new ArgumentNullException("writer", "new MDFChunkWriter: BinaryWriter parameter cannot be null!");
                    }
                }

                /// <summary>
                /// Creates a new MDFChunkWriter object bound to a BinaryWriter created for the given filepath.
                /// If the file could not be opened, throws an IOException.
                /// </summary>
                /// <param name="filepath">The path to the file to write to.</param>
                public MDFChunkWriter(string filepath)
                {
                    try
                    {
                        this.writer = new BinaryWriter(new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write), Encoding.Unicode);
                    }
                    catch (ArgumentException ex)
                    {
                        throw new IOException("new MDFChunkWriter: Could not create or open the given file for writing.", ex);
                    }
                }

                /// <summary>
                /// Closes this writer and flushes all of the data.
                /// </summary>
                public void close()
                {
                    writer.Close();
                }


                /// <summary>
                /// Writes the chunk header and returns a new header with an offset field marking the place where the header was written.
                /// </summary>
                /// <param name="header"></param>
                public MDFChunkHeader writeChunkHeader(MDFChunkHeader header)
                {
                    header.offset = writer.BaseStream.Position;
                    writer.Write(header.id);
                    writer.Write(header.size);
                    return header;
                }

                /// <summary>
                /// Calculates the size of the chunk from the current file position back to the start of the chunk header and writes that size into the chunk header.
                /// The size is corrected straight into the file, and the file position is reset back to the end of the chunk.
                /// </summary>
                /// <param name="header">The chunk header of this chunk.</param>
                /// <returns>The updated chunk header structure, with the new corrected size field (just in case anyone needs it).</returns>
                public MDFChunkHeader writeCalculatedChunkSize(MDFChunkHeader header)
                {
                    long curPos = writer.BaseStream.Position;
                    header.size = (UInt32)(curPos - header.offset);
                    writer.BaseStream.Seek(header.offset, SeekOrigin.Begin);
                    header = writeChunkHeader(header);
                    writer.BaseStream.Seek(curPos, SeekOrigin.Begin);
                    return header;
                }

                /// <summary>
                /// Writes an Int32 value
                /// </summary>
                /// <param name="val">the value to write.</param>
                public void writeInt32(Int32 val)
                {
                    writer.Write(val);
                }

                /// <summary>
                /// Writes a UInt32 value.
                /// </summary>
                /// <param name="val">the value to write.</param>
                public void writeUInt32(UInt32 val)
                {
                    writer.Write(val);
                }

                /// <summary>
                /// Writes an IEEE 32-bit floating-point number.
                /// </summary>
                /// <param name="val">the value to write.</param>
                public void writeFloat(float val)
                {
                    writer.Write(val);
                }

                /// <summary>
                /// Writes a UInt32 as a variable-length encoded value.
                /// </summary>
                /// <param name="val">the value to write.</param>
                public void writeUVInt32(UInt32 val)
                {
                    if (val >= 254)
                    {
                        if (val >= 65536)
                        {
                            writer.Write((byte)0xFF);
                            writer.Write(val);
                        }
                        else
                        {
                            writer.Write((byte)0xFE);
                            writer.Write((UInt16)val);
                        }
                    }
                    else
                    {
                        writer.Write((byte)val);
                    }
                }

                /// <summary>
                /// Writes a Int32 as a variable-length encoded value.
                /// </summary>
                /// <param name="val">the value to write.</param>
                public void writeVInt32(Int32 val)
                {
                    if (val > 127 || val <= -127)
                    {
                        if (val > 32767 || val < -32768)
                        {
                            writer.Write((sbyte)-128);
                            writer.Write(val);
                        }
                        else
                        {
                            writer.Write((sbyte)-127);
                            writer.Write((Int16)val);
                        }
                    }
                    else
                    {
                        writer.Write((sbyte)val);
                    }
                }

                /// <summary>
                /// Writes an array of UInt32s using variable-length encoding.
                /// </summary>
                /// <param name="arr">the array to write.</param>
                public void writeUVIntArray(UInt32[] arr)
                {
                    writeUVInt32((UInt32)arr.Length);
                    for (int i = 0; i < arr.Length; i++)
                    {
                        writeUVInt32(arr[i]);
                    }
                }

                /// <summary>
                /// Writes an array of Int32s using variable-length encoding.
                /// </summary>
                /// <param name="arr">the array to write.</param>
                public void writeVIntArray(Int32[] arr)
                {
                    writeUVInt32((UInt32)arr.Length);
                    for (int i = 0; i < arr.Length; i++)
                    {
                        writeVInt32(arr[i]);
                    }
                }

                /// <summary>
                /// Writes a Unicode UTF-16 size-prefixed string.
                /// </summary>
                /// <param name="str">the string to write</param>
                public void writeString(string str)
                {
                    writer.Write(str);
                }

                /// <summary>
                /// Returns the stream position at this current location.
                /// </summary>
                /// <returns>the stream position at this current location.</returns>
                public long tell()
                {
                    return writer.BaseStream.Position;
                }

                /// <summary>
                /// Seeks to the given position in the stream.
                /// </summary>
                /// <param name="place">the position to seek to, in bytes.</param>
                public void seek(long place)
                {
                    writer.BaseStream.Seek(place, SeekOrigin.Begin);
                }

                /// <summary>
                /// Seeks to the end of the stream.
                /// </summary>
                public void seekEnd()
                {
                    writer.BaseStream.Seek(0, SeekOrigin.End);
                }
            }
        }
    }
}