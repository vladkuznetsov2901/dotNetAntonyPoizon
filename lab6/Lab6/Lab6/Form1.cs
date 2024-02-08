using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Lab6
{
    public interface IFileOperation
    {
        // Check if the file should be processed
        bool Accept(string fileName);

        // File handler function
        void Process(string fileName);
    }

    public partial class Form1 : Form
    {
        private readonly string monitoredDirectory = @"D:\Studies\test";
        private readonly List<IFileOperation> fileOperations;

        public Form1()
        {
            InitializeComponent();

            // Initialize the list of file operations
            fileOperations = new List<IFileOperation>
            {
                new TextFileHandler(),
                new ImageFileHandler(),
                // Add more handlers as needed
            };

            // Start monitoring the directory
            StartMonitoring();
        }

        private void StartMonitoring()
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = monitoredDirectory;

            // Subscribe to the Created event
            watcher.Created += OnFileCreated;

            // Enable the watcher
            watcher.EnableRaisingEvents = true;
        }

        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            // Check each file operation to see if it accepts the file
            foreach (var operation in fileOperations)
            {
                if (operation.Accept(e.Name))
                {
                    // If accepted, process the file
                    operation.Process(e.FullPath);
                }
            }
        }

        // Example file handler for text files
        public class TextFileHandler : IFileOperation
        {
            public bool Accept(string fileName)
            {
                return fileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase);
            }

            public void Process(string fileName)
            {
                // Add your text file handling logic here
                MessageBox.Show($"Processing text file: {fileName}");
            }
        }

        // Example file handler for image files
        public class ImageFileHandler : IFileOperation
        {
            public bool Accept(string fileName)
            {
                return fileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                       fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase);
            }

            public void Process(string fileName)
            {
                // Add your image file handling logic here
                MessageBox.Show($"Processing image file: {fileName}");
            }
        }
    }
}
