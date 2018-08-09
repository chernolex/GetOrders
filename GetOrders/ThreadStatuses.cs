using System.Drawing;

namespace GetOrders
{
    public static class ThreadStatuses
    {
        public static string GetMessageByStatus(ThreadStatus status, string additionalInfo)
        {
            if (status == ThreadStatus.Success)
                return "Операция успешно завершена";
            if (status == ThreadStatus.Stopping)
                return "Остановка операции";
            if (status == ThreadStatus.Stopped)
                return "Операция остановлена";
            if (status == ThreadStatus.Connecting)
                return "Попытка соединения №" + additionalInfo;
            if (status == ThreadStatus.NoConnection)
                return "Не удалось подключиться";
            if (status == ThreadStatus.TryingToCopy)
                return "Попытка скопировать файлы";
            if (status == ThreadStatus.CopyingFile)
                return "Копируется файл " + additionalInfo;
            if (status == ThreadStatus.GettingFilesList)
                return "Получение списка файлов";
            if (status == ThreadStatus.NoFile)
                return "Файлы не найдены";
            if (status == ThreadStatus.FileCopied)
                return "Файл(ы) скопированы";
            if (status == ThreadStatus.Error)
                return "Ошибка при копировании";
            if (status == ThreadStatus.None)
                return "";

            return "Неизвестный статус";
        }

        public static ThreadStatusType GetThreadStatusTypeByStatus(ThreadStatus status)
        {
            if (status == ThreadStatus.FileCopied ||
                status == ThreadStatus.Success)
                return ThreadStatusType.Success;

            if (status == ThreadStatus.Connecting ||
                status == ThreadStatus.CopyingFile ||
                status == ThreadStatus.GettingFilesList ||
                status == ThreadStatus.TryingToCopy ||
                status == ThreadStatus.Stopping)
                return ThreadStatusType.Pending;

            if (status == ThreadStatus.Error ||
                status == ThreadStatus.NoConnection ||
                status == ThreadStatus.NoFile ||
                status == ThreadStatus.Stopped)
                return ThreadStatusType.Failed;

            return ThreadStatusType.Unknown;
        }

        public static Color GetColorByStatus(ThreadStatus status)
        {
            if (GetThreadStatusTypeByStatus(status) == ThreadStatusType.Success)
                return Color.YellowGreen;

            if (GetThreadStatusTypeByStatus(status) == ThreadStatusType.Failed)
                return Color.FromArgb(253, 100, 120);

            return Color.FromArgb(240, 240, 240);
        }
    }
}