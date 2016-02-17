using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Web;
using System.Web.SessionState;
using dng.ResumableUpload.EventHandlers;
using dng.ResumableUpload.Exceptions;
using dng.ResumableUpload.Log;

namespace dng.ResumableUpload
{
    public class ChunkedFileUploadHandler : IHttpHandler, IReadOnlySessionState
    {

        #region Private Fields

        private readonly static string TargetPath;
        private readonly static string UploadBasePath;

        #endregion Private Fields

        #region Public Constructors

        static ChunkedFileUploadHandler()
        {
            UploadBasePath = ConfigurationManager.AppSettings["ChunkedFileUploadBasePath"];
            TargetPath = ConfigurationManager.AppSettings["ChunkedFileUploadTargetPath"];
        }

        #endregion Public Constructors

        #region Public Events

        public static event UploadExceptionHandler UploadException;

        public static event UploadSucceededHandler UploadSucceeded;

        #endregion Public Events

        #region Public Properties

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void ProcessRequest(
            HttpContext context)
        {
            ChunkUploadParameter chunkUploadParameter = null;
            try
            {
                if (context.Request.HttpMethod == "POST")
                {
                    chunkUploadParameter = InitChunkUploadParameter(context.Request.Form);

                    HandleUploadedChunk(context, chunkUploadParameter);

                    Logger.Info("Post Uploadedchunk done.", chunkUploadParameter);
                }
                else
                {
                    chunkUploadParameter = InitChunkUploadParameter(context.Request.QueryString);
                    CheckIfChunkExistAndIsComplete(context, chunkUploadParameter);

                    Logger.Info("Check if chunk exists done.", chunkUploadParameter);
                }
            }
            catch (Exception exception)
            {
                ChunkedUploadException chunkedUploadException;
                if (exception is ChunkedUploadException)
                {
                    chunkedUploadException = exception as ChunkedUploadException;
                }
                else
                {
                    chunkedUploadException = new ChunkedUploadException(
                        exception.Message,
                        exception,
                        chunkUploadParameter);
                }

                OnUploadError(chunkedUploadException);

                context.Response.StatusCode = 500;
            }
        }

        #endregion Public Methods

        #region Internal Methods

        internal static void OnUploadError(
            ChunkedUploadException chunkedUploadException)
        {
            Logger.Error(
                chunkedUploadException.Message,
                chunkedUploadException,
                chunkedUploadException.ChunkUploadParameter);

            if (UploadException != null)
                UploadException(chunkedUploadException);
        }

        internal static void OnUploadSucceeded(
            ChunkUploadParameter chunkUploadParameter)
        {
            if (UploadSucceeded != null)
                UploadSucceeded(chunkUploadParameter);
        }

        #endregion Internal Methods

        #region Private Methods

        private void CheckIfChunkExistAndIsComplete(
            HttpContext context,
            ChunkUploadParameter chunkUploadParameter)
        {
            var fileManager = new FileChunk(UploadBasePath, chunkUploadParameter);

            context.Response.StatusCode = fileManager.Exists() ? 200 : 204;
        }

        private void HandleUploadedChunk(
            HttpContext context, 
            ChunkUploadParameter chunkUploadParameter)
        {
            var fileManager = new FileChunk(UploadBasePath, chunkUploadParameter);

            if (context.Request.Files.Count == 0)
            {
                context.Response.StatusCode = 400;
                return;
            }

            fileManager.Save(context.Request.Files[0].InputStream);

            if (fileManager.CheckIfAllChunksUploaded() && fileManager.MergeAllChunks(TargetPath))
                OnUploadSucceeded(chunkUploadParameter);

            context.Response.StatusCode = 201;
        }

        private ChunkUploadParameter InitChunkUploadParameter(NameValueCollection parameters)
        {
            var identifier = parameters["flowIdentifier"].ToString();
            var fileName = parameters["flowFilename"].ToString();

            var chunkNumber = 0;
            int.TryParse(parameters["flowChunkNumber"].ToString(), out chunkNumber);

            var chunkSize = 0;
            int.TryParse(parameters["flowChunkSize"].ToString(), out chunkSize);

            var currentChunkSize = 0;
            int.TryParse(parameters["flowCurrentChunkSize"].ToString(), out currentChunkSize);

            int totalChunks = 0;
            int.TryParse(parameters["flowTotalChunks"].ToString(), out totalChunks);

            long totalSize = 0;
            long.TryParse(parameters["flowTotalSize"].ToString(), out totalSize);

            return new ChunkUploadParameter(identifier, fileName, currentChunkSize, totalChunks, totalSize, chunkNumber, chunkSize);
        }

        #endregion Private Methods

    }
}