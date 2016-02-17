using System;
using System.IO;
using dng.ResumableUpload.Exceptions;
using dng.ResumableUpload.Log;

namespace dng.ResumableUpload
{
    internal class FileChunk
    {

        #region Private Fields

        private readonly ChunkUploadParameter _chunkUploadParameter;
        private readonly string _uploadDirectory;

        #endregion Private Fields

        #region Public Constructors

        public FileChunk(
            string uploadDirectory,
            ChunkUploadParameter chunkUploadParameter)
        {
            _uploadDirectory = uploadDirectory;
            _chunkUploadParameter = chunkUploadParameter;
        }

        #endregion Public Constructors

        #region Internal Methods

        internal bool CheckIfAllChunksUploaded()
        {
            if (_chunkUploadParameter.ChunkNumber < _chunkUploadParameter.TotalNumbersOfChunks)
                return false;

            for (int chunkNumber = 1; chunkNumber <= _chunkUploadParameter.TotalNumbersOfChunks; chunkNumber++)
            {
                var chunkFilename = BuildChunkFileName();
                if (!File.Exists(chunkFilename))
                {
                    Logger.Debug("The chunk is missing. Filename:"  + chunkFilename, _chunkUploadParameter);
                    return false;
                }
            }

            Logger.Info("All chunks are uploaded.", _chunkUploadParameter);

            return true;
        }

        internal bool Exists()
        {
            var chunkFileName = BuildChunkFileName();

            if (File.Exists(chunkFileName))
            {
                var fileInfo = new FileInfo(chunkFileName);
                return fileInfo.Length == _chunkUploadParameter.ChunkSize;
            }

            return false;
        }

        internal bool MergeAllChunks(
            string targetPath)
        {
            Logger.Debug("Merging of chunks started.", _chunkUploadParameter);

            var newFilePath = Path.Combine(targetPath, _chunkUploadParameter.FileName);

            try
            {
                using (var sourceStream = new FileStream(newFilePath, FileMode.Create))
                {
                    for (int currentChunk = 1; currentChunk <= _chunkUploadParameter.TotalNumbersOfChunks; currentChunk++)
                    {
                        var currentChunkFileName = BuildChunkFileName(currentChunk);

                        using (Stream fromStream = File.OpenRead(currentChunkFileName))
                        {
                            fromStream.CopyTo(sourceStream);
                        }
                    }
                 }
            }
            catch (Exception exception)
            {
                throw new ChunkedUploadException("Merging of chunks failed.", exception, _chunkUploadParameter);
            }

            Logger.Info("Merging of chunks done.", _chunkUploadParameter);

            return true;
        }

        internal void Save(
            Stream inputStream)
        {
            Logger.Trace("Saving of chunkMerging of chunks started.", _chunkUploadParameter);

            var chunkPath = Path.Combine(_uploadDirectory, _chunkUploadParameter.Identifier.ToString());
            var chunkFileName = BuildChunkFileName();

            if (!Directory.Exists(chunkPath))
            {
                try
                {
                    Directory.CreateDirectory(chunkPath);
                }
                catch (Exception exception)
                {
                    throw new ChunkedUploadException("Saving of chunk failed.", exception, _chunkUploadParameter);
                }
            }

            try
            {
                using (Stream stream = new FileStream(chunkFileName, FileMode.Create))
                {
                    inputStream.CopyTo(stream);
                }
            }
            catch (Exception exception)
            {
                throw new ChunkedUploadException("Saving of chunk failed.", exception, _chunkUploadParameter);
            }

            Logger.Info("Saving of chunkMerging of chunks done.", _chunkUploadParameter);
        }

        #endregion Internal Methods

        #region Private Methods

        private string BuildChunkFileName()
        {
            return BuildChunkFileName(_chunkUploadParameter.ChunkNumber);
        }

        private string BuildChunkFileName(
            int chunkNumber)
        {
            return Path.Combine(
                _uploadDirectory,
                _chunkUploadParameter.Identifier.ToString(),
                string.Concat(chunkNumber.ToString(), "-", _chunkUploadParameter.FileName));
        }

        #endregion Private Methods

    }
}