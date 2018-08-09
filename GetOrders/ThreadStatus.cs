namespace GetOrders
{
    public enum ThreadStatus
    {
        Success,
        Stopping,
        Stopped,
        Connecting,
        NoConnection,
        TryingToCopy,
        CopyingFile,
        GettingFilesList,
        NoFile,
        FileCopied,
        Error,
        None
    }
}