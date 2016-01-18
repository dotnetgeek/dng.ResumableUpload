using dng.ResumableUpload.Exceptions;

namespace dng.ResumableUpload.EventHandlers
{
    public delegate void UploadExceptionHandler(ChunkedUploadException chunkedUploadException);
}