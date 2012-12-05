namespace Mila.Utility.MM
{
    /// <summary>
    /// Enumerates all of the different errors that can occur during reading.
    /// </summary>
    public enum MMReaderErrors
    {
        Success = 0,
        FileFormatUnrecognized = 1,
        IncompatibleVersion = 2,
        FileCorrupt = 3,
        MissingVertexList = 4,
        MissingVertexAttributesList = 5,
        MissingFaceSetList = 6
    }

    /// <summary>
    /// Contains a list of errors that can arise while trying to write the Model.
    /// </summary>
    public enum MMWriterErrors
    {
        Success = 0
    }
}