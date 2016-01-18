using System.Text;

namespace dng.ResumableUpload
{
    public class ChunkUploadParameter
    {

        #region Public Constructors

        public ChunkUploadParameter(
            string identifier,
            string fileName,
            int currentChunkSize,
            int totalNumbersOfChunks,
            long totalSize,
            int chunkNumber,
            int chunkSize)
        {
            Identifier = identifier;
            FileName = fileName;
            ChunkNumber = chunkNumber;
            ChunkSize = chunkSize;
            CurrentChunkSize = currentChunkSize;
            TotalNumbersOfChunks = totalNumbersOfChunks;
            TotalSize = totalSize;
        }

        #endregion Public Constructors

        #region Public Properties

        public int ChunkNumber
        {
            get;
            private set;
        }

        public int ChunkSize
        {
            get;
            private set;
        }

        public int CurrentChunkSize
        {
            get;
            private set;
        }

        public string FileName
        {
            get;
            private set;
        }

        public string Identifier
        {
            get;
            private set;
        }

        public int TotalNumbersOfChunks
        {
            get;
            private set;
        }

        public long TotalSize
        {
            get;
            private set;
        }

        #endregion Public Properties

        #region Public Methods

        public new string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendFormat("Identifier: {0} ", Identifier);
            sb.AppendFormat("FileName: {0} ", FileName);
            sb.AppendFormat("TotalSize: {0} ", TotalSize);
            sb.AppendFormat("ChunkNumber: {0} ", ChunkNumber);
            sb.AppendFormat("TotalNumbersOfChunks: {0} ", TotalNumbersOfChunks);
            sb.AppendFormat("CurrentChunkSize: {0} ", CurrentChunkSize);
            sb.AppendFormat("ChunkSize: {0} ", ChunkSize);

            return sb.ToString();
        }

        #endregion Public Methods
    }
}