//using System;
//using System.Collections.Generic;
//using System.Text;
//using Mila.Utility.MDF;
//using Mila.Types;
//using Mila.Data;

//namespace Mila.Utility.MM
//{
//    /// <summary>
//    /// Used to read a Mila Model file (.mm file) into a Model object.
//    /// Can also read in a raw MMDL chunk.
//    /// </summary>
//    public class MMReader
//    {
//        private MDFChunkReader reader = null;
//        private static int version = 110; // version 1.10

//        /// <summary>
//        /// Creates a new MMReader from the given MDFChunkReader object.
//        /// </summary>
//        /// <param name="reader">the MDFChunkReader containing the stream to read from.</param>
//        public MMReader(MDFChunkReader reader)
//        {
//            if (reader == null)
//            {
//                throw new ArgumentNullException("reader", "new MMReader: MDFChunkReader parameter must reference a valid open reader.");
//            }
//            else
//                this.reader = reader;
//        }

//        /// <summary>
//        /// Creates a new MMReader and opens it to the given path.
//        /// </summary>
//        /// <param name="filepath">The full path to the file to open for reading.</param>
//        public MMReader(string filepath)
//        {
//            reader = new MDFChunkReader(filepath);
//        }

//        /// <summary>
//        /// Reads a Mila Model file (.mm file) and fills the given model.
//        /// </summary>
//        /// <param name="model">The model to read out of the file.</param>
//        /// <returns>An errorcode specifying the success of the operation.</returns>
//        public MMReaderErrors readMMFile(ref Model model)
//        {
//            return readMM__Chunk(ref model);
//        }

//        /// <summary>
//        /// Returns the version of the MM file.
//        /// </summary>
//        /// <returns>The version of the MM file as an integer code (ex: 100 = version 1.00).  If the MM file is corrupt, returns -1.</returns>
//        public int getMMFileVersion()
//        {
//            MDFChunkHeader header = reader.peekChunkHeader();

//            if (header.id != (Int32)MMChunkIds.MM__)
//                return -1;

//            reader.enterChunk(header);

//            return reader.readInt32();
//        }

//        /// <summary>
//        /// Closes the reader object used to read the Mila Model (.mm) file.
//        /// </summary>
//        public void close()
//        {
//            reader.close();
//        }

//        /// <summary>
//        /// Reads the "MM  " chunk containing the entire contents of the Mila Model (.mm) file.
//        /// </summary>
//        /// <param name="model">The model to read to.</param>
//        /// <returns>An errorcode specifying the success of the operation.</returns>
//        private MMReaderErrors readMM__Chunk(ref Model model)
//        {
//            MDFChunkHeader header = reader.peekChunkHeader();

//            if (header.id != (Int32)MMChunkIds.MM__)
//                return MMReaderErrors.FileFormatUnrecognized;

//            reader.enterChunk(header);

//            int fileVersion = reader.readInt32();

//            if (fileVersion != version)
//                return MMReaderErrors.IncompatibleVersion;

//            int checkSum = reader.readInt32();

//            // FIXME: Compare checksums.

//            return readMMDLChunk(ref model);
//        }


//        /// <summary>
//        /// Reads in the MMDL chunk and fills the model object given.
//        /// This method can be called directly if there is an MMDL chunk not inside of a Mila Model (.mm) file.
//        /// </summary>
//        /// <param name="model">The model object to fill.</param>
//        /// <returns>An errorcode specifying the success of the operation.</returns>
//        public MMReaderErrors readMMDLChunk(ref Model model)
//        {
//            MDFChunkHeader header = reader.peekChunkHeader();
//            MMReaderErrors error;

//            if (header.id != (Int32)MMChunkIds.MMDL)
//                return MMReaderErrors.FileCorrupt;

//            reader.enterChunk(header);

//            model = new Model();

//            model.Name = reader.readString();


//            long offset = reader.tell() - header.offset - (sizeof(Int32) * 2);

//            if ((error = readVLSTChunk(ref model, header, offset)) != MMReaderErrors.Success)
//                return error;

//            if ((error = readALSTChunk(ref model, header, offset)) != MMReaderErrors.Success)
//                return error;

//            if ((error = readFLSTChunk(ref model, header, offset)) != MMReaderErrors.Success)
//                return error;

//            return MMReaderErrors.Success;
//        }

//        /// <summary>
//        /// Reads the VLST chunk containing the vertex list.
//        /// </summary>
//        /// <param name="model">The model to fill.</param>
//        /// <param name="parent">The parent chunk (MMDL)</param>
//        /// <param name="offset">The chunk-data-relative offset to the set of chunks contained in the parent chunk.</param>
//        /// <returns>An errorcode specifying the success of the operation.</returns>
//        private MMReaderErrors readVLSTChunk(ref Model model, MDFChunkHeader parent, long offset)
//        {
//            MDFChunkHeader header = new MDFChunkHeader();

//            header = reader.findNextChunk(parent, (Int32)MMChunkIds.VLST, offset);
//            if (header.offset == -1)
//                return MMReaderErrors.MissingVertexList;

//            reader.enterChunk(header);

//            int length = reader.readVInt32();

//            // There shouldn't be less than zero vertices.
//            if (length < 0)
//                return MMReaderErrors.FileCorrupt;

//            model.Vertices = new Vertex[length];

//            double largestRadius = -1.0f;
//            double candidateRadius = 0.0f;

//            for (int i = 0; i < length; i++)
//            {
//                model.Vertices[i] = new Vertex();
//                model.Vertices[i].x = reader.readFloat();
//                model.Vertices[i].y = reader.readFloat();
//                model.Vertices[i].z = reader.readFloat();

//                // Let's compute the bounding sphere for the model while we're at it.
//                if ((candidateRadius = (double)Math.Sqrt((model.Vertices[i].x) * (model.Vertices[i].x) +
//                                                        (model.Vertices[i].y) * (model.Vertices[i].y) +
//                                                        (model.Vertices[i].z) * (model.Vertices[i].z))) > largestRadius)
//                                                        {
//                                                            largestRadius = candidateRadius;
//                                                        }
//            }

//            model.Radius = largestRadius;

//            return MMReaderErrors.Success;
//        }

//        /// <summary>
//        /// Reads the ALST chunk containing VertexAttributes.
//        /// </summary>
//        /// <param name="model">The model to fill.</param>
//        /// <param name="parent">The parent chunk (MMDL).</param>
//        /// <param name="offset">The chunk-data-relative offset to the set of chunks contained in the parent chunk.</param>
//        /// <returns>An errorcode specifying the success of the operation.</returns>
//        private MMReaderErrors readALSTChunk(ref Model model, MDFChunkHeader parent, long offset)
//        {
//            MDFChunkHeader header = new MDFChunkHeader();
//            MMReaderErrors error;

//            header = reader.findNextChunk(parent, (Int32)MMChunkIds.ALST, offset);
//            if (header.offset == -1)
//                return MMReaderErrors.MissingVertexAttributesList;

//            reader.enterChunk(header);

//            int length = reader.readVInt32();

//            // There shouldn't be less than zero vertix attributes.
//            if (length < 0)
//                return MMReaderErrors.FileCorrupt;

//            model.VertexAttributes = new VertexAttributes[length];

//            for (int i = 0; i < length; i++)
//            {
//                model.VertexAttributes[i] = new VertexAttributes();
//                model.VertexAttributes[i].u = reader.readFloat();
//                model.VertexAttributes[i].v = reader.readFloat();
//                model.VertexAttributes[i].r = reader.readFloat();
//                model.VertexAttributes[i].g = reader.readFloat();
//                model.VertexAttributes[i].b = reader.readFloat();
//                if ((error = readVector(ref model.VertexAttributes[i].normal)) != MMReaderErrors.Success)
//                    return error;
//            }

//            return MMReaderErrors.Success;
//        }

//        /// <summary>
//        /// Reads the FLST chunk containing FaceSets.
//        /// </summary>
//        /// <param name="model">The model to fill.</param>
//        /// <param name="parent">The parent chunk (MMDL).</param>
//        /// <param name="offset">The chunk-data-relative offset to the set of chunks contained in the parent chunk.</param>
//        /// <returns>An errorcode specifying the success of the operation.</returns>
//        private MMReaderErrors readFLSTChunk(ref Model model, MDFChunkHeader parent, long offset)
//        {
//            MDFChunkHeader header = new MDFChunkHeader();
//            MMReaderErrors error;

//            header = reader.findNextChunk(parent, (Int32)MMChunkIds.FLST, offset);
//            if (header.offset == -1)
//                return MMReaderErrors.MissingFaceSetList;

//            reader.enterChunk(header);

//            int length = reader.readVInt32();

//            // There shouldn't be less than zero FaceSets.
//            if (length < 0)
//                return MMReaderErrors.FileCorrupt;

//            model.FaceSets = new FaceSet[length];

//            for (int i = 0; i < length; i++)
//            {
//                if ((error = readFSETChunk(ref model, i)) != MMReaderErrors.Success)
//                    return error;
//            }

//            return MMReaderErrors.Success;
//        }


//        /// <summary>
//        /// Reads the FSET chunk containing an individual FaceSet.
//        /// </summary>
//        /// <param name="model">The model to fill.</param>
//        /// <param name="index">The index into the list of FaceSets to write to.</param>
//        /// <returns>An errorcode specifying the success of the operation.</returns>
//        private MMReaderErrors readFSETChunk(ref Model model, int index)
//        {
//            MDFChunkHeader header = reader.peekChunkHeader();
//            MMReaderErrors error;

//            if (header.id != (Int32)MMChunkIds.FSET)
//                return MMReaderErrors.FileCorrupt;

//            reader.enterChunk(header);

//            FaceSet fs = new FaceSet();

//            fs.materialId = reader.readVInt32();
//            fs.material = reader.readString();
//            if ((error = readFCESChunk(ref fs)) != MMReaderErrors.Success)
//                return error;

//            model.FaceSets[index] = fs;

//            return MMReaderErrors.Success;
//        }


//        /// <summary>
//        /// Reads the FCES chunk containing a list of faces in a FaceSet.
//        /// </summary>
//        /// <param name="fs">the FaceSet to fill.</param>
//        /// <returns>An errorcode specifying the success of the operation.</returns>
//        private MMReaderErrors readFCESChunk(ref FaceSet fs)
//        {
//            MDFChunkHeader header = reader.peekChunkHeader();
//            MMReaderErrors error;

//            if (header.id != (Int32)MMChunkIds.FCES)
//                return MMReaderErrors.FileCorrupt;

//            reader.enterChunk(header);

//            int length = reader.readVInt32();

//            // There should not be less than zero faces.
//            if (length < 0)
//                return MMReaderErrors.FileCorrupt;

//            fs.faces = new Face[length];

//            for (int i = 0; i < length; i++)
//            {
//                fs.faces[i] = new Face();
//                fs.faces[i].vertices = reader.readVIntArray();
//                fs.faces[i].vertexAttributes = reader.readVIntArray();
//                if ((error = readVector(ref fs.faces[i].normal)) != MMReaderErrors.Success)
//                    return error;
//            }

//            return MMReaderErrors.Success;
//        }

//        /// <summary>
//        /// Reads a vector.
//        /// </summary>
//        /// <param name="v">The vector to read into.</param>
//        /// <returns>An errorcode specifying the success of the operation.</returns>
//        private MMReaderErrors readVector(ref Vector3D v)
//        {
//            v = new Vector3D();
//            v.X = reader.readFloat();
//            v.Y = reader.readFloat();
//            v.Z = reader.readFloat();

//            return MMReaderErrors.Success;
//        }

		

//        /// <summary>
//        /// Lists all of the codes corresponding to the chunk ids.
//        /// </summary>
//        private enum MMChunkIds
//        {
//            MM__ = 0x20204d4d,
//            MMDL = 0x4c444d4d,
//            VLST = 0x54534c56,
//            ALST = 0x54534c41,
//            VECT = 0x54434556,
//            FLST = 0x54534c46,
//            FSET = 0x54455346,
//            FCES = 0x53454346
//        }
//    }
//}
