using System;

namespace dng.ResumableUpload.Exceptions
{
    public class ChunkedUploadException : Exception
    {

        #region Public Constructors

        public ChunkedUploadException()
        {
        }

        public ChunkedUploadException(
            string message,
            ChunkUploadParameter chunkUploadParameter)
            : base(message)
        {
            ChunkUploadParameter = chunkUploadParameter;
        }

        public ChunkedUploadException(
            string message,
            Exception innerException,
            ChunkUploadParameter chunkUploadParameter)
            : base(message, innerException)
        {
            ChunkUploadParameter = chunkUploadParameter;
        }

        #endregion Public Constructors

        #region Public Properties

        public ChunkUploadParameter ChunkUploadParameter
        {
            get;
            private set;
        }

        #endregion Public Properties

    }
}