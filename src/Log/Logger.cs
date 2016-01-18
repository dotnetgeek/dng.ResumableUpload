using System;
using dng.ResumableUpload.EventHandlers;

namespace dng.ResumableUpload.Log
{
    public static class Logger
    {
        #region Public Events

        public static event UploadLoggingHandler UploadLogging;

        #endregion Public Events

        #region Internal Methods

        internal static void Debug(
            string message,
            ChunkUploadParameter chunkUploadParameter)
        {
            OnLogging(LogLevel.Debug, message, chunkUploadParameter);
        }

        internal static void Error(
            string message,
            Exception exception,
            ChunkUploadParameter chunkUploadParameter)
        {
            OnLogging(LogLevel.Error, message, chunkUploadParameter, exception);
        }

        internal static void Info(
            string message,
            ChunkUploadParameter chunkUploadParameter)
        {
            OnLogging(LogLevel.Info, message, chunkUploadParameter);
        }

        internal static void Trace(
            string message,
            ChunkUploadParameter chunkUploadParameter)
        {
            OnLogging(LogLevel.Trace, message, chunkUploadParameter);
        }

        internal static void Warning(
            string message,
            ChunkUploadParameter chunkUploadParameter)
        {
            OnLogging(LogLevel.Warn, message, chunkUploadParameter);
        }

        #endregion Internal Methods

        #region Private Methods

        private static void OnLogging(
            LogLevel logLevel,
            string message,
            ChunkUploadParameter chunkUploadParameter,
            Exception exception = null)
        {
            if (UploadLogging != null)
                UploadLogging(logLevel, message, chunkUploadParameter, exception);
        }

        #endregion Private Methods
    }
}