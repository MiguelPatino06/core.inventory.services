namespace TShirt.InventoryApp.Services.FileWatcher
{
    partial class FileWatcherService
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.FSWatcher = new System.IO.FileSystemWatcher();
            ((System.ComponentModel.ISupportInitialize)(this.FSWatcher)).BeginInit();
            // 
            // FSWatcher
            // 
            this.FSWatcher.EnableRaisingEvents = true;
            ((System.ComponentModel.ISupportInitialize)(this.FSWatcher)).EndInit();
            this.FSWatcher.EnableRaisingEvents = true;
            //this.FSWatcher.IncludeSubdirectories = true;
            //this.FSWatcher.NotifyFilter = ((System.IO.NotifyFilters)((((((((System.IO.NotifyFilters.FileName | System.IO.NotifyFilters.DirectoryName)
            // | System.IO.NotifyFilters.Attributes)
            // | System.IO.NotifyFilters.Size)
            // | System.IO.NotifyFilters.LastWrite)
            // | System.IO.NotifyFilters.LastAccess)
            // | System.IO.NotifyFilters.CreationTime)
            // | System.IO.NotifyFilters.Security)));

            this.ServiceName = "TestCSWinWatcherService";
            ((System.ComponentModel.ISupportInitialize)(this.FSWatcher)).EndInit();
        }


        #endregion

        private System.IO.FileSystemWatcher FSWatcher;
    }
}
