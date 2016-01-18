using System;
using dng.ResumableUpload.Log;

namespace dng.ResumableUpload.EventHandlers
{
    public delegate void UploadLoggingHandler(LogLevel logLevel, string message, ChunkUploadParameter chunkUploadParameter, Exception exception);
}